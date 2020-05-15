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
        public string RoleName { get; set; }
        public RoleNameTelemetryInitializer()
        {

        }
        public RoleNameTelemetryInitializer(string roleName)
        {
            if ( string.IsNullOrWhiteSpace(this.RoleName) && string.IsNullOrWhiteSpace(roleName))
            {
                Console.WriteLine($"{nameof(roleName)} is empty. So cloud_RoleName may not be set.");
            }
            this.RoleName = roleName;
        }
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrWhiteSpace(telemetry.Context.Cloud.RoleName))
            {
                    //set custom role name here
                    telemetry.Context.Cloud.RoleName = this.RoleName;
            }
        }
    }
}
