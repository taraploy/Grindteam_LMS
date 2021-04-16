using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    public class PopUpController : Controller
    {
        LMS_GRINDEntities1 gds;

        // GET: PopUp
        public ActionResult Index()
        {
            gds = new LMS_GRINDEntities1();

            return View(from StudentCours in gds.StudentCourses.Take(5) select StudentCours);
        }

        [HttpPost]
        public ActionResult Details(DateTime due_date)
        {
            gds = new LMS_GRINDEntities1();

            return PartialView("Details", gds.Assignments.Find(due_date)); //Assignments might throw an error //it does on my editor
            //return PartialView("Details", entities.Customers.Find(customerId));
        }

        [HttpPost]
        public ActionResult PopUpItem(DateTime due_date)
        {
            gds = new LMS_GRINDEntities1();

            return PartialView("Details", gds.Assignments.Find(due_date)); //Assignments might throw an error //it does on my editor
            //return PartialView("Details", entities.Customers.Find(customerId));
        }


    }
}