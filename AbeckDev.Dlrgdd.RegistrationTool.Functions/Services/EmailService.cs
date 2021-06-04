using Newtonsoft.Json;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Services
{
    public class EmailService
    {
        TransactionalEmailsApi apiInstance;
        SendSmtpEmailSender senderMail;
        SendSmtpEmailReplyTo senderReplyToMail;
        long? welcomeMailTemplateId;

        public EmailService()
        {
            //Get E-Mail Instance
            var configuration = new Configuration();
            configuration.ApiKey.Add("api-key", System.Environment.GetEnvironmentVariable("SendInBlueApiKey"));
            apiInstance = new TransactionalEmailsApi(configuration);
            welcomeMailTemplateId = long.Parse(System.Environment.GetEnvironmentVariable("RegistrationSucceededMailTemplateId"));

            //Define Sender Information
            senderMail = new SendSmtpEmailSender(System.Environment.GetEnvironmentVariable("eMailNameFrom"), System.Environment.GetEnvironmentVariable("eMailAddressFrom"));
            senderReplyToMail = new SendSmtpEmailReplyTo(System.Environment.GetEnvironmentVariable("eMailAddressFrom"), System.Environment.GetEnvironmentVariable("eMailNameFrom"));
        }

        public async Task SendEMail(string subject, string toMail, string content)
        {
            //Set receipient
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(new SendSmtpEmailTo(toMail));

            //Create Dummy Bcc and Cc
            var Bcc = new List<SendSmtpEmailBcc>();
            var Cc = new List<SendSmtpEmailCc>();

            string HtmlContent = content;
            string TextContent = null;
            string Subject = subject;
            SendSmtpEmailReplyTo ReplyTo = senderReplyToMail;

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(senderMail, To, Bcc, Cc, HtmlContent, TextContent, Subject, ReplyTo);
                CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);

            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong while sending Mail", e);
            }
        }


        public async Task SendRegistrationSucceededMail(string toMail, string name, string username, string password)
        {
            //Set receipient
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(new SendSmtpEmailTo(toMail, name));

            //Define dummy content
            string HtmlContent = null;
            string TextContent = null;
            string Subject = null;

            //Set mail specific settings
            SendSmtpEmailReplyTo ReplyTo = senderReplyToMail;
            object parameters = new RegistrationMailModel
            {
                name = name,
                username = username,
                password = password,
                editAccountUri = System.Environment.GetEnvironmentVariable("EditAccountBaseUri")+$"?userid={username}&eMail={toMail}",
            };


            try
            {
                var sendSmtpEmail = new SendSmtpEmail(senderMail, To, null, null, HtmlContent, TextContent, Subject, ReplyTo, null, null, welcomeMailTemplateId, parameters);
                CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);

            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong while sending Mail", e);
            }
        }

        private class RegistrationMailModel
        {
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("username")]
            public string username { get; set; }
            [JsonProperty("password")]
            public string password { get; set; }
            [JsonProperty("editAccountUri")]
            public string editAccountUri { get; set; }
        }

    }
}
