using Assignment1;
using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    public class AssignmentController : Controller
    {
        // GET: Assignment
        LMS_GRINDEntities1 gds;

        public ActionResult InstructorAssignment()
        {
            CourseCardList.GenerateInstructorCourseList();
            AssignmentList.GenerateInstructorAssignmentList();
            return View("InstructorAssignmentView");
        }

        public ActionResult StudentAssignment()
        {
            AssignmentList.GenerateStudentAssignmentList();
            return View();
        }
        
        public ActionResult AddAssignment()
        {
           // InstructorCours ic = new InstructorCours();
            List<Cours> displayCourseList = GetCourseList();   // Display instructor's course list
            gds = new LMS_GRINDEntities1();
            @ViewBag.courseList = displayCourseList;

            return View("AddAssignmentView");
        }

        [HttpPost]
        public ActionResult SaveAssignment(string courseNum, string assignmentName, 
            string assignmentDesc, int maxPoints, DateTime dueDate, string assignmentType)
        {
            gds = new LMS_GRINDEntities1();
            int courseNumber = gds.Courses.Where(x => x.course_num == courseNum).Select(x => x.course_id).FirstOrDefault();
            int insCourseId = gds.InstructorCourses.Where(x => x.instructor_course_id == courseNumber).Select(x => x.instructor_course_id).FirstOrDefault();
            int courseId = gds.Assignments.Where(x => x.instructor_course_id == insCourseId).Select(x => x.instructor_course_id).FirstOrDefault();

            Assignment newAssignment = new Assignment();
            newAssignment.assignment_name = assignmentName;
            newAssignment.assignment_desc = assignmentDesc;

            //FIX course id saving correct value in database
            newAssignment.instructor_course_id = courseId;
            
            
            newAssignment.max_points = maxPoints;
            newAssignment.due_date = dueDate;
            
            //FIX assignment type
            newAssignment.assignment_type = assignmentType;

            gds.Assignments.Add(newAssignment);
            gds.SaveChanges();

            AssignmentList.GenerateInstructorAssignmentList();
            return View("InstructorAssignmentView");
        }

        /// <summary>
        /// Display course List for dropdown menu for instructor add assignment
        /// </summary>
        /// <returns></returns>        
        public List<Cours> GetCourseList()
        {
            List<Cours> displayCourseList = new List<Cours>();
            gds = new LMS_GRINDEntities1();
            //var courseList = gds.InstructorCourses.Join(gds.Courses, insCourse => insCourse.course_id, course => course.course_id).ToList();
            List<InstructorCours> instructorCourses = gds.InstructorCourses.Where(x => x.instructor_id == Name.user_id).ToList();

            foreach (var course in instructorCourses)
            {
                Cours insCourse = gds.Courses.Where(x => x.course_id == course.course_id).FirstOrDefault();
                displayCourseList.Add(insCourse);
            }

            //List<Cours> courseList = from i in gds.InstructorCourses.Where(x => x.course_id.Contains(instructorCourses)).ToList();
            //displayCourseList = courseList;
            //foreach (var course in courseList)
            //{
            //    if (course.courseNum != null)
            //    {
            //        //displayCourseList.Add(course.courseNum.ToString() + " " + course.courseName);
            //        displayCourseList.Add(course);
            //    }
            //}

            return displayCourseList;
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
    }
}