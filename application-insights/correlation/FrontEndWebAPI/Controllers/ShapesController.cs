using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FrontEndWebAPI.Controllers
{
    [RoutePrefix("api/shapes")]
    public class ShapesController : ApiController
    {

        [Route("circle/{radius}")]
        async public Task<double> GetRadius(int radius)
        {
            TelemetryClient client = TelemetryFactory.GetTelemetryClient();

            IOperationHolder<DependencyTelemetry> holder = client.StartOperation<DependencyTelemetry>("Custom operation from FrontEndWebAPI");
            client.TrackEvent("Custom event from FrontEndWebAPI");
            double pi = await new PiValueProvider().Get();
            client.StopOperation<DependencyTelemetry>(holder);
            return radius * radius * pi;
        }
    }
    internal class PiValueProvider
    {
        async internal Task<double> Get()
        {
            WebClient client = new WebClient();
            string pi =await client.DownloadStringTaskAsync("https://api.pi.delivery/v1/pi?start=0&numberOfDigits=5");
            // value of pi will come as JSON like {"content":"3.141"}. Using RegEx to extract value
            pi = Regex.Match(pi, @"[\d.]+").Value;
            return Convert.ToDouble(pi);
        }
    }
}