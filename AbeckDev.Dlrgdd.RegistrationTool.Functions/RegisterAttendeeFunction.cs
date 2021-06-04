using System;
using System.Threading.Tasks;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Models;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions
{
    public static class RegisterAttendeeFunction
    {
        [FunctionName("RegisterAttendeeFunction")]
        public static async Task RunAsync([ServiceBusTrigger("userregistration", Connection = "ServiceBusConnectionString")]string myQueueItem, string messageId, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {messageId}");

            //Load the encryption module
            var encryptionService = new EncryptionService();
            //Get the Object from the ServiceBus
            var registrationRequest = encryptionService.DecryptRegistrationRequest(JsonConvert.DeserializeObject<UserRegistrationRequest>(myQueueItem));
            //Get attendee service for table storage communication
            var attendeeService = new AttendeeService();
            //GetEmailService
            var emailService = new EmailService();

            //Save Attendee information to the table storage --> may be crypted
            var attendee = encryptionService.Decrypt¡ttendeeRecord(attendeeService.CreateAttendeeRecord(registrationRequest));


            //Inform Attendee via Mail 
            await emailService.SendRegistrationSucceededMail(attendee.Email, attendee.Name + " " + attendee.Surname, attendee.Username, attendee.Password);




        }
    }
}
