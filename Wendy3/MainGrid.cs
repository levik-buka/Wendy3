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

//#pragma warning disable CA1303 // #warning directive

namespace Wendy
{
    internal partial class MainGrid : Form
    {
        private InvoiceHistory invoiceHistory;
        private bool anyChangesToInvoiceHistory;

        /// <summary>
        /// Default constructor
        /// </summary>
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

            ShowInvoiceHistory(invoiceHistory, GetPriceShowFormat());
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

        /// <summary>
        /// Process special keys: delete, escape
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
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

        private void ShowInvoiceHistory(InvoiceHistory invoiceHistory, Price.ShowFormat priceFormat)
        {
            dataGridView.Rows.Clear();

            if (invoiceHistory == null) return;

            foreach(InvoiceShared invoice in invoiceHistory.Invoices)
            {
                FeeConfig feeConfig = invoiceHistory.FeeConfigHistory.GetFeeConfigHistoryForPeriod(invoice).FirstOrDefault();
                invoice.SetFeeConfig(feeConfig);

                ShowSharedInvoice(invoice, priceFormat);
            }

            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void RefreshDataGridViewRows(DataGridViewRowCollection rows, Price.ShowFormat priceFormat)
        {
            foreach(DataGridViewRow row in rows)
            {
                InvoiceShared invoice = GetCommonInvoiceFromRow(row);

                if (IsUserInvoice(row))
                {
                    ShowUserInvoice(row, invoice, GetUserInvoiceFromRow(row), priceFormat);
                }
                else
                {
                    ShowCommonInvoice(row, invoice, priceFormat);
                }
            }
        }


        private void ShowSharedInvoice(InvoiceShared invoice, Price.ShowFormat priceFormat)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView);

            ShowCommonInvoice(row, invoice, priceFormat);
            dataGridView.Rows.Add(row);

            foreach (UserInvoice userInvoice in invoice.UserInvoices)
            {
                row = new DataGridViewRow();
                row.CreateCells(dataGridView);

                ShowUserInvoice(row, invoice, userInvoice, priceFormat);
                dataGridView.Rows.Add(row);
            }
        }


        private static void ShowCommonInvoice(DataGridViewRow row, InvoiceShared invoice, Price.ShowFormat priceFormat)
        {
            Contract.Requires(row != null);
            Contract.Requires(invoice != null);

            row.DefaultCellStyle.BackColor = Color.LightCyan;

            row.Cells[0].Value = invoice.Id;
            row.Cells[0].Tag = invoice.Id;
            row.Cells[1].Value = invoice.PeriodToString();
            row.Cells[2].Value = "Yhteinen";
            row.Cells[3].Value = invoice.GetReadOut().ToString();
            row.Cells[4].Value = invoice.GetConsumption().ToString();
            row.Cells[5].Value = invoice.GetBasicFee().ToString(priceFormat);
            row.Cells[6].Value = invoice.GetUsageFee().ToString(priceFormat);
            row.Cells[7].Value = invoice.Balanced;
            row.Cells[8].Value = invoice.GetTotalPrice().ToString(Price.ShowFormat.both);

            row.Tag = invoice.CommonInvoice;
        }

        private static void ShowUserInvoice(DataGridViewRow row, InvoiceShared invoice, UserInvoice userInvoice, Price.ShowFormat priceFormat)
        {
            Contract.Requires(row != null);
            Contract.Requires(invoice != null);
            Contract.Requires(userInvoice != null);

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
            
            row.Cells[5].Value = userInvoice.GetBasicFee().ToString(priceFormat);
            row.Cells[5].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(
                    inv => (inv.GetBasicFee().GetTotalPrice().VATLess), 
                    userInv => (userInv.GetBasicFee().GetTotalPrice().VATLess)) ?
                Color.LightPink : row.Cells[5].Style.BackColor;
            
            row.Cells[6].Value = userInvoice.GetUsageFee().ToString(priceFormat);
            row.Cells[6].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(
                    inv => (inv.GetUsageFee().GetTotalPrice().VATLess), 
                    userInv => (userInv.GetUsageFee().GetTotalPrice().VATLess)) ?
                Color.LightPink : row.Cells[6].Style.BackColor;
            
            row.Cells[7].Value = invoice.Balanced;
            
            row.Cells[8].Value = userInvoice.GetTotalPrice().ToString(Price.ShowFormat.both);
            row.Cells[8].Style.BackColor = invoice.IsSumOfUserInvoicesDifferent(
                    inv => (inv.GetTotalPrice().VATLess + inv.GetTotalPrice().WithVAT),
                    userInv => (userInv.GetTotalPrice().VATLess + userInv.GetTotalPrice().WithVAT)) ?
                Color.LightPink : row.Cells[8].Style.BackColor;

            row.Tag = userInvoice;
        }

        private void ShowErrorStatus(Exception ex, string description)
        {
            selectedSumStatus.Text = description + " " + ex.Message;
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
                VATLessSum += invoice.GetTotalPrice().VATLess;
                withVATSum += invoice.GetTotalPrice().WithVAT;
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
                    foreach (var _ in invoice.UserInvoices)
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

        private static UserInvoice GetUserInvoiceFromRow(DataGridViewRow row)
        {
            if (IsUserInvoice(row) == false) return null;

            return row.Tag as UserInvoice;
        }

        private static bool IsUserInvoice(DataGridViewRow row)
        {
            // tag is invoice id, value is owner name for user invoices
            return row.Cells[0].Value is string;
        }

        private Price.ShowFormat GetPriceShowFormat()
        {
            if (showPricesWithVAT.Checked) return Price.ShowFormat.withVAT;

            return Price.ShowFormat.withoutVAT;
        }

        private void ShowPricesWithVAT_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDataGridViewRows(dataGridView.Rows, GetPriceShowFormat());
        }


    }
}