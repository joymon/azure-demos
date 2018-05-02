using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient
{
    public partial class Form1 : Form
    {
        TextBoxTraceListener _textBoxListener;
        public Form1()
        {
            InitializeComponent();
        }

        private async void LogCorrelatedEventsFromChainedTasksbutton_Click(object sender, EventArgs e)
        {
            await FindAreaWithCorrelatedAppInsight();
            DoWorkWithCorrelatedAppinsightUsing();
        }

        private async Task FindAreaWithCorrelatedAppInsight()
        {
            var logger = new AppInsightLogger();
            string opId = Guid.NewGuid().ToString();
            IOperationHolder<RequestTelemetry> opHolder = new AppInsightLogger().StartOperation("Find Area normal", opId);
            
            Log("DoWorkWithCorrelatedAppInsight - Entered", opId);
            double raidus = await GetRadiusFromUI();
            Log("DoWorkWithCorrelatedAppInsight Converted Radius is - ", opId);
            double area = await GetAreaTask(raidus, opId, opHolder.Telemetry.Id);
            Log($"DoWorkWithCorrelatedAppInsight Area is - {area}", opId);
            new AppInsightLogger().StopOperation(opHolder);
        }

        private static void DoWorkWithCorrelatedAppinsightUsing()
        {
            string opId = Guid.NewGuid().ToString();

            //using (IOperationHolder<RequestTelemetry> op = new AppInsightLogger().StartOperation("Find Area using", opId))
            {

            }
        }

        private async Task<double> GetArea(double radius)
        {
            double area = radius * radius * 3.14;
            int delay = new Random().Next() % 8000;
            await Task.Delay(delay);
            Log($"Inside GetArea - Aread - {area}, Delay-{delay}");
            return area;
        }
        /// <summary>
        /// Trying this as separate child operation
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Task<double> GetAreaTask(double radius, string opId, string parentOpId)
        {
            return Task.Run<double>(() =>
            {
                var appInsight = new AppInsightLogger();
                IOperationHolder<DependencyTelemetry> dep = appInsight.StartOperation<DependencyTelemetry>("GetAreaTask", opId, parentOpId);
                double area = radius * radius * 3.14;
                int delay = new Random().Next() % 8000;
                Log($"Inside GetAreaTask - Going to Delay-{delay}");
                Task.Delay(delay).Wait();
                Log($"Inside GetAreaTask - Area - {area}");
                new AppInsightLogger().StopOperation(dep);
                return area;
            });
        }
        private async Task<double> GetRadiusFromUI()
        {
            string radiusString = RadiustextBox.Text;
            double radius = Convert.ToDouble(radiusString);
            int delay = new Random().Next() % 8000;
            await Task.Delay(delay);
            Log($"Inside GetRadiusFromUI - Radius - {radius},Delay-{delay}");
            return radius;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _textBoxListener = new TextBoxTraceListener(LogstextBox);
            Trace.Listeners.Add(_textBoxListener);
        }
        void Log(string message, string opId = null)
        {
            string msgToLog = $"Thread Id-{Thread.CurrentThread.ManagedThreadId},OpId - {opId}, Message - {message}";
            Trace.WriteLine(msgToLog);
            new AppInsightLogger().TrackEvent(msgToLog);
        }

        #region WebService
        private async void FindAreaViaWebServiceButton_Click(object sender, EventArgs e)
        {
            string opId = Guid.NewGuid().ToString();
            var opHolder = new AppInsightLogger().StartOperation<RequestTelemetry>("Find Area Via WebService", opId);
            
            Log("FindAreaViaWebServiceButton_Click - Entered", opId);

            double area = await GetAreaFromReSTAPI(await GetRadiusFromUI(),opId);

            Log($"FindAreaViaWebServiceButton_Click - Area is - {area}", opId);
            new AppInsightLogger().StopOperation(opHolder);
        }

        private Task<double> GetAreaFromReSTAPI(double radius, string operation_id)
        {
            return Task.Run<double>(() =>
            {
                var url = $"http://localhost/FrontEndWCFService/FrontEndService.svc/areaOf/{radius}";
                var wc = new WebClient();
                wc.Headers.Add("Request-Id", operation_id);
                string value = wc.DownloadString(url);
                value = Regex.Matches(value, @"\d+").OfType<Match>().Select(m=>m.Value).LastOrDefault();
                Log($"GetAreaFromReSTAPI - Value from web service - {value}");
                return Convert.ToDouble(value);
            });
        }
    }
    #endregion
}
