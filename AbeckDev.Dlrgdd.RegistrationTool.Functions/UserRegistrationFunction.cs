using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions
{
    public static class UserRegistrationFunction
    {
        [FunctionName("UserRegistrationFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Get encryption service
            var encryptionService = new EncryptionService();

            //Validate User Input

            //Get Dictionary out of input
            Dictionary<string, string> InputMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(new StreamReader(req.Body).ReadToEnd());

            if (!InputMessage.ContainsKey("name") ||
                !InputMessage.ContainsKey("surname") ||
                !InputMessage.ContainsKey("eMail") ||
                !InputMessage.ContainsKey("Birthday") ||
                !InputMessage.ContainsKey("Adress"))
            {
                return new BadRequestObjectResult("Not all needed parameters are set!");
            }




            return new OkObjectResult("");
        }
    }
}

