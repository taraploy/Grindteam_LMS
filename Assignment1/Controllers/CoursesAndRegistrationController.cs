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
        List<Cours> allCourses = new List<Cours>();
        // GET: CoursesAndRegistration
        public ActionResult Index()
        {
            gds = new LMS_GRINDEntities1();
            return View(from course in gds.Courses.Take(10) select course);
        }

        public ActionResult InstructorCview()
        {
            gds = new LMS_GRINDEntities1();

            return View(from course in gds.Courses.Take(100) select course);
            //return View(from c in gds.Courses join i in gds.InstructorCourses on c.course_id equals i.course_id  where @Name.user_id = c.instructor_id select new { c.course_num, c.course_name. c.course_desc, i.instructor_id, c.num_credits, c.days_of_week, c.start_time, c.end_time, c.building, c.room_no, c.max_capacity });



            //try
            //{
            //    return View(from course in gds.Courses.Take(100) where searchDepartment == course.dept_id && searchKeyword == course.course_name select course);
            //}
            //catch (Exception e)
            //{
            //    //Console.WriteLine("Error");
            //}

            //if (searchDepartment.ToString() != "" && searchKeyword != "")
            //{
            //    return View(from course in gds.Courses.Take(100) where searchDepartment == course.dept_id && searchKeyword == course.course_name select course);
            //}
            //else if (searchKeyword == "")
            //{
            //    return View(from course in gds.Courses.Take(100) where searchDepartment == course.dept_id select course);

            //}
            //else if (searchDepartment.ToString() == "")
            //{
            //    return View(from course in gds.Courses.Take(100) where searchKeyword == course.course_name select course);
            //}
            //else
            //{
            //    
            //}
            //return View((from course in gds.Courses.Take(100) select course, (from user in gds.ulUsers.Take(100) select user)));
            //return View(from Course, Departments, InstructorCourses, ulUser  in gds.Courses.Take(25) select course);


            //using (gds)
            //{
            //    var query = (from c in Courses join d in Departments on c.dept_id equals d.dept_id join i in InstructorCourses on c.instructor_id equals i.instructor_id join u in ulUser on i.instructor_id equals u.ulUser_id select new { c.course_num, c.course_name, c.course_desc, u.first_name, u.last_name, d.department });
            //}

        }


        public ActionResult ViewCourses()
        {
            return View("CoursesView");
        }
        public ActionResult AddCourse()
        {
            return View("AddCourseView");
        }

        //public ActionResult Register()
        //{
        //    return View("RegistrationView");
        //}

        /// <summary>
        /// Generates list for dropdown and returns RegistrationView
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewRegistration()
        {
            // for dropdown list on Registration View
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            List<string> displayList = new List<string>();
            var courseList = gds.Courses.ToList();
            string sStart;
            string sEnd;
            DateTime dtStartTime;
            DateTime dtEndTime;

            foreach (var course in courseList)
            {
                if (course.course_name != null)
                {
                    // convert time format
                    dtStartTime = DateTime.Today.Add((TimeSpan)course.start_time);
                    sStart = dtStartTime.ToString("hh:mm tt");
                    dtEndTime = DateTime.Today.Add((TimeSpan)course.end_time);
                    sEnd = dtEndTime.ToString("hh:mm tt");

                    displayList.Add(course.course_name + " " + sStart + " - " + sEnd + " " + course.days_of_week);
                }
            }

            SelectList list = new SelectList(displayList);
            ViewBag.courselistname = list;

            return View("RegistrationView");
        }

        /// <summary>
        /// Adds a StudentCourse to the db
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRegistration(FormCollection form) // TO DO: verify that user has not already added course
        {

            StudentCours sc = new StudentCours();
            List<string> displayList = new List<string>();
            gds = new LMS_GRINDEntities1();
            string selectedItem = form[0].ToString();
            var courseList = gds.Courses.ToList();
            int index = 0;
            string sStart;
            string sEnd;
            DateTime dtStartTime;
            DateTime dtEndTime;

            foreach (var course in courseList)
            {
                if (course.course_name != null)
                {
                    // convert time format
                    dtStartTime = DateTime.Today.Add((TimeSpan)course.start_time);
                    sStart = dtStartTime.ToString("hh:mm tt");
                    dtEndTime = DateTime.Today.Add((TimeSpan)course.end_time);
                    sEnd = dtEndTime.ToString("hh:mm tt");

                    displayList.Add(course.course_name + " " + sStart + " - " + sEnd + " " + course.days_of_week);
                }
            }

            foreach (var item in displayList)
            {
                if (selectedItem == item)
                {
                    break;
                }

                index++;
            }

            sc.course_id = courseList[index].course_id;
            sc.student_id = Name.user_id;

            //gds.StudentCourses.Add(sc);
            //gds.SaveChanges();
            return RedirectToAction("ReturnToView", "UserAccount", null);
        }

        [HttpPost]
        public ActionResult Unregister(FormCollection form)
        {
            StudentCours sc = new StudentCours();
            List<string> displayList = new List<string>();
            gds = new LMS_GRINDEntities1();
            var courseList = gds.Courses.ToList();
            string selectedItem = form[0].ToString();
            int index = 0;

            try
            {
                foreach (var item in displayList)
                {
                    if (selectedItem == item)
                    {
                        break;
                    }

                    index++;
                }

                sc.course_id = courseList[index].course_id;
                sc.student_id = Name.user_id;


                //Try to remove it
                gds.StudentCourses.Remove(sc);

                //Save the changes
                gds.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View();
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
        public ActionResult SaveCourse(string courseNum, string courseName, string courseDesc, 
            int courseCredits, int maxCapacity, string courseLocation, 
            int courseRoom, int departments,  string monday, string tuesday, string wednesday, 
            string thursday,  string friday, TimeSpan startTime, TimeSpan endTime)
        {
            gds = new LMS_GRINDEntities1();
            Cours course = new Cours();

            
            course.course_name = courseName;
            Course.courseName = courseName;
            course.course_desc = courseDesc;
            Course.courseDesc = courseDesc;
            course.course_num = courseNum;
            Course.courseNum = courseNum;
            course.max_capacity = maxCapacity;
            Course.maxCapacity = maxCapacity;
            course.dept_id = departments;
            //Course.department = Department.dept_name;
            course.num_credits = courseCredits;
            Course.numCredits = courseCredits;
            course.days_of_week = monday + tuesday + wednesday + thursday + friday;
            Course.meetingDays = monday + tuesday + wednesday + thursday + friday;
            course.start_time = startTime;
            Course.startTime = startTime;
            course.end_time = endTime;
            Course.endTime = endTime;
            course.building = courseLocation;
            Course.location = courseLocation;
            course.room_no = courseRoom;
            Course.roomNum = courseRoom;

            gds.Courses.Add(course);
            gds.SaveChanges();

            Course.courseId = course.course_id;

            InstructorCours insCourse = new InstructorCours();
            InstructorCourseContext.courseId = course.course_id;
            insCourse.course_id = course.course_id;
            InstructorCourseContext.instructorId = Name.user_id;
            insCourse.instructor_id = Name.user_id;
            Course.instructorFullName = Name.first_name + " " + Name.last_name;
            Course.instructorLastName = Name.last_name;
            Course.instructorFirstName = Name.first_name;

            gds.InstructorCourses.Add(insCourse);
            gds.SaveChanges();

            InstructorCourseContext.instructorCourseId = insCourse.instructor_course_id;
            allCourses.Add(course);
            return View("CoursesView");
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