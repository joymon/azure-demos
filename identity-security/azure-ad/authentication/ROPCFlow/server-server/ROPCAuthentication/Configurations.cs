using System;
using System.Configuration;
using System.Security;

namespace ROPCAuthentication
{
    class Configurations
    {
        public static string AADTenantId => ConfigurationManager.AppSettings["AADTenantId"];
        public static string SiteId => ConfigurationManager.AppSettings["SiteId"];
        public static string LibraryId => ConfigurationManager.AppSettings["LibraryId"];
        public static string FileId => ConfigurationManager.AppSettings["FileId"];
        public static string AADAppregistrationId => ConfigurationManager.AppSettings["AADAppregistrationId"];
        public static string ServiceAccountName => ConfigurationManager.AppSettings["serviceAccountName"];
        public static string RootSiteUrl => ConfigurationManager.AppSettings["RootSiteURL"];
        public static string AbsoluteSPSFilePath => ConfigurationManager.AppSettings["AbsoluteSPSFilePath"];
        public static Guid SPSiteId => GetSPSiteId();

        private static Guid GetSPSiteId()
        {
            return new Guid(SiteId.Split(',')[1]);
        }
        public static Guid SPWebId => GetSPWebId();

        private static Guid GetSPWebId()
        {
            return new Guid(SiteId.Split(',')[2]);
        }

        public static SecureString ServiceAccountSecurePassword => getSecurePassword();

        private static SecureString getSecurePassword()
        {
            string serviceAccountPasswordClearText = ConfigurationManager.AppSettings["serviceAccountPasswordClearText"];
            return CommonUtils.GetSecureString(serviceAccountPasswordClearText);
        }
        #region To work with PnPFrameworkLibrary
        public static string SubSiteURL => ConfigurationManager.AppSettings["SubSiteURL"];
        public static string HumanReadableAbosolutePathToFile => ConfigurationManager.AppSettings["HumanReadableAbosolutePathToFile"];
        #endregion
    }
}
