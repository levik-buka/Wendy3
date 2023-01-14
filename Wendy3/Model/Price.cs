using Microsoft.SqlServer.Server;
using System;
using System.Globalization;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    /// <summary>
    /// Price with and without VAT
    /// </summary>
    public class Price
    {
        /// <summary>
        /// Price printing formats
        /// </summary>
        public enum ShowFormat
        {
            /// <summary>
            /// Print price without VAT
            /// </summary>
            withoutVAT,
            /// <summary>
            /// Print price with VAT
            /// </summary>
            withVAT,
            /// <summary>
            /// Print price with and without VAT
            /// </summary>
            both
        }

        /// <summary>
        /// VAT percentage
        /// </summary>
        public decimal VAT { get; set; }
        /// <summary>
        /// VATless fee
        /// </summary>
        public decimal VATLess { get; set; }
        /// <summary>
        /// fee including VAT
        /// </summary>
        public decimal WithVAT
        {
            get { return VATLess.AddVAT(VAT).RoundToCents(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VATLessPrice"></param>
        /// <param name="VAT"></param>
        public Price(decimal VATLessPrice, decimal VAT)
        {
            this.VAT = VAT;
            this.VATLess = VATLessPrice;
        }

        /// <summary>
        /// Print price without VAT
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(ShowFormat.withoutVAT);
        }

        /// <summary>
        /// Returns VAT and VATless fees
        /// </summary>
        /// <returns></returns>
        public string ToString(ShowFormat fmt)
        {
            if (fmt == ShowFormat.withoutVAT)
            {
                return String.Format(new NumberFormatInfo(), $"{VATLess,10:C2}");
            }
            else if (fmt == ShowFormat.withVAT)
            {
                return String.Format(new NumberFormatInfo(), $"{WithVAT,10:C2}");
            }

            return String.Format(new NumberFormatInfo(), $"{VATLess,10:C2} / {WithVAT,10:C2}");
        }
    }
}