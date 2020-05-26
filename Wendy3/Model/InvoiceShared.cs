using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    /// <summary>
    /// Invoice object with common invoice data and user invoice data
    /// </summary>
    public class InvoiceShared : DateRange
    {
        /// <summary>
        /// Indicates is invoice estimated or not
        /// </summary>
        public bool Balanced { get; set; }

        /// <summary>
        /// Common invoice data
        /// </summary>
        [Newtonsoft.Json.JsonProperty("CommonInvoice")]
        public Invoice CommonInvoice { get; private set; }
        /// <summary>
        /// List of user invoices
        /// </summary>
        public List<UserInvoice> UserInvoices { get; } = new List<UserInvoice>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public InvoiceShared(long id, DateTime startDate, DateTime? endDate) 
        {
            Id = id;
            Start = startDate;
            End = endDate;

            CommonInvoice = new Invoice(endDate ?? startDate, 0, 0);
        }

        /// <summary>
        /// Creates empty invoice
        /// </summary>
        /// <returns></returns>
        public static InvoiceShared CreateEmpty()
        {
            return new InvoiceShared(0, new DateTime(), null);
        }

        /// <summary>
        /// Return read-out date of common invoice
        /// </summary>
        /// <returns></returns>
        public DateTime GetReadOutDate()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetReadOutDate();
        }

        /// <summary>
        /// Return read-outs of common invoice
        /// </summary>
        /// <returns></returns>
        public ConsumptionValue GetReadOut()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetReadOut();
        }

        /// <summary>
        /// Return consumption of common invoice
        /// </summary>
        /// <returns></returns>
        public ConsumptionValue GetConsumption()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetConsumption();
        }

        /// <summary>
        /// Return basic water fee of common invoice
        /// </summary>
        /// <returns></returns>
        public WaterFee GetBasicFee()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetBasicFee();
        }

        /// <summary>
        /// Return usage water fee of common invoice
        /// </summary>
        /// <returns></returns>
        public WaterFee GetUsageFee()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetUsageFee();
        }

        /// <summary>
        /// Return fee plan used by invoice
        /// </summary>
        /// <returns></returns>
        public FeeConfig GetFeeConfig()
        {
            return CommonInvoice.GetFeeConfig();
        }

        /// <summary>
        /// Set fee plan used by invoice
        /// </summary>
        /// <param name="feeConfig"></param>
        public void SetFeeConfig(FeeConfig feeConfig)
        {
            CommonInvoice.SetFeeConfig(feeConfig);
            UserInvoices.ForEach(userInvoice => userInvoice.SetFeeConfig(feeConfig));
        }

        /// <summary>
        /// Return total water fee of common invoice
        /// </summary>
        /// <returns></returns>
        public TotalFee GetTotalFee()
        {
            return CommonInvoice.GetTotalFee();
        }
    }
}
