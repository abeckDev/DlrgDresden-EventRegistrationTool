using System;
using System.Collections.Generic;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Models
{
    public class UserRegistrationRequest
    {

        public string UserId { get; set; }
        public string Name { get; set; }

        public string Surname  { get; set; }

        public string EmailAddress { get; set; }

        public string City { get; set; }
        public string ZipCode { get; set; }

        public string Birthday { get; set; }

        public bool IsValidatedOrgMember { get; set; } = false;
        
        public bool IsEncrypted { get; set; }

        public bool IsRegisteredInTableStorage { get; set; } = false;

        public bool IsUserInformedViaMail { get; set; } = false;

    }
}
