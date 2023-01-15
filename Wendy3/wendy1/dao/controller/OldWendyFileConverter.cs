using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Wendy.Fee.Entity;
using Wendy.Invoice.Entity;
using Wendy.Invoice.Service;
using Wendy.Meter.Entity;
using Wendy.Wendy1.Entity;

namespace Wendy.Wendy1.Dao.Controller
{
    /// <summary>
    /// Old wendy file converter
    /// </summary>
    public static class OldWendyFileConverter
    {
        /// <summary>
        /// Convert old wendy file to invoice history model
        /// </summary>
        /// <param name="oldWendyData"></param>
        /// <returns></returns>
        public static InvoiceHistory ToInvoiceHistory(OldWendyFile oldWendyData)
        {
            Contract.Requires(oldWendyData != null);

            var invoiceHistory = new InvoiceHistory();

            var mainMeterConfig = ToMeterConfig(oldWendyData.Config) ?? MeterConfig.CreateEmpty();
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

            foreach (var user in oldWendyData.Users)
            {
                InvoiceShared sharedInvoice = invoiceHistory.Invoices.GetInvoiceById(user.InvoiceId);
                sharedInvoice?.UserInvoices.Add(ToUserInvoice(sharedInvoice.GetReadOutDate(), user));
            }

            return invoiceHistory;
        }

        private static UserInvoice ToUserInvoice(DateTime readOutDate, OldUsers user)
        {
            Contract.Requires(user != null);

            var userInvoice = new UserInvoice(user.User, readOutDate, user.Consumption, user.Consumption);
            userInvoice.GetBasicFee().CleanWaterFee = user.BasicFee;
            userInvoice.GetUsageFee().CleanWaterFee = user.WaterFee;
            userInvoice.GetUsageFee().WasteWaterFee = user.WasteFee;

            return userInvoice;
        }

        private static InvoiceShared ToInvoiceShared(OldInvoices invoice)
        {
            Contract.Requires(invoice != null);

            InvoiceShared invoiceShared = new InvoiceShared(invoice.Id, invoice.StartDate, invoice.EndDate)
            {
                Balanced = invoice.Balanced
            };

            invoiceShared.GetReadOut().Estimated = invoice.Consumption;
            invoiceShared.GetReadOut().Real = invoice.Consumption;
            invoiceShared.GetConsumption().Estimated = invoice.Estimation;
            invoiceShared.GetBasicFee().CleanWaterFee = invoice.BasicFee;
            invoiceShared.GetUsageFee().CleanWaterFee = invoice.WaterFee;
            invoiceShared.GetUsageFee().WasteWaterFee = invoice.WasteFee;

            return invoiceShared;
        }

        private static FeeConfig ToFeeConfig(OldConfigFee configFee)
        {
            Contract.Requires(configFee != null);

            var config = new FeeConfig
            {
                End = configFee.EndSpecified ? configFee.End : (DateTime?)null,
                Start = configFee.BeginSpecified ? configFee.Begin : new DateTime(),
                MonthlyBasicFee = new BasicFeeConfig(0, 0, 0),
                MonthlyUsageFee = new WaterFee(configFee.WaterFee, configFee.WasteFee),
                VAT = configFee.VAT,
                VATIncludedIntoMonthlyFees = false
            };

            return config;
        }

        private static void ConvertUserNamesToUserMeterConfigHistory(List<OldUserNames> userNames, List<UserMeterConfigHistory> userMeterConfigHistory)
        {
            Contract.Requires(userNames != null);
            Contract.Requires(userMeterConfigHistory != null);

            foreach (var username in userNames)
            {
                var userMeterConfig = new UserMeterConfigHistory()
                {
                    MeterUser = username.Name
                };
                userMeterConfig.MeterConfigs.Add(ToMeterConfig(username));

                userMeterConfigHistory.Add(userMeterConfig);
            }
        }

        private static MeterConfig ToMeterConfig(OldUserNames username)
        {
            Contract.Requires(username != null);

            var mererConfig = new MeterConfig
            {
                Start = new DateTime(),
                StartReadOut = username.StartConsumption
            };

            return mererConfig;
        }

        private static MeterConfig ToMeterConfig(OldConfig config)
        {
            if (config == null) return null;

            var mererConfig = new MeterConfig
            {
                Start = new DateTime(),
                StartReadOut = config.StartConsumption
            };

            return mererConfig;
        }
    }
}
