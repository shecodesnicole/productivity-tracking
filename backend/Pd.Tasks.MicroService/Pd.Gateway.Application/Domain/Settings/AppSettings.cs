using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Domain.Settings
{
    public class AppSettings
    {
        public const string SectionName = nameof(AppSettings);
        public const string DbConnectionString = "DB_CONNECTION_STRING";

        public const string ServiceAccountName = "SERVICE_ACCOUNT_NAME";
        public string SecretKey { get; set; }
    }
}
