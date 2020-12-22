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
    public class InternalService : IInternalService
    {
         static InternalService()
        {
            TelemetryAdder.AddToActive();
        }
        public string GetData(int value)
        {
            
            return string.Format("You entered: {0}", value);
        }

        public double GetValueOfPi()
        {
            TelemetryClient client = new TelemetryClient(TelemetryConfiguration.Active);
            //IOperationHolder<DependencyTelemetry> holder = client.StartOperation<DependencyTelemetry>("Custom operation from Internal service");
            client.TrackTrace("Custom trace from Internal service");
            //client.StopOperation<DependencyTelemetry>(holder);
            return 3.14d;
        }
    }
}
