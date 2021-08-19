using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Models;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
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
            //Get MetadataInformation
            var metaInformationService = new MetaInformationService();
            //Get attendee service
            var attendeeService = new AttendeeService();


            //Check if Deadline is already reached
            if (metaInformationService.IsRegistrationDeadlineReached())
            {
                return new BadRequestObjectResult("Registration not possible. Event Deadline Reached!");
            }


            //Validate User Input
            //Get Dictionary out of input
            Dictionary<string, string> InputMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(new StreamReader(req.Body).ReadToEnd());

            if (!InputMessage.ContainsKey("name") ||
                !InputMessage.ContainsKey("surname") ||
                !InputMessage.ContainsKey("eMail") ||
                !InputMessage.ContainsKey("birthday") ||
                !InputMessage.ContainsKey("address"))
            {
                return new BadRequestObjectResult("Not all needed parameters are set!");
            }

            //Check if Birthday is valid input 
            if (!DateTime.TryParse(InputMessage["birthday"], out DateTime UserBirthday))
            {
                return new BadRequestObjectResult("Could not parse Birthday!");
            }


            //All needed parameters are present, lets create an object for that to work with
            var registrationRequest = new UserRegistrationRequest()
            {
                Name = InputMessage["name"],
                Surname = InputMessage["surname"],
                EmailAddress = InputMessage["eMail"],
                Birthday = UserBirthday.ToString(),
                Address = InputMessage["address"]
            };

            //Validate if user is Eligable for attendance based on Validation Module
            Dictionary<string, string> validationParameters = new Dictionary<string, string>()
            {
                {"vorname", registrationRequest.Name },
                {"nachname", registrationRequest.Surname },
                {"strasse", registrationRequest.Address },
                {"geburtsdat", $"{UserBirthday.Day.ToString("00")}/{UserBirthday.Month.ToString("00")}/{UserBirthday.Year.ToString()}" }
            };
            if (!attendeeService.IsValidMember(validationParameters, "AND"))
            {
                return new BadRequestObjectResult("User not found in member database!");
            }
            //If yes, extract userId from CSV and write it to the request
            string memberId = attendeeService.GetMemberIdFromMemberTable(validationParameters, "AND");
            registrationRequest.UserId = memberId;


            //ToDo: Check if account exists already (based on mail and attendeeId)

            //Encrypt user information for further processing
            registrationRequest = encryptionService.EncryptRegistrationRequest(registrationRequest);

            //Create Queue Item for further processing
            var queueClient = new QueueClient(System.Environment.GetEnvironmentVariable("ServiceBusConnectionString"), "userregistration");

            //Send Message
            string message = JsonConvert.SerializeObject(registrationRequest);
            var encodedMessage = new Message()
            {
                Body = Encoding.UTF8.GetBytes(message),
                ContentType = "application/json",
            };
            await queueClient.SendAsync(encodedMessage);

            //Feedback
            return new OkObjectResult("OK!");
        }
    }
}

