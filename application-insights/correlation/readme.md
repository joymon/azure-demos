Details are available at

[Azure Application Insights - End to end correlation from Angular to WCF Services](http://joymonscode.blogspot.com/2017/08/azure-application-insights-end-to-end.html)

# How to run
- Compiling
  - Make sure the URL [https://www.myget.org/F/applicationinsights-sdk-labs/api/v3/index.json](https://www.myget.org/F/applicationinsights-sdk-labs/api/v3/index.json) is presend in package sources. This is needed for the Microsoft.ApplicationInsights.Wcf nuget
- Hosting
  - Make sure the InternalService hosted with net.pipe binding enabled
  - Replace {YOURKEY} with actual application insight instrumentation key
  - Replace this.baseURL variable in service.js has the base path. It has to end with /


