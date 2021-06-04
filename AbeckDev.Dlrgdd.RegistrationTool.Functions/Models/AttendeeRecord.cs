using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Models
{
    public class AttendeeRecord : TableEntity
    {
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public string UserId
        {
            get { return RowKey; }
            set { RowKey = value; }

        }


        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string Birthday { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public bool IsEncrypted { get; set; } = false;

    }
}
