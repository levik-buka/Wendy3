using System;
using System.Globalization;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    /// <summary>
    /// Total VAT and VATless fee
    /// </summary>
    public class TotalFee
    {
        /// <summary>
        /// VAT percentage
        /// </summary>
        public decimal VAT { get; set; }
        /// <summary>
        /// Total VATless fee
        /// </summary>
        public decimal VATLessFee { get; set; }
        /// <summary>
        /// Total fee including VAT
        /// </summary>
        public decimal WithVAT
        {
            get { return (VATLessFee * (1m + VAT / 100)).RoundToCents(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VATLessFee"></param>
        /// <param name="VAT"></param>
        public TotalFee(decimal VATLessFee, decimal VAT)
        {
            this.VAT = VAT;
            this.VATLessFee = VATLessFee;
        }

        /// <summary>
        /// Returns VAT and VATless fees
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(new NumberFormatInfo(),
                $"{VATLessFee,10:C2} / {WithVAT,10:C2}");
        }
    }
}