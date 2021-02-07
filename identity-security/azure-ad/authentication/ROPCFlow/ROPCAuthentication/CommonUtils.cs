using System.Security;

namespace ROPCAuthentication
{
    public class CommonUtils
    {
        internal static SecureString GetSecureString(string pw)
        {
            SecureString securePassword = new SecureString();

            foreach (char c in pw)
                securePassword.AppendChar(c);
            securePassword.MakeReadOnly();

            return securePassword;
        }
    }
}
