using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Collections;
using System.Linq;
using Assignment1.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;


namespace Assignment1.Controllers
{
    public class ProfileController : Controller
    {
        LMS_GRINDEntities1 gds;
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View("EditProfilePage");
        }

        [HttpPost]
        public ActionResult SaveChanges(User u)
        {
            gds = new LMS_GRINDEntities1();
            var user = gds.ulUsers.First(x => x.email_address == Name.email);
            try
            {
                // Set static name variables
                user.first_name = u.First_Name;
                Name.first_name = user.first_name;
                user.last_name = u.Last_Name;
                Name.last_name = user.last_name;
                user.bio = u.Bio;
                Name.bio = user.bio;
                user.street_address = u.StreetAddress;
                Name.streetAddress = user.street_address;
                user.phone_num = u.PhoneNumber;
                Name.phoneNum = user.phone_num;
                user.link1 = u.Link1;
                Name.link1 = user.link1;
                user.link2 = u.Link2;
                Name.link2 = user.link2;
                user.link3 = u.Link3;
                Name.link3 = user.link3;

                //TODO profile image functionality
                //user.profileImage = Name.profileImage;

                gds.SaveChanges();
                return View("ProfilePage");
            }
            catch (Exception ex)
            {
                //Error
            }

            return View("EditProfilePage");
        }

    }
}