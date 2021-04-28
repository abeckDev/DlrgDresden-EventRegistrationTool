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


        //public AccountCreationRequest EncryptAccountCreationRequest(AccountCreationRequest creationRequest)
        //{
        //    if (!creationRequest.isEncrypted)
        //    {
        //        creationRequest.name = EncryptString(creationRequest.name);
        //        creationRequest.eMail = EncryptString(creationRequest.eMail);
        //        creationRequest.password = EncryptString(creationRequest.password);
        //        creationRequest.surname = EncryptString(creationRequest.surname);
        //        if (creationRequest.UserId != null)
        //        {
        //            creationRequest.UserId = EncryptString(creationRequest.UserId);
        //        }
        //        creationRequest.isEncrypted = true;
        //    }
        //    return creationRequest;
        //}

        //public AccountCreationRequest DecryptAccountCreationRequest(AccountCreationRequest creationRequest)
        //{
        //    if (creationRequest.isEncrypted)
        //    {
        //        creationRequest.name = DecryptString(creationRequest.name);
        //        creationRequest.eMail = DecryptString(creationRequest.eMail);
        //        creationRequest.password = DecryptString(creationRequest.password);
        //        creationRequest.surname = DecryptString(creationRequest.surname);
        //        if (creationRequest.UserId != null)
        //        {
        //            creationRequest.UserId = DecryptString(creationRequest.UserId);
        //        }
        //        creationRequest.isEncrypted = false;
        //    }
        //    return creationRequest;
        //}

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
