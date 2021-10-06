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

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions
{
    public static class ResetUserCredentialsFunctions
    {
        [FunctionName("ResetUserCredentialsFunctions")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Load the encryption module
            var encryptionService = new EncryptionService();
            //Get attendee service for table storage communication
            var attendeeService = new AttendeeService();
            //GetEmailService
            var emailService = new EmailService();

           

            var InputMessage = req.Query;
            //set userId to parse
            if (!InputMessage.ContainsKey("userId"))
            {            
                return new BadRequestObjectResult("Please provide your userId");
            }

            //Set mail to parse
            if (!InputMessage.ContainsKey("eMail"))
            {
                return new BadRequestObjectResult("Please provide your eMail");
            }

            //UserId and eMail is present 

            //Get Record based on id from Table 
            var attendeeRecord = attendeeService.GetAttendeeRecord(InputMessage["userId"]);
            if (attendeeRecord == null)
            {
                return new NotFoundObjectResult("We could not find a registration based on the userId and eMail combination");
            }
            attendeeRecord = encryptionService.DecryptAttendeeRecord(attendeeRecord);

            //Check if eMail adress is a match with record 
            if (attendeeRecord.Email != InputMessage["eMail"])
            {
                return new NotFoundObjectResult("We could not find a registration based on the userId and eMail combination");
            }

            //User proved that he can access his record
            await emailService.SendRegistrationSucceededMail(attendeeRecord.Email, attendeeRecord.Name + " " + attendeeRecord.Surname, attendeeRecord.Username, attendeeRecord.Password);

            //Return Ok
            return new OkResult();
        }
    }
}
