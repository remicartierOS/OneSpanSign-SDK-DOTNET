﻿using System;
using System.Collections.Generic;

namespace Silanis.ESL.SDK
{
    /// <summary>
    /// Conversion from API to SDK Usage Report.
    /// </summary>
    internal class UsageReportConverter
    {
        private Silanis.ESL.SDK.UsageReport sdkUsageReport = null;
        private Silanis.ESL.API.UsageReport apiUsageReport = null;

        internal UsageReportConverter(Silanis.ESL.API.UsageReport apiUsageReport)
        {
            this.apiUsageReport = apiUsageReport;
        }

        /// <summary>
        /// Convert from API UsageReport to SDK UsageReport.
        /// </summary>
        /// <returns>The SDK usage report.</returns>
        public Silanis.ESL.SDK.UsageReport ToSDKUsageReport()
        {
            if (apiUsageReport == null)
            {
                return sdkUsageReport;
            }

            IList<Silanis.ESL.API.SenderUsageReport> senderUsageReportList = apiUsageReport.Senders;

            if (senderUsageReportList.Count != 0)
            {
                Silanis.ESL.SDK.UsageReport result = new Silanis.ESL.SDK.UsageReport();
                result.From = apiUsageReport.From;
                result.To = apiUsageReport.To;

                Silanis.ESL.SDK.SenderUsageReport sdkSenderUsageReport;
                foreach (Silanis.ESL.API.SenderUsageReport apiSenderUsageReport in senderUsageReportList)
                {
                    sdkSenderUsageReport = ToSDKSenderUsageReport(apiSenderUsageReport);
                    result.AddSenderUsageReport(sdkSenderUsageReport);
                }

                return result;
            }

            return sdkUsageReport;
        }

        // Convert from API to SDK SenderUsageReport.
        private Silanis.ESL.SDK.SenderUsageReport ToSDKSenderUsageReport(Silanis.ESL.API.SenderUsageReport apiSenderUsageReport)
        {
            Silanis.ESL.SDK.SenderUsageReport sdkSenderUsageReport = new Silanis.ESL.SDK.SenderUsageReport();
            sdkSenderUsageReport.Sender = new SenderConverter(apiSenderUsageReport.Sender).ToSDKSender();

            IDictionary<UsageReportCategory, int> categoryCount = new Dictionary<UsageReportCategory, int>();
            foreach (KeyValuePair<string, object> entry in apiSenderUsageReport.Packages)
            { 

                UsageReportCategory usageReportCategory = UsageReportCategory.valueOf(entry.Key.ToUpper());
                categoryCount.Add(usageReportCategory, Convert.ToInt32(entry.Value));
            }
            sdkSenderUsageReport.CountByUsageReportCategory = categoryCount;

            return sdkSenderUsageReport;
        }

    }
}

