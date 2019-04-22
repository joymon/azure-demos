using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RoleNameTelemetryInitializer : ITelemetryInitializer
    {
        string _roleName;
        public RoleNameTelemetryInitializer(string roleName)
        {
            if (string.IsNullOrWhiteSpace(_roleName))
            {
                Console.WriteLine($"{nameof(roleName)} is empty. So cloud_RoleName may not be set.");
            }
            _roleName = roleName;
        }
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrWhiteSpace(telemetry.Context.Cloud.RoleName))
            {
                    //set custom role name here
                    telemetry.Context.Cloud.RoleName = this._roleName;
            }
        }
    }
}
