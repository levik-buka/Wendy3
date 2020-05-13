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
        private Invoice CommonInvoice { get; set; }
        public List<UserInvoice> UserInvoices { get; } = new List<UserInvoice>();

        public InvoiceShared(long id, DateTime startDate, DateTime? endDate) 
        {
            Id = id;
            Start = startDate;
            End = endDate;

            CommonInvoice = new Invoice
            {
                ReadOut = new ConsumptionReadOut(endDate ?? startDate, 0, 0)
            };
        }

        virtual public bool IsBalanced() { return false; }

        public DateTime GetReadOutDate()
        {
            return CommonInvoice.ReadOut.ReadOutDate;
        }

        public void SetReadOut(ulong estimatedReadout, ulong realReadOut)
        {
            CommonInvoice.ReadOut.SetReadOut(estimatedReadout, realReadOut);
        }
        public ConsumptionValue GetReadOut()
        {
            return CommonInvoice.ReadOut.GetReadOut();
        }

        public void SetBasicFee(decimal cleanWaterFee, decimal wasteWaterFee)
        {
            CommonInvoice.BasicFee = new WaterFee(cleanWaterFee, wasteWaterFee);
        }
        public WaterFee GetBasicFee()
        {
            return CommonInvoice.BasicFee;
        }

        public void SetUsageFee(decimal cleanWaterFee, decimal wasteWaterFee)
        {
            CommonInvoice.UsageFee = new WaterFee(cleanWaterFee, wasteWaterFee);
        }
        public WaterFee GetUsageFee()
        {
            return CommonInvoice.UsageFee;
        }
    }
}
