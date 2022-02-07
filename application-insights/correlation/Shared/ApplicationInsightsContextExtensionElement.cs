using System;
using System.ServiceModel.Configuration;

namespace Shared
{
    class ApplicationInsightsContextExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(ApplicationInsightsContextBehavior);

        protected override object CreateBehavior()
        {
           return new ApplicationInsightsContextBehavior();
        }
    }
}
