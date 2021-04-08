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

        public ActionResult GoToInstructorGrading(int id)
        {
            return InstructorGrading(id);
        }

        public ActionResult InstructorGrading(int? assignmentId)
        {
            gds = new LMS_GRINDEntities1();
            Assignment thisAssignment = gds.Assignments.Where(x => x.assignment_id == assignmentId).FirstOrDefault();
            int assignId = gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.assignment_id).FirstOrDefault();
            int instructorCourseId = gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.instructor_course_id).FirstOrDefault();
            int courseId = gds.InstructorCourses.Where(x => x.instructor_course_id == instructorCourseId).Select(x => x.course_id).FirstOrDefault();
            string courseNum = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_num).FirstOrDefault();
            string courseName = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_name).FirstOrDefault();
            Cours course = gds.Courses.Where(x => x.course_id == courseId).FirstOrDefault();
            int ic_id = gds.InstructorCourses.Where(x => x.course_id == courseId).Select(x => x.instructor_course_id).FirstOrDefault();
            int instructorId = gds.InstructorCourses.Where(x => x.instructor_course_id == ic_id).Select(x => x.instructor_id).FirstOrDefault();
            String instructorLastName = gds.ulUsers.Where(x => x.ulUser_id == instructorId).Select(x => x.last_name).FirstOrDefault();
            ViewBag.AssignmentName = thisAssignment.assignment_name;
            ViewBag.AssignmentId = thisAssignment.assignment_id;
            ViewBag.CourseNum = courseNum;
            ViewBag.CourseName = courseName;
            ViewBag.AssignmentDueDate = thisAssignment.due_date;
            ViewBag.MaxPoints = thisAssignment.max_points;
            ViewBag.SubmissionType = thisAssignment.submission_type;
            AssignmentList.GenerateAllSubmissions(assignmentId);
            return View("InstructorGradingView");
        }

        /// <summary>
        /// Instructor grade assignment individually
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        public ActionResult GradeAssignment(int assignmentGradeId)
        {
            gds = new LMS_GRINDEntities1();
            //Populate the page with individual assignment information
            StudentAssignment stuAssignment = gds.StudentAssignments.Where(x => x.assignment_grade_id == assignmentGradeId).FirstOrDefault();
            int student_id = gds.StudentAssignments.Where(x => x.assignment_grade_id == assignmentGradeId).Select(x => x.student_id).FirstOrDefault();
            int assignmentId = gds.StudentAssignments.Where(x => x.assignment_grade_id == assignmentGradeId).Select(x => x.assignment_id).FirstOrDefault();
            Assignment thisAssignment = gds.Assignments.Where(x => x.assignment_id == assignmentId).FirstOrDefault();
            String assignmentName = gds.Assignments.Where(x => x.assignment_id == assignmentId).Select(x => x.assignment_name).FirstOrDefault();
            String firstName = gds.ulUsers.Where(x => x.ulUser_id == student_id).Select(x => x.first_name).FirstOrDefault();
            String lastName = gds.ulUsers.Where(x => x.ulUser_id == student_id).Select(x => x.last_name).FirstOrDefault();
            ViewBag.AssignmentName = assignmentName;
            ViewBag.StudentName = firstName + " " + lastName;
            ViewBag.StudentId = stuAssignment.student_id;
            ViewBag.SubmissionDate = stuAssignment.submission_date;
            ViewBag.TextSubmission = stuAssignment.text_submission;
            ViewBag.FileSubmission = stuAssignment.file_submission;
            ViewBag.DueDate = thisAssignment.due_date;
            ViewBag.MaxPoints = thisAssignment.max_points;
            ViewBag.AssignmentGradeId = stuAssignment.assignment_grade_id;
            ViewBag.AssignmentId = assignmentId;
            ViewBag.CurrentGrade = stuAssignment.grade;
            ViewBag.Feedback = stuAssignment.instructor_feedback;
            return View("GradeAssignmentView");
        }

        [HttpPost]
        public ActionResult SubmitGradeAssignment(int assignmentGradeId, int grade, string instructorFeedback)
        {
            gds = new LMS_GRINDEntities1();
            //Save grade to database
            StudentAssignment stuAssignment = gds.StudentAssignments.Where(x => x.assignment_grade_id == assignmentGradeId).FirstOrDefault();
            int? id = gds.StudentAssignments.Where(x => x.assignment_grade_id == assignmentGradeId).Select(x => x.assignment_id).FirstOrDefault();
            try
            {
                stuAssignment.grade = grade;
                stuAssignment.instructor_feedback = instructorFeedback;
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
            catch
            {
                return View("GradeAssignmentView");
            }
            return RedirectToAction("InstructorGrading", new { assignmentId = id });
        }
    }
}