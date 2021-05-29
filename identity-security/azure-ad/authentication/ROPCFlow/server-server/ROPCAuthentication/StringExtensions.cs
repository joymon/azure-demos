using System.Security;

namespace ROPCAuthentication
{
    static public class StringExtensions
    {
        internal static SecureString ToSecureString(this string pw)
        {
            SecureString securePassword = new SecureString();

            foreach (char c in pw)
                securePassword.AppendChar(c);
            securePassword.MakeReadOnly();

            return securePassword;
        }
    }
}
