namespace ROPCAuthentication
{
    public class SharePointManagerFactory
    {
        public static ISharePointManager Get(SharePointInteractionType type)
        {
            if (type == SharePointInteractionType.GraphAPI)
            {
                return new GraphAPIBasedSharePointManager();
            }
            else
            {
                return new PnPFrameworkBasedSharePointManager();
            }
        }
        
    }
}