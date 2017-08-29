using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InternalService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class InternalService : IInternalService
    {
        public string GetData(int value)
        {
            
            return string.Format("You entered: {0}", value);
        }

        public double GetValueOfPi()
        {
            TelemetryClient client = new TelemetryClient(TelemetryConfiguration.Active);
            IOperationHolder<DependencyTelemetry> holder = client.StartOperation<DependencyTelemetry>("Custom operation from Internal service");
            client.TrackEvent("Custom event from Internal service");
            client.StopOperation<DependencyTelemetry>(holder);
            return 3.14d;
        }
    }
}
