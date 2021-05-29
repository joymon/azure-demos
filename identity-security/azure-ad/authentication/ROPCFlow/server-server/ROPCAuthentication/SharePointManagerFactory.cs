using System;

namespace ROPCAuthentication
{
    public class SharePointManagerFactory
    {
        public static ISharePointManager Get(SharePointInteractionType type)
        {
            switch (type)
            {
                case SharePointInteractionType.GraphAPI:
                    return new GraphAPIBasedSharePointManager();
                case SharePointInteractionType.PnPFramework:
                    return new PnPFrameworkBasedSharePointManager();
                default:
                    throw new ArgumentException($"Argument value {type} is invalid for the Enum {nameof(SharePointInteractionType)}");
            }
        }
    }
}