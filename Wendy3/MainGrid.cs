using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
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
            catch (FileNotFoundException fnfEx)
            {
                try // old wendy.xml file
                {
                    StreamReader xmlReader = File.OpenText(@"wendy.xml");
                    var oldWendyData = XmlUtil.DeserializeXML<Model.Wendy1.OldWendyFile>(xmlReader, nameSpace: null);
                    invoiceHistory = Tasks.Converters.OldWendyFileConverter.ToInvoiceHistory(oldWendyData);

                    new ConsumptionCalculator(invoiceHistory).CalculateConsumption();
                }
                catch (Exception ex)
                {
                    ShowErrorStatus(ex, fnfEx.Message);
                }
            }

            ShowInvoiceHistory(invoiceHistory);
        }

        private void ShowInvoiceHistory(InvoiceHistory invoiceHistory)
        {
            dataGridView.Rows.Clear();

            if (invoiceHistory == null) return;

            foreach(InvoiceShared invoice in invoiceHistory.Invoices)
            {
                FeeConfig feeConfig = invoiceHistory.FeeConfigHistory.GetFeeConfigHistoryForPeriod(invoice).FirstOrDefault();
                ShowCommonInvoice(invoice, feeConfig);

                foreach(UserInvoice userInvoice in invoice.UserInvoices)
                {
                    ShowUserInvoice(invoice, userInvoice, feeConfig);
                }
            }

            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ShowCommonInvoice(InvoiceShared invoice, FeeConfig feeConfig)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(feeConfig != null);

            var row = new DataGridViewRow();
            row.CreateCells(dataGridView);
            row.DefaultCellStyle.BackColor = Color.LightCyan;

            row.Cells[0].Value = invoice.Id;
            row.Cells[1].Value = invoice.PeriodToString();
            row.Cells[2].Value = "Yhteinen";
            row.Cells[3].Value = invoice.GetReadOut().ToString();
            row.Cells[4].Value = invoice.GetConsumption().ToString();
            row.Cells[5].Value = invoice.GetBasicFee().ToString();
            row.Cells[6].Value = invoice.GetUsageFee().ToString();
            row.Cells[7].Value = invoice.Balanced;
            row.Cells[8].Value = invoice.GetTotalFee(feeConfig.VAT).ToString();

            dataGridView.Rows.Add(row);
        }

        private void ShowUserInvoice(InvoiceShared invoice, UserInvoice userInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(userInvoice != null);
            Contract.Requires(feeConfig != null);

            var row = new DataGridViewRow();
            row.CreateCells(dataGridView);

            row.Cells[0].Value = userInvoice.InvoiceOwner;
            row.Cells[1].Style.BackColor = Color.LightGray;
            row.Cells[2].Value = userInvoice.InvoiceOwner;
            row.Cells[3].Value = userInvoice.GetReadOut().ToString();
            row.Cells[4].Value = userInvoice.GetConsumption().ToString();
            row.Cells[5].Value = userInvoice.GetBasicFee().ToString();
            row.Cells[6].Value = userInvoice.GetUsageFee().ToString();
            row.Cells[7].Value = invoice.Balanced;
            row.Cells[8].Value = userInvoice.GetTotalFee(feeConfig.VAT).ToString();

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
    }
}