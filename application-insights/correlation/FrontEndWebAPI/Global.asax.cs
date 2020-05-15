﻿using Microsoft.ApplicationInsights.Extensibility;
using Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace FrontEndWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //string roleName = ConfigurationManager.AppSettings["ai.roleName"];
            //TelemetryConfiguration.Active.TelemetryInitializers.Add(new RoleNameTelemetryInitializer(roleName));
        }
    }
}
