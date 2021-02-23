using Assignment1.Models;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Collections;
using System.Linq;

namespace Assignment1.Controllers
{
    public class UserAccountController : Controller
    {

        /// <summary>
        /// LMS_GRINDEntities object
        /// </summary>
        LMS_GRINDEntities gds;


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //void connectionString()
        //{
        //    conn.ConnectionString = "Data Source=titan.cs.weber.edu,10433;Initial Catalog=LMS_GRIND;User ID=LMS_GRIND;Password=Dontstop21";
        //}

        /// <summary>
        /// Connect to database and verify that the user exists
        /// If the user exists, then return to Success View
        /// otherwise, return to the login screen
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Verify(User u)
        {
            //Encrypt password functionality
            byte[] data = System.Text.Encoding.ASCII.GetBytes(u.Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);

            gds = new LMS_GRINDEntities();
            var query = gds.ulUsers.Where(x => x.email_address == u.Email && x.user_password == hash);

            if (query.Any())
            {
                foreach (ulUser user in query)
                {
                    // Set static name variables
                    Name.first_name = user.first_name;
                    Name.last_name = user.last_name;
                    Name.role = user.role;
                }

                if (Name.role == "Instructor")
                {
                    return View("InstructorView");
                }
                else
                {
                    return View("StudentView");
                }
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult Index()
        {
            return View("SignUp");
        }

        /// <summary>
        /// SignUp method
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignUp(User u)
        {
            //Encrypt password functionality
            byte[] data = System.Text.Encoding.ASCII.GetBytes(u.Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);

            gds = new LMS_GRINDEntities();
            var query = gds.ulUsers.Where(x => x.email_address == u.Email);

            // If the query returns any results, then that
            // email address is already taken, otherwise
            // proceed to create user.
            if (query.Any())
            {
                return View("SignUp");
            }
            else
            {
                ulUser user = new ulUser();

                // set user's values
                user.first_name = u.First_Name;
                user.last_name = u.Last_Name;
                user.birthdate = u.Birthdate;
                user.email_address = u.Email;
                user.user_password = hash;
                user.role = u.Role;

                // add new user to database
                gds.ulUsers.Add(user);
                gds.SaveChanges();

                // set static variables
                Name.first_name = u.First_Name;
                Name.last_name = u.Last_Name;
                Name.role = u.Role;

                if (Name.role == "Instructor")
                {
                    return View("InstructorView");
                }
                else
                {
                    return View("StudentView");
                }
            }
        }
    }
}