using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace DesktopClient
{
    class AppInsightLogger

    {
        
        internal IOperationHolder<RequestTelemetry> StartOperation(string name, string opID)
        {
            return GetTelemetryClient().StartOperation<RequestTelemetry>(name, opID);
        }
        internal IOperationHolder<TelemetryType> StartOperation<TelemetryType>(string name, string opID,string parentId=null) where TelemetryType:OperationTelemetry,new()
        {
            return GetTelemetryClient().StartOperation<TelemetryType>(name, opID,parentId);
        }
        internal IOperationHolder<RequestTelemetry> StartOperation(string name)
        {
            return GetTelemetryClient().StartOperation<RequestTelemetry>(name);
        }
        internal void StopOperation<TelemetryType>(IOperationHolder<TelemetryType> opHolder) where TelemetryType : OperationTelemetry, new()
        {
             GetTelemetryClient().StopOperation<TelemetryType>(opHolder);
        }
        internal void CustomLog(string message, string OpId)
        {
            TelemetryClient client = GetTelemetryClient();
            if (string.IsNullOrEmpty(OpId))
            {
            }
            else
            {
                client.Context.Operation.Id = OpId;
            }
            IOperationHolder<DependencyTelemetry> holder = client.StartOperation<DependencyTelemetry>("Custom operation from DesktopClient");
            holder.Telemetry.Type = "Custom";
            client.StopOperation<DependencyTelemetry>(holder);
        }

        internal void TrackEvent(string message)
        {
            GetTelemetryClient().TrackEvent(message);
        }
        internal static TelemetryClient GetTelemetryClient()
        {
            return new TelemetryClient(TelemetryConfiguration.Active);
        }
    }
}
