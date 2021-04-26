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
        CourseRegistration cs;

        public ActionResult InstructorCview(string search)
        {
            gds = new LMS_GRINDEntities1();
            var departments = (from d in gds.Departments    // select all departments
                               select d);

            if (!String.IsNullOrEmpty(search))
            {
                departments = departments.Where(x => x.dept_name.Contains(search)); // filter based on search
            }

            CourseCardList.GenerateInstructorCourseList();

            return View("InstructorCview", departments.ToList().OrderBy(x => x.dept_name));
        }

        public ActionResult InstructorCourseInformation(int id)
        {
            gds = new LMS_GRINDEntities1();
            Cours course = gds.Courses.Where(x => x.course_id == id).FirstOrDefault();
            int ic_id = gds.InstructorCourses.Where(x => x.course_id == id).Select(x => x.instructor_course_id).FirstOrDefault();
            Department department = gds.Departments.Where(x => x.dept_id == course.dept_id).FirstOrDefault();
            int instructorId = gds.InstructorCourses.Where(x => x.instructor_course_id == ic_id).Select(x => x.instructor_id).FirstOrDefault();
            String instructorFirstName = gds.ulUsers.Where(x => x.ulUser_id == instructorId).Select(x => x.first_name).FirstOrDefault();
            String instructorLastName = gds.ulUsers.Where(x => x.ulUser_id == instructorId).Select(x => x.last_name).FirstOrDefault();

            ViewBag.selectedCourse = course;
            ViewBag.courseDepartment = department;
            ViewBag.InstructorName = instructorFirstName + " " + instructorLastName;
            return View("InstructorCourseInformationView");
        }

        public ActionResult AddCourse()
        {
            return View("AddCourseView");
        }

        /// <summary>
        /// Display course details for student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Display details of the selected course</returns>
        public ActionResult StudentCourseDetail(int id)
        {
            gds = new LMS_GRINDEntities1();
            Cours course = gds.Courses.Where(x => x.course_id == id).FirstOrDefault();
            int ic_id = gds.InstructorCourses.Where(x => x.course_id == id).Select(x => x.instructor_course_id).FirstOrDefault();
            Department department = gds.Departments.Where(x => x.dept_id == course.dept_id).FirstOrDefault();
            int instructorId = gds.InstructorCourses.Where(x => x.instructor_course_id == ic_id).Select(x => x.instructor_id).FirstOrDefault();
            String instructorFirstName = gds.ulUsers.Where(x => x.ulUser_id == instructorId).Select(x => x.first_name).FirstOrDefault();
            String instructorLastName = gds.ulUsers.Where(x => x.ulUser_id == instructorId).Select(x => x.last_name).FirstOrDefault();

            CourseCardList.GenerateStudentCourseList();
            AssignmentList.GenerateStudentAssignmentList(id);
            AssignmentList.GenerateThisStudentsSubmissions(Name.user_id);
            AssignmentList.GenerateThisStudentsSubmissionsForCourse(Name.user_id, ic_id);

            StudentCours studentCours = gds.StudentCourses.Where(x => (x.course_id == course.course_id) && (x.student_id == Name.user_id)).FirstOrDefault();

            // Calculate overall points
            int maxPoints = 0;
            int points = 0;
            double? gradePoints = 0;
            if (AssignmentList.StudentAssignments.Any())
            {
                foreach (var assignment in AssignmentList.StudentAssignments)
                {
                    AssignmentList.GenerateThisStudentsSubmissionForAssignment(assignment.AssignmentId);
                    if (AssignmentList.StudentAssignmentSubmission.isGraded)
                    {
                        points += (int)AssignmentList.StudentAssignmentSubmission.Grade;
                        maxPoints += (int)assignment.MaxPoints;
                    }
                }

                ViewBag.points = points;
                ViewBag.maxPoints = maxPoints;
                // Get letter grade 
                gradePoints = ((double)points / maxPoints) * 100;
                // Display 2 decimal places
                gradePoints = Math.Truncate(100 * (double)gradePoints) / 100;               
                if (gradePoints >= 0)
                {
                    ViewBag.gradePercentage = gradePoints + "%";  // Display percentage
                }
                else
                {
                    ViewBag.gradePercentage = " ";
                }
            }

            AssignmentList.GenerateThisStudentsSubmissionsForCourse(Name.user_id, ic_id);
            ViewBag.letterGrade =  studentCours.letter_grade;  // Display letter grade
            ViewBag.selectedCourse = course;
            ViewBag.courseDepartment = department;
            ViewBag.InstructorName = instructorFirstName + " " + instructorLastName;

            return View("StudentCourseDetailView");
        }          

        /// <summary>
        /// Display course details for instructor
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Display details of the selected course</returns>
        public ActionResult InstructorCourseDetail(int id)
        {
            gds = new LMS_GRINDEntities1();
            Cours course = gds.Courses.Where(x => x.course_id == id).FirstOrDefault();
            Department department = gds.Departments.Where(x => x.dept_id == course.dept_id).FirstOrDefault();
            int ic_id = gds.InstructorCourses.Where(x => x.course_id == id).Select(x => x.instructor_course_id).FirstOrDefault();
            CourseCardList.GenerateInstructorCourseList();
            AssignmentList.GenerateInstructorAssignmentList(id);

            // Calculate overall spread of letter grades for students in course
            LetterGradeList.GenerateCourseLetterGrades(id);
            ViewBag.percentA = LetterGradeList.PercentA;
            ViewBag.percentB = LetterGradeList.PercentB;
            ViewBag.percentC = LetterGradeList.PercentC;
            ViewBag.percentD = LetterGradeList.PercentD;
            ViewBag.percentF = LetterGradeList.PercentF;
            ViewBag.percentUngraded = LetterGradeList.PercentUngraded;
            ViewBag.selectedCourse = course;
            ViewBag.courseDepartment = department;
            return View("InstructorCourseDetailView");
        }

        /// <summary>
        /// Generates a list of courses and filters based on 
        /// search criteria
        /// </summary>
        /// <param name="search"></param>
        /// <returns>RegistrationView and a list of courses</returns>
        public ActionResult ViewRegistration(string search)
        {
            // for dropdown list on Registration View
            gds = new LMS_GRINDEntities1();

            var courses = (from c in gds.Courses
                           select c);

            if (!String.IsNullOrEmpty(search))
            {
                courses = courses.Where(x => x.course_num.Contains(search) ||
                                        x.course_name.Contains(search));
            }

            return View("RegistrationView", courses.ToList());
        }

        /// <summary>
        /// Adds a StudentCourse to the db
        /// </summary>
        /// <returns>StudentView</returns>
        [HttpPost]
        public ActionResult AddRegistration(FormCollection form)
        {
            StudentCours sc = new StudentCours();
            List<string> displayList = GetDisplayList();
            gds = new LMS_GRINDEntities1();
            string selectedItem = form["ddCourses"].ToString();
            var courseList = gds.Courses.ToList();
            int index = GetSelectedIndex(displayList, selectedItem);

            sc.course_id = courseList[index].course_id;
            sc.student_id = Name.user_id;

            // check database for specified course id and student id
            var ret = (from s in gds.StudentCourses
                             where s.student_id == Name.user_id
                             where s.course_id == sc.course_id
                             select s).Count();

            if (ret > 0)
            {
                ViewBag.Message = "Could not register for course. You are already registered.";
                return ViewRegistration("");
            }
            else
            {

                gds.StudentCourses.Add(sc);
                gds.SaveChanges();
                CourseCardList.GenerateStudentCourseList();
                ToDoList.GenerateStudentToDoList();

                return RedirectToAction("ReturnToView", "UserAccount", null);
            }
            
        }


        /// <summary>
        /// Removes a StudentCourse from db
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Unregister(FormCollection form) // TODO: Validation - verify user is enrolled in class before removing
        {
            gds = new LMS_GRINDEntities1();
            List<string> displayList = GetDisplayList();
            var courseList = gds.Courses.ToList();
            string selectedItem = form["ddCourses"].ToString(); // Item selected from dropdown
            int index = 0;

            try
            {

                // determine index of selected item
                index = GetSelectedIndex(displayList, selectedItem);

                // create a list of StudentCourses sc
                var sc = gds.StudentCourses.ToList();

                // query sc for the course to be unregistered and store in id
                var course = sc.First(x => x.student_id == Name.user_id && x.course_id == courseList[index].course_id);

                gds.StudentCourses.Remove(course);

                //Save the changes
                gds.SaveChanges();

                // update list and return to StudentView
                CourseCardList.GenerateStudentCourseList();
                ToDoList.GenerateStudentToDoList();
                return RedirectToAction("ReturnToView", "UserAccount", null);
            }
            catch
            {
                ViewBag.Message = "Could not unregister from course because no registration exists";
                return ViewRegistration("");
            }
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


        /// <summary>
        /// Save new course to database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCourse(string courseNum, string courseName, string courseDesc,
            int courseCredits, int maxCapacity, string courseLocation,
            int courseRoom, int departments, string monday, string tuesday, string wednesday,
            string thursday, string friday, TimeSpan startTime, TimeSpan endTime)
        {
            cs = new CourseRegistration();
            cs.SaveCourse2(courseNum, courseName, courseDesc, courseCredits, maxCapacity, courseLocation,
                courseRoom, departments, monday, tuesday, wednesday, thursday, friday, startTime, endTime, Name.user_id);

            return View("InstructorCView");
        }

        [HttpPost]
        public ActionResult UpdateCourse(int id, string courseNum, string courseName, string courseDesc,
            int courseCredits, int maxCapacity, string courseLocation,
            int courseRoom, int departments, string monday, string tuesday, string wednesday,
            string thursday, string friday, TimeSpan startTime, TimeSpan endTime)
        {
            gds = new LMS_GRINDEntities1();
            Cours course = gds.Courses.Where(x => x.course_id == id).FirstOrDefault();
            try
            {
                course.course_name = courseName;
                course.course_desc = courseDesc;
                course.course_num = courseNum;
                course.max_capacity = maxCapacity;
                course.dept_id = departments;
                course.num_credits = courseCredits;
                course.days_of_week = monday + tuesday + wednesday + thursday + friday;
                course.start_time = startTime;
                course.end_time = endTime;
                course.building = courseLocation;
                course.room_no = courseRoom;

                ViewBag.selectedCourse = course;
                Department department = gds.Departments.Where(x => x.dept_id == course.dept_id).FirstOrDefault();
                ViewBag.courseDepartment = department;
                
                gds.SaveChanges();
                CourseCardList.GenerateInstructorCourseList();
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
            catch
            {
                return View("EditCourseView");
            }
            return View("InstructorCourseInformationView");
            //return View("InstructorCview");
        }

        /// <summary>
        /// HTML controller method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCourse(int id)
        {
            gds = new LMS_GRINDEntities1();
            Cours course = gds.Courses.Where(x => x.course_id == id).FirstOrDefault();
            int department = gds.Departments.Where(x => x.dept_id == course.dept_id).Select(x => x.dept_id).FirstOrDefault();
            string daysOfWeek = gds.Courses.Where(x => x.course_id == id).Select(x => x.days_of_week).FirstOrDefault();
            ViewBag.selectedCourse = course;
            bool[] selectedDept = new bool[9];

            for (int i = 0; i < 9; i++)
            {
                if (i == department)
                {
                    selectedDept[i] = true;
                }
                else
                {
                    selectedDept[i] = false;
                }
            }

            ViewBag.deptArray = selectedDept;

            if (daysOfWeek.Contains("M"))
            {
                ViewBag.monday = true;
            }
            if (daysOfWeek.Contains("T"))
            {
                ViewBag.tuesday = true;
            }
            if (daysOfWeek.Contains("W"))
            {
                ViewBag.wednesday = true;
            }
            if (daysOfWeek.Contains("R"))
            {
                ViewBag.thursday = true;
            }
            if (daysOfWeek.Contains("F"))
            {
                ViewBag.friday = true;
            }
            return View("EditCourseView");
        }

        public ActionResult DeleteCourse(int id)
        {
            cs = new CourseRegistration();
            cs.DeleteCourse2(id);
            CourseCardList.GenerateInstructorCourseList();
            return RedirectToAction("InstructorCView");
        }

        /// <summary>
        /// Returns a list of strings used for registration dropdown
        /// </summary>
        /// <returns></returns>
        public List<string> GetDisplayList()
        {
            List<string> displayList = new List<string>();
            gds = new LMS_GRINDEntities1();
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

                    displayList.Add(course.course_num + " " + course.course_name + " " + sStart + " - " + sEnd + " " + course.days_of_week);
                }
            }

            return displayList;

        }

        /// <summary>
        /// Returns the index of the selected item from dropdown
        /// </summary>
        /// <param name="displayList"></param>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        public int GetSelectedIndex(List<string> displayList, string selectedItem)
        {
            int index = 0;

            foreach (var item in displayList)
            {
                if (selectedItem == item)
                {
                    break;
                }

                index++;
            }

            return index;
        }

        /// <summary>
        /// Function to get letter grade
        /// </summary>
        /// <param name="gradePoints"></param>
        /// <returns></returns>
        public String getLetterGrade(double gradePoints)
        {
            String letterGrade = "";
            if (gradePoints >= 90.0) letterGrade = "A";        
            else if (gradePoints >= 80.0 && gradePoints < 90.0) letterGrade = "B"; 
            else if (gradePoints >= 70.0 && gradePoints < 80.0) letterGrade = "C"; 
            else if (gradePoints >= 60.0 && gradePoints < 70.0) letterGrade = "D"; 
            else if (gradePoints < 60.0) letterGrade = "F"; 
            return letterGrade;
        }

    }
}
