using Project_V3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Project_V3.ViewModels
{
    public class VMLoginModel
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入帳號")]

        public string Account { get; set; }

        string password;
        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string Password
        {
            
            get { return password; }
            set { password = hashPw.getHashPwd(value); }
        }
    }
}