using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wendy.Logic.Calculators;
using Wendy.Model;
using Wendy.Tasks.Extensions;
using Wendy.Tasks.Utils;

namespace Wendy
{
    public partial class MainGrid : Form
    {
        private InvoiceHistory invoiceHistory;
        private bool anyChangesToInvoiceHistory = false;

        public MainGrid()
        {
            InitializeComponent();
        }

        private void MainGrid_Load(object sender, EventArgs e)
        {
            try // new wendy.json file first
            {
                StreamReader jsonReader = File.OpenText(@"wendy.json");
                invoiceHistory = JsonUtil.DeserializeJson<InvoiceHistory>(jsonReader);
            }
            catch (FileNotFoundException jsonException)
            {
                try // old wendy.xml file
                {
                    StreamReader xmlReader = File.OpenText(@"wendy.xml");
                    var oldWendyData = XmlUtil.DeserializeXML<Model.Wendy1.OldWendyFile>(xmlReader, nameSpace: null);
                    invoiceHistory = Tasks.Converters.OldWendyFileConverter.ToInvoiceHistory(oldWendyData);

                    new ConsumptionCalculator(invoiceHistory).CalculateConsumption();
                }
                catch (FileNotFoundException xmlException)
                {
                    ShowErrorStatus(xmlException, jsonException.Message);
                }
            }

            ShowInvoiceHistory(invoiceHistory);
        }

        private void MainGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!anyChangesToInvoiceHistory) return;

            if (MessageBox.Show("Tallennetaanko muutokset?", "Lopetus", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                try
                {
                    File.Move(@"wendy.json", @"wendy.json.bak");
                }
                catch (FileNotFoundException) { }

                string json = JsonUtil.SerializeToJson(invoiceHistory);
                File.WriteAllText(@"wendy.json", json);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Delete:
                    {
                        DeleteSelectedInvoices();
                        return true;
                    }

                case Keys.Escape:
                    {
                        Close();
                        return true;
                    }

                default:
                    return false;
            }
        }

        private void ShowInvoiceHistory(InvoiceHistory invoiceHistory)
        {
            dataGridView.Rows.Clear();

            if (invoiceHistory == null) return;

            foreach(InvoiceShared invoice in invoiceHistory.Invoices)
            {
                FeeConfig feeConfig = invoiceHistory.FeeConfigHistory.GetFeeConfigHistoryForPeriod(invoice).FirstOrDefault();
                invoice.SetFeeConfig(feeConfig);

                ShowCommonInvoice(invoice);

                foreach(UserInvoice userInvoice in invoice.UserInvoices)
                {
                    ShowUserInvoice(invoice, userInvoice);
                }
            }

            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ShowCommonInvoice(InvoiceShared invoice)
        {
            Contract.Requires(invoice != null);

            var row = new DataGridViewRow();
            row.CreateCells(dataGridView);
            row.DefaultCellStyle.BackColor = Color.LightCyan;

            row.Cells[0].Value = invoice.Id;
            row.Cells[0].Tag = invoice.Id;
            row.Cells[1].Value = invoice.PeriodToString();
            row.Cells[2].Value = "Yhteinen";
            row.Cells[3].Value = invoice.GetReadOut().ToString();
            row.Cells[4].Value = invoice.GetConsumption().ToString();
            row.Cells[5].Value = invoice.GetBasicFee().ToString();
            row.Cells[6].Value = invoice.GetUsageFee().ToString();
            row.Cells[7].Value = invoice.Balanced;
            row.Cells[8].Value = invoice.GetTotalFee().ToString();

            row.Tag = invoice.CommonInvoice;
            dataGridView.Rows.Add(row);
        }

        private void ShowUserInvoice(InvoiceShared invoice, UserInvoice userInvoice)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(userInvoice != null);

            var row = new DataGridViewRow();
            row.CreateCells(dataGridView);

            row.Cells[0].Value = userInvoice.InvoiceOwner;
            row.Cells[0].Tag = invoice.Id;
            row.Cells[1].Style.BackColor = Color.LightGray;

            row.Cells[2].Value = userInvoice.InvoiceOwner;

            row.Cells[3].Value = userInvoice.GetReadOut().ToString();
            row.Cells[3].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(inv => inv.GetReadOut().Sum(), userInv => userInv.GetReadOut().Sum()) ? 
                Color.LightPink : row.Cells[3].Style.BackColor;
            
            row.Cells[4].Value = userInvoice.GetConsumption().ToString();
            row.Cells[4].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(inv => inv.GetConsumption().Sum(), userInv => userInv.GetConsumption().Sum()) ?
                Color.LightPink : row.Cells[4].Style.BackColor;
            
            row.Cells[5].Value = userInvoice.GetBasicFee().ToString();
            row.Cells[5].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(
                    inv => (inv.GetBasicFee().GetTotalFee(inv.GetFeeConfig().VAT).VATLessFee), 
                    userInv => (userInv.GetBasicFee().GetTotalFee(userInv.GetFeeConfig().VAT).VATLessFee)) ?
                Color.LightPink : row.Cells[5].Style.BackColor;
            
            row.Cells[6].Value = userInvoice.GetUsageFee().ToString();
            row.Cells[6].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(
                    inv => (inv.GetUsageFee().GetTotalFee(inv.GetFeeConfig().VAT).VATLessFee), 
                    userInv => (userInv.GetUsageFee().GetTotalFee(userInv.GetFeeConfig().VAT).VATLessFee)) ?
                Color.LightPink : row.Cells[6].Style.BackColor;
            
            row.Cells[7].Value = invoice.Balanced;
            
            row.Cells[8].Value = userInvoice.GetTotalFee().ToString();
            row.Cells[8].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(
                    inv => (inv.GetTotalFee().VATLessFee + inv.GetTotalFee().WithVAT),
                    userInv => (userInv.GetTotalFee().VATLessFee + userInv.GetTotalFee().WithVAT)) ?
                Color.LightPink : row.Cells[8].Style.BackColor;

            row.Tag = userInvoice;
            dataGridView.Rows.Add(row);
        }

