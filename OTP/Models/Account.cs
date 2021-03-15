using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTP.Models
{
    public class Account
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public int Active { get; set; }
        public String Otp { get; set; }
    }
}