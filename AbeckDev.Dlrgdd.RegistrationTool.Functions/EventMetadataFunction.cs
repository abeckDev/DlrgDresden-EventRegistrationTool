using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Services;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Models;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions
{
    public static class EventMetadataFunction
    {
        [FunctionName("EventMetadataFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            //Get MetaInformationService
            var metaInformationService = new MetaInformationService();


            return new OkObjectResult(new MetaInformation()
            {
                RegistrationDeadline = metaInformationService.GetEventRegistrationDeadline(),
                IsDeadlineReached = metaInformationService.IsRegistrationDeadlineReached(),
                IsRegistrationStartReached = metaInformationService.IsRegistrationStartReached(),
                IsRegistrationPossible = metaInformationService.IsRegistrationPossible()
            });
        }
    }
}
