using AbeckDev.Dlrgdd.RegistrationTool.Functions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Services
{
    public class EncryptionService
    {
        public byte[] EncryptionKey { get; set; }
        public byte[] IV { get; set; }

        public EncryptionService()
        {
            EncryptionKey = Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("EncryptionKey"));
            IV = Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("AES_IV"));
        }



        public AttendeeRecord EncryptÁttendeeRecord(AttendeeRecord attendeeRecord)
        {
            if (!attendeeRecord.IsEncrypted)
            {
                //Is not encrypted yet - we need to work
                attendeeRecord.Name = EncryptString(attendeeRecord.Name);
                attendeeRecord.Surname = EncryptString(attendeeRecord.Surname);
                attendeeRecord.Email = EncryptString(attendeeRecord.Email);
                attendeeRecord.Birthday = EncryptString(attendeeRecord.Birthday);
                attendeeRecord.City = EncryptString(attendeeRecord.City);
                attendeeRecord.ZipCode = EncryptString(attendeeRecord.ZipCode);
                attendeeRecord.Password = EncryptString(attendeeRecord.Password);
                attendeeRecord.Username = EncryptString(attendeeRecord.Username);
                attendeeRecord.IsEncrypted = true;
            }
            return attendeeRecord;
        }

        public AttendeeRecord DecryptÁttendeeRecord(AttendeeRecord attendeeRecord)
        {
            if (attendeeRecord.IsEncrypted)
            {
                //Is not encrypted yet - we need to work
                attendeeRecord.Name = DecryptString(attendeeRecord.Name);
                attendeeRecord.Surname = DecryptString(attendeeRecord.Surname);
                attendeeRecord.Email = DecryptString(attendeeRecord.Email);
                attendeeRecord.Birthday = DecryptString(attendeeRecord.Birthday);
                attendeeRecord.City = DecryptString(attendeeRecord.City);
                attendeeRecord.ZipCode = DecryptString(attendeeRecord.ZipCode);
                attendeeRecord.Password = DecryptString(attendeeRecord.Password);
                attendeeRecord.Username = DecryptString(attendeeRecord.Username);
                attendeeRecord.IsEncrypted = false;
            }
            return attendeeRecord;
        }



        public UserRegistrationRequest EncryptRegistrationRequest(UserRegistrationRequest creationRequest)
        {
            if (!creationRequest.IsEncrypted)
            {
                //Is not encrypted yet - we need to work
                creationRequest.Name = EncryptString(creationRequest.Name);
                creationRequest.Surname = EncryptString(creationRequest.Surname);
                creationRequest.EmailAddress = EncryptString(creationRequest.EmailAddress);
                creationRequest.Birthday = EncryptString(creationRequest.Birthday);
                creationRequest.City = EncryptString(creationRequest.City);
                creationRequest.ZipCode = EncryptString(creationRequest.ZipCode);
                if (creationRequest.UserId != null)
                {
                    creationRequest.UserId = EncryptString(creationRequest.UserId);
                }
                creationRequest.IsEncrypted = true;
            }
            return creationRequest;
        }

        public UserRegistrationRequest DecryptRegistrationRequest(UserRegistrationRequest creationRequest)
        {
            if (creationRequest.IsEncrypted)
            {
                //Is  encrypted yet - we need to work
                creationRequest.Name = DecryptString(creationRequest.Name);
                creationRequest.Surname = DecryptString(creationRequest.Surname);
                creationRequest.EmailAddress = DecryptString(creationRequest.EmailAddress);
                creationRequest.Birthday = DecryptString(creationRequest.Birthday);
                creationRequest.City = DecryptString(creationRequest.City);
                creationRequest.ZipCode = DecryptString(creationRequest.ZipCode);
                if (creationRequest.UserId != null)
                {
                    creationRequest.UserId = DecryptString(creationRequest.UserId);
                }
                creationRequest.IsEncrypted = false;
            }
            return creationRequest;
        }

        public string EncryptString(string StringToEncrypt)
        {
            using (Aes myaes = Aes.Create())
            {
                byte[] array = EncryptStringToBytes_Aes(StringToEncrypt, EncryptionKey, IV);
                return System.Convert.ToBase64String(array);
            }
        }

        public string DecryptString(string StringToDecrypt)
        {
            using (Aes myase = Aes.Create())
            {

                byte[] array = Convert.FromBase64String(StringToDecrypt);
                return DecryptStringFromBytes_Aes(Convert.FromBase64String(StringToDecrypt), EncryptionKey, IV);
            }
        }

        byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");


            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
