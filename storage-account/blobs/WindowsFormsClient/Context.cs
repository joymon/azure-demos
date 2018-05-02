using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    static class Context
    {
        internal static string AzCopyPath = @"C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy";
        internal static readonly string AccountKey = "<Key>";
        internal static readonly string StorageAccountFullName = "<Name of storage account>";
        internal static readonly string FolderToDownloadFrom = "<Destination folder>";
        static Context()
        {
            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["StorageAccountFullName"]))
            { }
            else
            {
                StorageAccountFullName = ConfigurationManager.AppSettings["StorageAccountFullName"];
            }
            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AccountKey"]))
            { }
            else
            {
                AccountKey = ConfigurationManager.AppSettings["AccountKey"];
            }
        }
    }
}
