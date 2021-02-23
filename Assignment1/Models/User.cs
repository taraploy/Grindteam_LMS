using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class User
    {
        public int User_ID { get; set; }
        public DateTime Birthdate { get; set; }
        public string Password { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}