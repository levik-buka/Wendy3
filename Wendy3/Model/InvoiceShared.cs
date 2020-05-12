using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class InvoiceShared : DateRange
    {
        private Invoice commonInvoice;
        public List<UserInvoice> UserInvoices { get; }

        public InvoiceShared(long id, DateTime startDate, DateTime? endDate) 
        {
            Id = id;
            Start = startDate;
            End = endDate;

            commonInvoice = new Invoice
            {
                ReadOut = new ConsumptionReadOut(endDate ?? startDate, 0, 0)
            };
        }

        virtual public bool IsBalanced() { return false; }

        public void SetReadOut(ulong estimatedReadout, ulong realReadOut)
        {
            commonInvoice.ReadOut.ReadOut.Estimated = estimatedReadout;
            commonInvoice.ReadOut.ReadOut.Real = realReadOut;

            commonInvoice.ReadOut.ResetConsumption();
            ResetReadOutOfUserInvoices();
        }

        public void SetBasicFee(decimal cleanWaterFee, decimal wasteWaterFee)
        {
            commonInvoice.BasicFee = new WaterFee(cleanWaterFee, wasteWaterFee);
            ResetFeesOfUserInvoices();
        }
        public void SetUsageFee(decimal cleanWaterFee, decimal wasteWaterFee)
        {
            commonInvoice.UsageFee = new WaterFee(cleanWaterFee, wasteWaterFee);
            ResetFeesOfUserInvoices();
        }

        private void ResetFeesOfUserInvoices()
        {
            UserInvoices?.ForEach(delegate(UserInvoice invoice) 
            {
                invoice.BasicFee = null;
                invoice.UsageFee = null;
            });
        }

        private void ResetReadOutOfUserInvoices()
        {
            UserInvoices?.ForEach(delegate(UserInvoice invoice) 
            {
                invoice.ReadOut.ResetReadOut();
            });
        }
    }
}
