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
    public static class GetMyEventInformationFunction
    {
        [FunctionName("GetMyEventInformationFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Get encryption service
            var encryptionService = new EncryptionService();
            //Get attendee service
            var attendeeService = new AttendeeService();

            //Build request mode
            string userId;
            string eMail;

            var InputMessage = req.Query;
            //set userId to parse
            if (InputMessage.ContainsKey("userId"))
            {
                userId = InputMessage["userId"];
            }
            else
            {
                return new BadRequestObjectResult("Please provide your userId");
            }

            //Set export format
            if (InputMessage.ContainsKey("eMail"))
            {
                eMail = InputMessage["eMail"];
            }
            else
            {
                return new BadRequestObjectResult("Please provide your eMail");
            }

            //UserId and eMail is present 

            //Get Record based on id from Table 
            var attendeeRecord = attendeeService.GetAttendeeRecord(userId);
            if (attendeeRecord == null)
            {
                return new NotFoundObjectResult("We could not find a registration based on the userId and eMail combination");
            }
            attendeeRecord = encryptionService.DecryptAttendeeRecord(attendeeRecord);

            //Check if eMail adress is a match with record 
            if (attendeeRecord.Email != eMail)
            {
                return new NotFoundObjectResult("We could not find a registration based on the userId and eMail combination");
            }

            //User proved that he can access his record 
            return new OkObjectResult(new MyEventInformation(attendeeRecord));

        }
    }
}
