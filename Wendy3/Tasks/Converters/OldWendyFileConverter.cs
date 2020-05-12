﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Tasks.Converters
{
    public static class OldWendyFileConverter
    {
        public static Model.InvoiceHistory ToInvoiceHistory(Model.Wendy1.OldWendyFile oldWendyData)
        {
            Contract.Requires(oldWendyData != null);

            var invoiceHistory = new Model.InvoiceHistory();

            var mainMeterConfig = ToMeterConfig(oldWendyData.Config) ?? Model.MeterConfig.CreateEmpty();
            invoiceHistory.MainMeterConfigHistory.MeterConfigs.Add(mainMeterConfig);

            ConvertUserNamesToUserMeterConfigHistory(oldWendyData.UserNames, invoiceHistory.UserMeterConfigHistory);

            foreach (var configFee in oldWendyData.ConfigFees)
            {
                invoiceHistory.FeeConfigHistory.Add(ToFeeConfig(configFee));
            }

            foreach (var invoice in oldWendyData.Invoices)
            {
                invoiceHistory.Invoices.Add(ToInvoiceShared(invoice));
            }

            return invoiceHistory;
        }

        private static Model.InvoiceShared ToInvoiceShared(Model.Wendy1.OldInvoices invoice)
        {
            Contract.Requires(invoice != null);

            Model.InvoiceShared invoiceShared = invoice.Balanced ? 
                new Model.InvoiceSharedBalanced(invoice.Id, invoice.StartDate, invoice.EndDate) :
                new Model.InvoiceShared(invoice.Id, invoice.StartDate, invoice.EndDate);

            invoiceShared.SetReadOut(invoice.Estimation, invoice.Consumption);
            invoiceShared.SetBasicFee(invoice.BasicFee, 0);
            invoiceShared.SetUsageFee(invoice.WaterFee, invoice.WasteFee);

            return invoiceShared;
        }

        private static Model.FeeConfig ToFeeConfig(Model.Wendy1.OldConfigFee configFee)
        {
            Contract.Requires(configFee != null);

            var config = new Model.FeeConfig
            {
                End = configFee.EndSpecified ? configFee.End : (DateTime?)null,
                Start = configFee.BeginSpecified ? configFee.Begin : new DateTime(),
                MonthlyBasicFee = new Model.BasicFeeConfig(0, 0, 0),
                MonthlyUsageFee = new Model.WaterFee(configFee.WaterFee, configFee.WasteFee),
                VAT = configFee.VAT,
                VATIncludedIntoMonthlyFees = false
            };

            return config;
        }

        private static void ConvertUserNamesToUserMeterConfigHistory(List<Model.Wendy1.OldUserNames> userNames, List<Model.UserMeterConfigHistory> userMeterConfigHistory)
        {
            Contract.Requires(userNames != null);
            Contract.Requires(userMeterConfigHistory != null);

            foreach (var username in userNames)
            {
                var userMeterConfig = new Model.UserMeterConfigHistory()
                {
                    MeterUser = username.Name
                };
                userMeterConfig.MeterConfigs.Add(ToMeterConfig(username));

                userMeterConfigHistory.Add(userMeterConfig);
            }
        }

        private static Model.MeterConfig ToMeterConfig(Model.Wendy1.OldUserNames username)
        {
            Contract.Requires(username != null);

            var mererConfig = new Model.MeterConfig
            {
                Start = new DateTime(),
                StartReadOut = username.StartConsumption
            };

            return mererConfig;
        }

        private static Model.MeterConfig ToMeterConfig(Model.Wendy1.OldConfig config)
        {
            if (config == null) return null;

            var mererConfig = new Model.MeterConfig
            {
                Start = new DateTime(),
                StartReadOut = config.StartConsumption
            };

            return mererConfig;
        }
    }
}
