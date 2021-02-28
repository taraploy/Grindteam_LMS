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
    public class CoursesAndRegistrationController : Controller

    {
        LMS_GRINDEntities1 gds;
        // GET: CoursesAndRegistration
        public ActionResult Index()
        {
            gds = new LMS_GRINDEntities1();
            return View(from course in gds.Courses.Take(10) select course);
        }

        public ActionResult ViewCourses()
        {
            return View("CoursesView");
        }
        public ActionResult AddCourse()
        {
            return View("AddCourseView");
        }

        public ActionResult Register()
        {
            return View("RegistrationView");
        }

        public ActionResult AddRegistration()
        {
            gds = new LMS_GRINDEntities1();
            StudentCours sc = new StudentCours();

            //sc.course_id = course.course_id;
            sc.student_id = Name.user_id;

            gds.StudentCourses.Add(sc);


            return View("RegistrationsView");
        }

        public ActionResult Unregister()
        {
            return View("RegistrationsView");
        }

        [HttpGet]
        public ActionResult CourseList()
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            var getCourseList = gds.Courses.ToList();
            SelectList list = new SelectList(getCourseList, "course_name", "days_of_week");
            ViewBag.courselistname = list;

            return View();
        }

        [HttpPost]
        public ActionResult SaveCourse(Course c, ulUser u)
        {
            gds = new LMS_GRINDEntities1();
            Cours course = new Cours();

            course.course_name = c.courseName;
            //Course.courseName = course.course_name;
            course.course_desc = c.courseDesc;
            course.course_num = c.courseNum;
            course.max_capacity = c.maxCapacity;
            //course.dept_id = c.deptId;
            course.dept_id = 1;
            course.num_credits = c.numCredits;
            course.days_of_week = c.meetingDays;
            course.start_time = c.startTime;
            course.end_time = c.endTime;
            course.building = c.location;
            course.room_no = c.roomNum;

            gds.Courses.Add(course);
            gds.SaveChanges();

            InstructorCours insCourse = new InstructorCours();
            insCourse.course_id = c.courseId;
            //insCourse.instructor_id = u.ulUser_id;
            insCourse.instructor_id = 7;

            gds.InstructorCourses.Add(insCourse);
            gds.SaveChanges();

            return View("CoursesView");
        }

        public ActionResult DepartmentList()
        {
            List<Department> ListDepartments = new List<Department>()
            {
                new Department() {dept_id = 1, dept_name = "Computer Science"}
            };

            return View();
        }

    }



    //public JsonResult InsertCourses(List<Cours> courses)
    //{
    //    using (LMS_GRINDEntities1 gds = new LMS_GRINDEntities1())
    //    {
    //        gds.Database.ExecuteSqlCommand("TRUNCATE TABLE [Courses]");
    //        if (courses == null)
    //        {
    //            courses = new List<Cours>();
    //        }

    //        foreach (Cours course in courses)
    //        {
    //            gds.Courses.Add(course);
    //        }
    //        int insertedRecords = gds.SaveChanges();
    //        return Json(insertedRecords);
    //    }
    //}
}
//public class dropdownModel
//{
//    var dbValues = db.EntityList.ToList();

//    var DropDownList = new new SelectListS(dbValues.Select(item => new SelectListItem
//{
//     Text = item.SelectedDbText,
//      Value = item.SelectedDbValue
//}).ToList(), "Value", "Text");
//     var viewModel = nwe YourViewModel()
//{
//  DropDownList = DownList
//};
// return View(viewModel);
//}