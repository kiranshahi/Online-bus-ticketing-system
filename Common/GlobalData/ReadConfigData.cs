using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Common.GlobalData
{
    public static class ReadConfigData
    {
        public static string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["BusTicketingContext"].ConnectionString;           
        }
        public static string GetAppSettingsValue(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}
