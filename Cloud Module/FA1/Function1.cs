/******************************************************************************
* Filename    = Function1.cs
*
* Author      = Arnav Rajesh Kadu
*
* Product     = Cloud
* 
* Project     = Unnamed Software Project
*
* Description = -
*****************************************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FA1
{
    /// <summary>
    /// 
    /// </summary>
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
