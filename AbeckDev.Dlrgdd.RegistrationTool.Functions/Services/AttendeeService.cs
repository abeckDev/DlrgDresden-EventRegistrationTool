﻿using System;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Text;
using AbeckDev.Dlrgdd.RegistrationTool.Functions.Models;
using System.Linq;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Services
{
    public class AttendeeService
    {
        CloudTableClient cloudTableClient;

        CloudTable cloudTable;
        const string AttendeeTableName = "jhvAttendees";
        EncryptionService encryptionService;


        public AttendeeService()
        {
            cloudTableClient = CloudStorageAccount.Parse(System.Environment.GetEnvironmentVariable("StorageAccountConnectionString")).CreateCloudTableClient();
            cloudTable = CreateTableIfNotExist(AttendeeTableName);
            encryptionService = new EncryptionService();
        }

        CloudTable CreateTableIfNotExist(string TableName)
        {
            CloudTable table = cloudTableClient.GetTableReference(TableName);
            table.CreateIfNotExistsAsync();
            return table;
        }

        //Write User to Table Storage

        public AttendeeRecord CreateAttendeeRecord(UserRegistrationRequest userRegistration)
        {
            var attendee = new AttendeeRecord()
            {
                PartitionKey = "attendees",
                Name = userRegistration.Name,
                Surname = userRegistration.Surname,
                Email = userRegistration.EmailAddress,
                Birthday = userRegistration.Birthday,
                Address = userRegistration.Address,
                //ToDo: Activate in Prod as soon as validation is implemented!!!
                //UserId = userRegistration.UserId,
                UserId = GenerateUserId(), //I am evil, please remove me in the future
                Username = GenerateUsername(userRegistration.Name, userRegistration.Surname),
                Password = GenerateRandomPassword(),
            };

            //Encrypt Settings
            attendee = encryptionService.EncryptÁttendeeRecord(attendee);

            //Write to Table Storage
            TableResult result = cloudTable.Execute(TableOperation.InsertOrReplace(attendee));
            return result.Result as AttendeeRecord;
        }

        //Does User exist
        public bool DoesUserExist(string UserId)
        {
            if (GetAttendeeRecord(UserId) != null)
            {
                return true;
            }
            return false;
        }

        //Checks if only userId or userId+E-Mail is already used
        public string GetExistingAttendeeReason(string UserId, string EmailAddress)
        {
            var attendeeRecord = GetAttendeeRecord(UserId);

            if (attendeeRecord == null)
            {
                //Attendee is not registered yet
                return "NotExists";
            }

            if (attendeeRecord.Email == EmailAddress)
            {
                //UserId AND E-Mail match
                return "E-Mail";
            }

            //Only UserId matches
            return "UserId";

        }


        //Delete User from Table Storage
        public void DeleteAttendee(string UserId)
        {
            var attendee = GetAttendeeRecord(UserId);
            cloudTable.Execute(TableOperation.Delete(attendee));
        }


        //Edit User Settings in Table Storage
        public AttendeeRecord EditAttendeeInformation(AttendeeRecord attendeeRecord, string userId)
        {
            //Get old record
            var attendee = GetAttendeeRecord(userId);

            //assign new values
            attendee = attendeeRecord;

            var result = cloudTable.Execute(TableOperation.InsertOrReplace(attendee));

            return result.Result as AttendeeRecord;

        }


        //Get specific attendee record
        AttendeeRecord GetAttendeeRecord(string userId)
        {
            try
            {
                if (userId == "")
                {
                    return null;
                }
                TableQuery<AttendeeRecord> query = new TableQuery<AttendeeRecord>()
                    .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId));
                return cloudTable.ExecuteQuery(query).FirstOrDefault(a => a.UserId == userId);
            }
            catch (Exception)
            {
                return null;
            }
        }



        string GenerateUsername(string name, string surname)
        {
            bool Isunique = false;
            string username = name + "." + surname;

            if (!GetAllUserNames().Exists(i => i == username))
            {
                Isunique = true;
            }

            while (!Isunique)
            {
                username = username + "1";
                if (!GetAllUserNames().Exists(i => i == username))
                {
                    Isunique = true;
                }

            }
            return username;
        }


        //Generate UserId
        string GenerateUserId()
        {
            bool isUnique = false;
            string newKey = null;
            while (!isUnique)
            {
                newKey = new Random().Next(10000, 99999).ToString();
                if (!GetAllUserIds().Exists(i => i == newKey))
                {
                    isUnique = true;
                }
            }
            return newKey;
        }

        //Get all attendees
        public List<AttendeeRecord> GetAllAttendeeRecords()
        {
            var attendees = new List<AttendeeRecord>();
            foreach (var attendee in cloudTable.ExecuteQuery(new TableQuery<AttendeeRecord>()))
            {
                attendees.Add(attendee);
            }
            return attendees;
        }

        //Get all userIds
        List<string> GetAllUserIds()
        {
            var userIds = new List<string>();
            foreach (var attendee in cloudTable.ExecuteQuery(new TableQuery<AttendeeRecord>()))
            {
                userIds.Add(attendee.UserId);
            }
            return userIds;
        }

        //Get all userIds
        List<string> GetAllUserNames()
        {
            var userNames = new List<string>();
            foreach (var attendee in cloudTable.ExecuteQuery(new TableQuery<AttendeeRecord>()))
            {
                userNames.Add(encryptionService.DecryptÁttendeeRecord(attendee).UserId);
            }
            return userNames;
        }

        //Generate password
        string GenerateRandomPassword(int size = 9)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase  
        string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}