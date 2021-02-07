namespace ROPCAuthentication
{
    class AuthenticationManagerFactory
    {
        internal static IAuthenticationManager Get()
        {
            return new MSALBasedAuthenticationManager();
        }
    }
}
