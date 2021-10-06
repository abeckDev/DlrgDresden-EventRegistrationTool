using System;
using System.Threading.Tasks;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Models;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions
{
    public static class UnregisterAttendeeFunction
    {
        [FunctionName("UnregisterAttendeeFunction")]
        public static async Task Run([ServiceBusTrigger("cancelregistration", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            //Load the encryption module
            var encryptionService = new EncryptionService();
            //Get the Object from the ServiceBus
            var deletionRequest = encryptionService.DecryptAttendeeRecord(JsonConvert.DeserializeObject<AttendeeRecord>(myQueueItem));
            //Get attendee service for table storage communication
            var attendeeService = new AttendeeService();
            //GetEmailService
            var emailService = new EmailService();
            //GetGraphApiService
            var graphApiService = new GraphApiService();

            //Delete Attendee from Table
            attendeeService.DeleteAttendee(deletionRequest.UserId);


            //Deregister Attendee in AzureAD
            //Build AdUser Object
            string tenantDomainName = System.Environment.GetEnvironmentVariable("tenantDomainName");


            await graphApiService.graphClient.Users[deletionRequest.Username + tenantDomainName]
                .Request()
                .DeleteAsync();

       
            //Inform Attendee via Mail 
            await emailService.SendRegistrationSucceededMail(deletionRequest.Email, deletionRequest.Name + " " + deletionRequest.Surname, deletionRequest.Username, deletionRequest.Password);
        }
    }
}
