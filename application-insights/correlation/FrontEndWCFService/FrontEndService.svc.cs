using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class FrontEndService : IFrontEndService
    {
        public string GetAreaOfCircle(string value)
        {
            TelemetryClient client = GetTelemetryClient();
            IOperationHolder<DependencyTelemetry> holder = client.StartOperation<DependencyTelemetry>("Custom operation from FrontEndWCFService");
            holder.Telemetry.Type = "Custom";
            double radius = Convert.ToDouble(value);
            double pi = GetValueOfPi().Result;
            client.StopOperation<DependencyTelemetry>(holder);
            return string.Format("Area of circle with {0} raidus is {1}", value, pi * radius * radius);
        }

        private static TelemetryClient GetTelemetryClient()
        {
            return new TelemetryClient(TelemetryConfiguration.Active);
        }
        static readonly Random random = new Random();
        private static async Task<double> GetValueOfPi()
        {
            InternalServiceReference.InternalServiceClient serviceClient = new InternalServiceReference.InternalServiceClient();
            double pi;
            bool shouldGetPiFromService = random.Next() % 2 == 0;
            shouldGetPiFromService = false; // Hack if the netPipeService is not working.
            if (shouldGetPiFromService) {
                pi = await serviceClient.GetValueOfPiAsync();
                //This public URL will also give the valyue. https://api.pi.delivery/v1/pi?start=0&numberOfDigits=10
            }
            else
            {
                pi = 3.14;
            }
            
            await Task.Delay(1000);
            GetTelemetryClient().TrackEvent($"FrontEndService.GetValueOfPi() = {pi}");
            
            return pi;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
