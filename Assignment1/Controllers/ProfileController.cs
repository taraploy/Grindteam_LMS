using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Collections;
using System.Linq;
using Assignment1;
using Assignment1.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;


namespace Assignment1.Controllers
{
    public class ProfileController : Controller
    {
        LMS_GRINDEntities1 gds;

        public ActionResult Index()
        {
            return View("ProfilePage");
        }

        public ActionResult Edit()
        {
            return View("EditProfilePage");
        }

        [HttpPost]
        public ActionResult SaveChanges(ulUser u)
        {
            gds = new LMS_GRINDEntities1();
            var user = gds.ulUsers.Where(x => x.ulUser_id == Name.user_id).First();
            //ulUser user = new ulUser();
            
            try
            {
                // Set static name variables
                user.first_name = u.first_name;
                Name.first_name = user.first_name;
                user.last_name = u.last_name;
                Name.last_name = user.last_name;
                user.bio = u.bio;
                Name.bio = user.bio;
                user.street_address = u.street_address;
                Name.streetAddress = user.street_address;
                user.phone_num = u.phone_num;
                Name.phoneNum = user.phone_num;
                user.link1 = u.link1;
                Name.link1 = user.link1;
                user.link2 = u.link2;
                Name.link2 = user.link2;
                user.link3 = u.link3;
                Name.link3 = user.link3;

                if (u.File != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(u.File.FileName) +
                                        DateTime.Now.ToString("yymmssfff") +
                                        Path.GetExtension(u.File.FileName);
                    u.profileImage = "~/ProfileImages/" + fileName;
                    Name.profileImage = u.profileImage;
                    user.profileImage = u.profileImage;
                    fileName = Path.Combine(Server.MapPath("~/ProfileImages/"), fileName);
                    u.File.SaveAs(fileName);
                }

                gds.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
                //Error
            }


            return View("ProfilePage");
        }

    }
}