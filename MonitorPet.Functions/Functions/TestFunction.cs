using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MonitorPet.Functions.Settings;
using MonitorPet.Functions.Security;

namespace MonitorPet.Functions.Functions
{
    public static class TestFunction
    {
        private static TokenAccess TokenServer { get; } =
            new TokenAccess(AppSettings.TryGetSettings(AppSettings.DEFAULT_QUERY_ACCESS_TOKEN));

        [FunctionName("TestFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (!TokenServer.IsValidAccessToken(req.Query[AppSettings.DEFAULT_QUERY_ACCESS_TOKEN]))
                return new UnauthorizedResult();

            return new OkObjectResult(
                AppSettings.TryGetSettings(AppSettings.DEFAULT_MYSQL_CONFIG).ToCharArray().Take(5)
            );
        }
    }
}