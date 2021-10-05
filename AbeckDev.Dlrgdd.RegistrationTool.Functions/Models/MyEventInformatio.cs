using System;
using System.Collections.Generic;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Models
{
    public class MyEventInformation
    {

        public MyEventInformation(AttendeeRecord attendeeRecord)
        {
            UserId = attendeeRecord.UserId;
            Name = attendeeRecord.Name;
            Surname = attendeeRecord.Surname;
            Email = attendeeRecord.Email;
            Username = attendeeRecord.Username;
            Birthday = attendeeRecord.Birthday;
            City = attendeeRecord.City;
            ZipCode = attendeeRecord.ZipCode;
        }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Birthday { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}
