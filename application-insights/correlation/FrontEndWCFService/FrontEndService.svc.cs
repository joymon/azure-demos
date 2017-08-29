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

namespace FrontEndWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class FrontEndService : IFrontEndService
    {
        public string GetAreaOfCircle(string value)
        {
            TelemetryClient client = new TelemetryClient(TelemetryConfiguration.Active);
            IOperationHolder<DependencyTelemetry> holder = client.StartOperation<DependencyTelemetry>("Custom operation from FrontEndWCFService");
            holder.Telemetry.Type = "Custom";
            double radius = Convert.ToDouble(value);
            InternalServiceReference.InternalServiceClient serviceClient = new InternalServiceReference.InternalServiceClient();
            double pi = serviceClient.GetValueOfPi();
            client.StopOperation<DependencyTelemetry>(holder);
            return string.Format("Area of circle with {0} raidus is {1}", value, pi * radius * radius);
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
