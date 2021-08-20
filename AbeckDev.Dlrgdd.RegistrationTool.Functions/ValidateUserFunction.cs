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
using System.Collections.Generic;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Models;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions
{
    public static class ValidateUserFunction
    {
        [FunctionName("ValidateUserFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Get MetadataInformation
            var metaInformationService = new MetaInformationService();
            //Get AttendeeService
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
                !InputMessage.ContainsKey("city") ||
                !InputMessage.ContainsKey("zip"))
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
                City = InputMessage["city"],
                ZipCode = InputMessage["zip"]

            };

            //ToDo: Validate if user is Eligable for attendance based on Validation Module
            Dictionary<string, string> validationParameters = new Dictionary<string, string>()
            {
                {"vorname", registrationRequest.Name },
                {"nachname", registrationRequest.Surname },
                {"ort", registrationRequest.City },
                {"plz", registrationRequest.ZipCode },
                {"geburtsdat", $"{UserBirthday.Day.ToString("00")}/{UserBirthday.Month.ToString("00")}/{UserBirthday.Year.ToString()}" }
            };
            if (!attendeeService.IsValidMember(validationParameters, "AND"))
            {
                return new BadRequestObjectResult("User not found in member database!");
            }
            //If yes, extract userId from CSV and write it to the request
            string memberId = attendeeService.GetMemberIdFromMemberTable(validationParameters, "AND");

            //Check if account exists already via userId
            if (attendeeService.DoesUserExist(memberId))
            {
                return new BadRequestObjectResult("You are already registered with that membership!");
            }

            //Check if account exists already via E-Mail
            if (attendeeService.IsEmailAlreadyUsed(registrationRequest.EmailAddress))
            {
                return new BadRequestObjectResult("That E-Mail is already used!");
            }



            //Feedback
            return new OkObjectResult("OK!");
        }
    }
}
