using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace FrontEndWebAPI.Controllers
{
    class TelemetryFactory
    {
        internal static TelemetryClient GetTelemetryClient()
        {
            return new TelemetryClient(TelemetryConfiguration.Active);
        }
    }
}
