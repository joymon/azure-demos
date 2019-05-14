using System.Configuration;
using System.Web.Http.Filters;

namespace FrontEndWebAPI
{
    public class AddCustomHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add("target", ConfigurationManager.AppSettings["ai.roleName"]);
        }
    }
}