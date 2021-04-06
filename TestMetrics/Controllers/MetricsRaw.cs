using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestMetrics.Metrics;

namespace TestMetrics.Controllers
{
    public class MetricsRaw : Controller
    {
        private readonly MetricsHelper _metricsHelper;

        public MetricsRaw(MetricsHelper metricsHelper)
        {
            _metricsHelper = metricsHelper;
        }

        [Route("metrics/report/{type}")]
        [HttpGet]
        public Task<string> Report(string type)
        {
            return _metricsHelper.ReportAsync(type);
        }
    }
}
