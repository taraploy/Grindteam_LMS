using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment1.Models;
namespace Assignment1.Controllers
{
    public class PaymentController : Controller
    {
        LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

        // GET: Payment
        public ActionResult Balance()
        {
            var semesterCredits = (from c in gds.Courses
                                join sc in gds.StudentCourses
                                on c.course_id equals sc.course_id
                                where sc.student_id == Name.user_id
                                select c).ToList();


            return View("BalanceView", semesterCredits);
        }
        
        public ActionResult PaymentInfo()
        {
            return View("PaymentInfoView");
        }
    }
}