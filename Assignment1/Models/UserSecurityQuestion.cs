using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class UserSecurityQuestion
    {
        private int Secuerity_Question_ID { get; set; }
        private string Security_Answer { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}