using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shared
{
    class ApplicationInsightsContextInspector : IDispatchMessageInspector
    {
        object IDispatchMessageInspector.AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // WORKAROUND! TEST CAREFULLY!
            // see https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/941
            
            // It will get value if there is Http interceptor modules.
            var formerActivity = (Activity)HttpContext.Current?.Items["__AspnetActivity__"];

            if (formerActivity != null)
            {
                var newActvity = new Activity(formerActivity.OperationName);
                newActvity.SetStartTime(formerActivity.StartTimeUtc);
                newActvity.SetParentId(formerActivity.ParentId ?? formerActivity.RootId);
                foreach (var baggage in formerActivity.Baggage)
                {
                    newActvity.AddBaggage(baggage.Key, baggage.Value);
                }

                foreach (var tag in formerActivity.Tags)
                {
                    newActvity.AddTag(tag.Key, tag.Value);
                }

                newActvity.Start();

                var requestTelemetry = (RequestTelemetry)HttpContext.Current?.Items["Microsoft.ApplicationInsights.RequestTelemetry"];
                if (requestTelemetry != null)
                {
                    requestTelemetry.Context.Operation.Id = newActvity.RootId;
                    requestTelemetry.Context.Operation.ParentId = newActvity.ParentId;
                    requestTelemetry.Id = newActvity.Id;
                }
            }
            return null;
        }
        
        void IDispatchMessageInspector.BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }
}
