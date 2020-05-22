﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class InvoiceShared : DateRange
    {
        public bool Balanced { get; set; }

        [Newtonsoft.Json.JsonProperty("CommonInvoice")]
        public Invoice CommonInvoice { get; private set; }
        public List<UserInvoice> UserInvoices { get; } = new List<UserInvoice>();

        public InvoiceShared(long id, DateTime startDate, DateTime? endDate) 
        {
            Id = id;
            Start = startDate;
            End = endDate;

            CommonInvoice = new Invoice(endDate ?? startDate, 0, 0);
        }

        public static InvoiceShared CreateEmpty()
        {
            return new InvoiceShared(0, new DateTime(), null);
        }

        public DateTime GetReadOutDate()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetReadOutDate();
        }

        public ConsumptionValue GetReadOut()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetReadOut();
        }

        public ConsumptionValue GetConsumption()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetConsumption();
        }

        public WaterFee GetBasicFee()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetBasicFee();
        }

        public WaterFee GetUsageFee()
        {
            Contract.Requires(CommonInvoice != null);

            return CommonInvoice.GetUsageFee();
        }

        public FeeConfig GetFeeConfig()
        {
            return CommonInvoice.GetFeeConfig();
        }

        public void SetFeeConfig(FeeConfig feeConfig)
        {
            CommonInvoice.SetFeeConfig(feeConfig);
            UserInvoices.ForEach(userInvoice => userInvoice.SetFeeConfig(feeConfig));
        }

        public TotalFee GetTotalFee()
        {
            return CommonInvoice.GetTotalFee();
        }
    }
}
