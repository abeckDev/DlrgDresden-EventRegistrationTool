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
using CsvHelper;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions
{
    public static class GetAttendeeRecordsFunction
    {
        [FunctionName("GetAttendeeRecordsFunction")]
        public static async  Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Get encryption service
            var encryptionService = new EncryptionService();
            //Get attendee service
            var attendeeService = new AttendeeService();

            //Build request mode
            string userId;
            string exportFormat;

            var InputMessage = req.Query;
            //set userId to parse
            if (InputMessage.ContainsKey("userId"))
            {
                userId = InputMessage["userId"];
            }
            else
            {
                userId = "";
            }

            //Set export format
            if (InputMessage.ContainsKey("exportFormat"))
            {
                exportFormat = InputMessage["exportFormat"];
            }
            else
            {
                exportFormat = "json";
            }
            if (exportFormat != "json" && exportFormat != "csv")
            {
                return new BadRequestObjectResult("That export format is not supported!");
            }

            //Create List to be filled based on userId
            var attendeeList = new List<AttendeeRecord>();

            if (userId == "")
            {
                //Get all attendee records
                var encryptedattendees = attendeeService.GetAllAttendeeRecords();
                foreach (var attendee in encryptedattendees)
                {
                    attendeeList.Add(encryptionService.DecryptAttendeeRecord(attendee));
                }
            }
            else
            {
                //Get specific record
                var attendeeRecord = attendeeService.GetAttendeeRecord(userId);
                if (attendeeRecord == null)
                {
                    return new NotFoundObjectResult("That user could not be found!");
                }

                attendeeList.Add(encryptionService.DecryptAttendeeRecord(attendeeService.GetAttendeeRecord(userId)));
            }

            //Format list based on export format and return
            switch (exportFormat)
            {
                case "json":
                    //Handle Json response
                    return new OkObjectResult(JsonConvert.SerializeObject(attendeeList));
                case "csv":
                    //Handle CSV Response
                    var stringBuilder = new StringBuilder();
                    var TextWriter = new StringWriter(stringBuilder);
                    var csv = new CsvWriter(TextWriter, System.Globalization.CultureInfo.InvariantCulture);
                    csv.WriteRecords(attendeeList);
                    return new OkObjectResult(stringBuilder.ToString());
                default:
                    return new BadRequestObjectResult("That output format is not supported!");
            }

        }
    }
}
