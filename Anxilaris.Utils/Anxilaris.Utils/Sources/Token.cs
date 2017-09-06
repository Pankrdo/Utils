//-----------------------------------------------------------------------
// <copyright file="Ip.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class Token
    {
        private static string Key = "RaJuAdMaBDAnx";
        private static byte[] IV = Encoding.ASCII.GetBytes("BestDay.Anxilaris");

        /// <summary>
        /// Encrypts a string with default key
        /// </summary>
        /// <param name="str">string to encrypt</param>
        /// <returns>encrypted string</returns>
        public static string Encrypt(string str)
        {
            return Encrypt(str, Key);
        }

        /// <summary>
        /// Encrypts a string with default key
        /// </summary>
        /// <param name="str">string to encrypt</param>
        /// <returns>encrypted string</returns>
        public static string EncryptURL(string str)
        {
            string url = Encrypt(str, Key);
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// Encrypts a string with default key
        /// </summary>
        /// <param name="str">string to encrypt</param>
        /// <returns>encrypted string</returns>
        public static string EncodeURL(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// Encrypts a string with specified key
        /// </summary>
        /// <param name="str">string to encrypt</param>
        /// <param name="key">key to encrypt</param>
        /// <returns>encrypted string</returns>
        public static string Encrypt(string str, string key)
        {
            string result = String.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] inputBytes = Encoding.ASCII.GetBytes(str);
                byte[] TextEncripted;
                RijndaelManaged cripto = new RijndaelManaged();
                using (MemoryStream ms = new MemoryStream(inputBytes.Length))
                {
                    using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(keyBytes, IV), CryptoStreamMode.Write))
                    {
                        objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        objCryptoStream.FlushFinalBlock();
                        objCryptoStream.Close();
                    }
                    TextEncripted = ms.ToArray();
                }
                result = Convert.ToBase64String(TextEncripted);
            }
            return result;
        }

        /// <summary>
        /// Encrypts a string with default key
        /// </summary>
        /// <param name="str">string to encrypt</param>
        /// <returns>encrypted string</returns>
        public static string DecryptURL(string str)
        {
            string url = HttpUtility.UrlDecode(str);
            return Decrypt(url, Key);
        }

        /// <summary>
        /// Encrypts a string with default key
        /// </summary>
        /// <param name="str">string to encrypt</param>
        /// <returns>encrypted string</returns>
        public static string DecodetURL(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// Encrypts a string with default key
        /// </summary>
        /// <param name="str">string to decrypt</param>
        /// <returns>decrypted string</returns>
        public static string Decrypt(string str)
        {
            return Decrypt(str, Key);
        }

        /// <summary>
        /// Encrypts a string with specified key
        /// </summary>
        /// <param name="str">string to decrypt</param>
        /// <param name="key">key to encrypt</param>
        /// <returns>decrypted string</returns>
        public static string Decrypt(string str, string key)
        {
            string textDecripted = String.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] inputBytes = Convert.FromBase64String(str);
                byte[] resultBytes = new byte[inputBytes.Length];
                RijndaelManaged cripto = new RijndaelManaged();
                using (MemoryStream ms = new MemoryStream(inputBytes))
                {
                    using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(keyBytes, IV), CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(objCryptoStream, true))
                        {
                            textDecripted = sr.ReadToEnd();
                        }
                    }
                }
            }
            return textDecripted;
        }
    }
}
