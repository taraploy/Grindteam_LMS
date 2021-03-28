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
    public class GradingController : Controller
    {
        // GET: Grading

        LMS_GRINDEntities1 gds;

        public ActionResult InstructorGrading(int? assignmentId)
        {
            gds = new LMS_GRINDEntities1();
            String assignmentName = gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.assignment_name).FirstOrDefault();
            DateTime dueDate = (DateTime)gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.due_date).FirstOrDefault();
            int? MaxPoints = gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.max_points).FirstOrDefault();
            string submissionType = gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.submission_type).FirstOrDefault();
            int instructorCourseId = gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.instructor_course_id).FirstOrDefault();
            int courseId = gds.InstructorCourses.Where(x => x.instructor_course_id == instructorCourseId).Select(x => x.course_id).FirstOrDefault();
            string courseNum = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_num).FirstOrDefault();
            string courseName = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_name).FirstOrDefault();
            Cours course = gds.Courses.Where(x => x.course_id == courseId).FirstOrDefault();
            int ic_id = gds.InstructorCourses.Where(x => x.course_id == courseId).Select(x => x.instructor_course_id).FirstOrDefault();
            int instructorId = gds.InstructorCourses.Where(x => x.instructor_course_id == ic_id).Select(x => x.instructor_id).FirstOrDefault();
            String instructorLastName = gds.ulUsers.Where(x => x.ulUser_id == instructorId).Select(x => x.last_name).FirstOrDefault();
            ViewBag.AssignmentName = assignmentName;
            ViewBag.CourseNum = courseNum;
            ViewBag.CourseName = courseName;
            ViewBag.AssignmentDueDate = dueDate;
            ViewBag.MaxPoints = MaxPoints;
            ViewBag.SubmissionType = submissionType;
            AssignmentList.GenerateAllSubmissions(assignmentId);
            return View("InstructorGradingView");
        }

        /// <summary>
        /// Instructor grade assignment individually
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        public ActionResult GradeAssignment(int assignmentId, int studentId)
        {
            gds = new LMS_GRINDEntities1();
            //Save grade to database
            return View("GradeAssignmentView");
        }
    }
}