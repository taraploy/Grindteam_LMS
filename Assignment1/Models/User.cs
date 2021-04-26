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
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Link3 { get; set; }
        public string ProfileImage { get; set; }
        public string LinkTitle1 { get; set; }
        public string LinkTitle2 { get; set; }
        public string LinkTitle3 { get; set; }

        public virtual ICollection<InstructorCours> InstructorCours { get; set; }
        public virtual ICollection<StudentCours> StudentCours { get; set; }
        
        public override string ToString()
        {
            return base.ToString();
        }
    }
}