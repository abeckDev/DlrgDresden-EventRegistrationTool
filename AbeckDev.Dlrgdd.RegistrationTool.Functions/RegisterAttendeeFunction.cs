using System;
using System.Collections.Generic;
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
            //GetGraphApiService
            var graphApiService = new GraphApiService();

            //Save Attendee information to the table storage --> may be crypted
            var attendee = encryptionService.DecryptAttendeeRecord(attendeeService.CreateAttendeeRecord(registrationRequest));

            //Register Attendee in AzureAD

            //Build AdUser Object
            string tenantDomainName = System.Environment.GetEnvironmentVariable("tenantDomainName");
            var AdUser = new User
            {
                AccountEnabled = true,
                //Generated user name with @<tenant>.onmicrosoft.com at the end
                UserPrincipalName = attendee.Username + tenantDomainName,
                DisplayName = attendee.Name + "  "+ attendee.Surname,
                Surname = attendee.Surname,
                GivenName = attendee.Name,
                UserType = "Guest",
                UsageLocation = "DE",
                CompanyName = "JHV-Mitglieder",
                MailNickname = attendee.Name+ "" + attendee.Surname,
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = false,
                    Password = attendee.Password
                },
            };

            //ToDo: Create error handling!
            //CreateAdUser
            var createdUser = await graphApiService.graphClient.Users
                .Request()
                .AddAsync(AdUser);

            //Inform Attendee via Mail 
            await emailService.SendRegistrationSucceededMail(attendee.Email, attendee.Name + " " + attendee.Surname, attendee.Username, attendee.Password);
        }
    }
}
