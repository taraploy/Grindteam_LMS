using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class SecurityQuestion
    {
        private int Security_Question_ID { get; set; }
        public List<string> Security_Question { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}