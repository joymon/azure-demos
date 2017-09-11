using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AngularClient.Controllers
{
    public class MVCWCFClientController : Controller
    {
        // GET: MVCWCFClient
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EchoHeaders()
        {
            return new JsonResult()
            {
                Data = Request.Headers.AllKeys.Select((key) => $"{key}"),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}