using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Project_V3.Models
{
    public class hashPw
    {
        public static string getHashPwd(string pwd)
        {
            byte[] hashPwd;
            string result = "";
            System.Text.UnicodeEncoding unicode = new System.Text.UnicodeEncoding();
            byte[] pwdBytes = unicode.GetBytes(pwd);

            SHA256 sha256 = SHA256.Create();
            hashPwd = sha256.ComputeHash(pwdBytes);

            foreach (byte b in hashPwd)
            {
                result += b.ToString();
            }
            return result;
        }
    }
}