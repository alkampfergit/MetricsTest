using App.Metrics;
using App.Metrics.Counter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMetrics.Metrics
{
    public class MetricsHelper
    {
        private readonly IMetricsRoot _metrics;

        public MetricsHelper()
        {
            _metrics = new MetricsBuilder() //AppMetrics.CreateDefaultBuilder()
               .Configuration
               .Configure(
                   options =>
                   {
                       options.DefaultContextLabel = "MyContext";
                       options.GlobalTags.Add("myTagKey", "myTagValue");
                       options.Enabled = true;
                       options.ReportingEnabled = true;
                   })
               .OutputMetrics.AsJson()
               .OutputMetrics.AsPlainText()
               .OutputMetrics.AsPrometheusPlainText()
               .Build();

            CreateCounters();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMetrics(_metrics);
            services.AddMetricsEndpoints();
        }

        private readonly Dictionary<String, CounterOptions> _counters = new();

        private void CreateCounters()
        {
            _counters["forecast"] = new CounterOptions()
            {
                Name = "forecast",
                MeasurementUnit = Unit.Calls,
            };
        }

        public void IncrementCounter(string key)
        {
            _metrics.Measure.Counter.Increment(_counters[key]);
        }

        public async Task<String> ReportAsync(string type)
        {
            var snapshot = _metrics.Snapshot.Get();
            var outputMetrics = _metrics.OutputMetricsFormatters.FirstOrDefault(r => r.MediaType.Format.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (outputMetrics != null)
            {
                using var stream = new MemoryStream();
                await outputMetrics.WriteAsync(stream, snapshot).ConfigureAwait(false);
                return Encoding.UTF8.GetString(stream.ToArray());
            }

            return "";
        }
    }
}