        private void ShowErrorStatus(Exception ex, string description)
        {
            selectedSumStatus.Text = description + " " + ex.Message;
        }

        private void ClearErrorStatus()
        {
            selectedSumStatus.Text = string.Empty;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            CalculateSumOfSelectedInvoices();
        }

        private void CalculateSumOfSelectedInvoices()
        {
            decimal VATLessSum = 0;
            decimal withVATSum = 0;

            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                Invoice invoice = row.Tag as Invoice;
                VATLessSum += invoice.GetTotalFee().VATLessFee;
                withVATSum += invoice.GetTotalFee().WithVAT;
            }

            selectedSumStatus.Text = String.Format(new NumberFormatInfo(), $"{VATLessSum:C2} + {(withVATSum - VATLessSum):C2} (alv) = {withVATSum:C2}");
        }

        private void DeleteSelectedInvoices()
        {
            if (dataGridView.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Poistetaanko valitut laskut?", "Posto", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                return;
            }

            anyChangesToInvoiceHistory = true;

            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                InvoiceShared invoice = GetCommonInvoiceFromRow(row);

                if (IsUserInvoice(row))
                {
                    UserInvoice userInvoice = GetUserInvoiceFromRow(row);
                    invoice.UserInvoices.Remove(userInvoice);
                }
                else
                {
                    foreach (var userInvoice in invoice.UserInvoices)
                    {
                        // removing user invoices from datagrid
                        dataGridView.Rows.RemoveAt(row.Index + 1);
                    }

                    invoiceHistory.Invoices.Remove(invoice);
                }

                dataGridView.Rows.Remove(row);
            }
        }

        private InvoiceShared GetCommonInvoiceFromRow(DataGridViewRow row)
        {
            long invoiceId = (long)row.Cells[0].Tag;
            return invoiceHistory.Invoices.GetInvoiceById(invoiceId);
        }

        private UserInvoice GetUserInvoiceFromRow(DataGridViewRow row)
        {
            if (IsUserInvoice(row) == false) return null;

            return row.Tag as UserInvoice;
        }

        private bool IsUserInvoice(DataGridViewRow row)
        {
            // tag is invoice id, value is owner name for user invoices
            return row.Cells[0].Value is string;
        }
    }
}