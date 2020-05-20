using System;
using System.Globalization;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    public class TotalFee
    {
        public decimal VAT { get; set; }
        public decimal VATLessFee { get; set; }

        public TotalFee(decimal VATLessFee, decimal VAT)
        {
            this.VAT = VAT;
            this.VATLessFee = VATLessFee;
        }

        public override string ToString()
        {
            return String.Format(new NumberFormatInfo(),
                $"{VATLessFee,10:C2} / {WithVAT(),10:C2}");
        }

        public decimal WithVAT()
        {
            return (VATLessFee * (1m + VAT / 100)).RoundToCents();
        }
    }
}