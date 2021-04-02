using Assignment1;
using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    public class AssignmentController : Controller
    {
        // GET: Assignment
        LMS_GRINDEntities1 gds;

        public ActionResult CourseAssignmentList()
        {
            return View("InstructorCourseAssignmentView");
        }

        public ActionResult InstructorAssignment(int id)
        {
            gds = new LMS_GRINDEntities1();
            var courseNum = gds.Courses.Where(x => x.course_id == id).Select(x => x.course_num).FirstOrDefault();
            var courseName = gds.Courses.Where(x => x.course_id == id).Select(x => x.course_name).FirstOrDefault();
            CourseCardList.GenerateInstructorCourseList();
            AssignmentList.GenerateInstructorAssignmentList(id);
            @ViewBag.CourseId = id;
            @ViewBag.CourseNum = courseNum;
            @ViewBag.CourseName = courseName;
            return View("InstructorAssignmentView");
        }

        /// <summary>
        /// Display assignment List for student course view based on particular course selected
        /// </summary>
        /// <returns>StudentAssignmentView</returns>    
        public ActionResult StudentAssignment(int? id)
        {
            AssignmentList.GenerateStudentSubmissionAssignmentItem(id);
            AssignmentList.GenerateSingleAssignmentItem(id);

            if (AssignmentList.StudentAssignmentSubmission.FileSubmission == null &&
                AssignmentList.StudentAssignmentSubmission.TextSubmission == null)
            {
                ViewBag.IsSubmitted = false;
            }
            else
            {
                ViewBag.IsSubmitted = true;
            }

            return View("StudentAssignmentView");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitAssignment(string text_submission, HttpPostedFileBase assignment_file)
        {
            gds = new LMS_GRINDEntities1();
            StudentAssignment sa = new StudentAssignment();

            if (AssignmentList.AssignmentItem.SubmissionType == "File")
            {
                if (assignment_file != null)
                {

                    var instructor = (from ic in gds.InstructorCourses
                                        where ic.instructor_course_id == AssignmentList.AssignmentItem.InstructorCourseId
                                        select ic).First();

                    var instructor_l_name = (from i in gds.ulUsers
                                             where i.ulUser_id == instructor.instructor_id
                                             select i).First();

                    string course_directory = instructor_l_name.last_name + "_" + AssignmentList.AssignmentItem.CourseNum;
                    string student_directory = Name.first_name + Name.last_name;
                    string file_name = Path.GetFileNameWithoutExtension(assignment_file.FileName) +
                    DateTime.Now.ToString("yymmssfff") +
                    Path.GetExtension(assignment_file.FileName);

                    // Set file path to the course directory within the Assignment Submissions folder
                    string file_path = Path.Combine(Server.MapPath("~/AssignmentSubmissions/"), course_directory);

                    // Create the course directory if it doesn't exist
                    if (!Directory.Exists(file_path))
                    {
                        Directory.CreateDirectory(file_path);
                    }

                    // Set file path to the student directory within the course folder
                    file_path = Path.Combine(Server.MapPath("~/AssignmentSubmissions/" + course_directory + "/"), student_directory);

                    // Create student directory if it doesn't exist
                    if (!Directory.Exists(file_path))
                    {
                        Directory.CreateDirectory(file_path);
                    }

                    // append filename to file path
                    file_path = Path.Combine(Server.MapPath("~/AssignmentSubmissions/" + course_directory + "/" + student_directory + "/"), file_name);

                    sa.file_submission = file_path;
                    sa.assignment_id = AssignmentList.AssignmentItem.AssignmentId;
                    sa.student_id = Name.user_id;
                    sa.submission_date = DateTime.Now;
                    assignment_file.SaveAs(file_path);

                    gds.StudentAssignments.Add(sa);
                    gds.SaveChanges();

                    ViewBag.IsSubmitted = true;
                }
                else
                {
                    ViewBag.IsSubmitted = false;
                }
            }
            else // TEXT SUBMISSION
            {
                sa.text_submission = text_submission;
                sa.assignment_id = AssignmentList.AssignmentItem.AssignmentId;
                sa.student_id = Name.user_id;
                sa.submission_date = DateTime.Now;

                gds.StudentAssignments.Add(sa);
                gds.SaveChanges();
                ViewBag.IsSubmitted = true;
            }

            ToDoList.GenerateStudentToDoList();
            return View("StudentAssignmentView");
        }

        public ActionResult AddAssignment(int id)
        {
            List<Cours> displayCourseList = GetCourseList();   // Display instructor's course list
            gds = new LMS_GRINDEntities1();
            var courseNum = gds.Courses.Where(x => x.course_id == id).Select(x => x.course_num).FirstOrDefault();
            var courseName = gds.Courses.Where(x => x.course_id == id).Select(x => x.course_name).FirstOrDefault();
            var courseId = gds.Courses.Where(x => x.course_id == id).Select(x => x.course_id).FirstOrDefault();
            @ViewBag.courseList = displayCourseList;
            @ViewBag.CourseNum = courseNum;
            @ViewBag.CourseName = courseName;
            @ViewBag.CourseId = courseId;
            ToDoList.GenerateInstructorToDoList();

            return View("AddAssignmentView");
        }

        [HttpPost]
        public ActionResult SaveAssignment(string assignmentName,
            string assignmentDesc, int maxPoints, DateTime dueDate, string assignmentType, string submissionType)
        {
            var courseId = int.Parse(Request.Form["CourseId"]);
            var courseNum = Request.Form["CourseNum"];
            var courseName = Request.Form["CourseName"];

           
            gds = new LMS_GRINDEntities1();
            Assignment newAssignment = new Assignment();

            var query = from course in gds.Courses
                        join insCourse in gds.InstructorCourses
                        on course.course_id equals insCourse.course_id
                        where course.course_id == courseId
                        select new
                        {
                            instructorCourseID = insCourse.instructor_course_id
                        };

            foreach (var insCourse in query)
            {
                newAssignment.instructor_course_id = insCourse.instructorCourseID;
            }

            newAssignment.assignment_name = assignmentName;
            newAssignment.assignment_desc = assignmentDesc;
            newAssignment.max_points = maxPoints;
            newAssignment.due_date = dueDate;
            newAssignment.assignment_type = assignmentType;
            newAssignment.submission_type = submissionType;
            gds.Assignments.Add(newAssignment);
            gds.SaveChanges();


            AssignmentList.GenerateInstructorAssignmentList(courseId);
            CourseCardList.GenerateInstructorCourseList();
            //List<Cours> displayCourseList = GetCourseList();   // Display instructor's course list
            //@ViewBag.courseList = displayCourseList;
            @ViewBag.CourseNum = courseNum;
            @ViewBag.CourseName = courseName;
            @ViewBag.CourseId = courseId;

            ToDoList.GenerateInstructorToDoList();

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
            List<InstructorCours> instructorCourses = gds.InstructorCourses.Where(x => x.instructor_id == Name.user_id).ToList();

            foreach (var course in instructorCourses)
            {
                Cours insCourse = gds.Courses.Where(x => x.course_id == course.course_id).FirstOrDefault();
                displayCourseList.Add(insCourse);
            }

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
        
        
        public ActionResult DeleteAssignment(int id)
        {
            gds = new LMS_GRINDEntities1();
            Assignment assignment = gds.Assignments.Where(x => x.assignment_id == id).FirstOrDefault();
            int insCourse = gds.Assignments.Where(x => x.assignment_id == id).Select(x => x.instructor_course_id).FirstOrDefault();
            int courseId = gds.InstructorCourses.Where(x => x.instructor_course_id == insCourse).Select(x => x.course_id).FirstOrDefault();
            var courseNum = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_num).FirstOrDefault();
            var courseName = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_name).FirstOrDefault();
            gds.Assignments.Remove(assignment);
            gds.SaveChanges();

            //Delete from assignments table, and student assignments table
            //TODO: implement delete from studentAssignments table'
            ViewBag.CourseNum = courseNum;
            ViewBag.CourseName = courseName;
            ViewBag.CourseId = courseId;
            AssignmentList.GenerateInstructorAssignmentList(courseId);
            CourseCardList.GenerateInstructorCourseList();
            return View("InstructorAssignmentView");
        }

        [HttpPost]
        public ActionResult UpdateAssignment(int id, string assignmentName, string assignmentDesc, int maxPoints, 
            DateTime dueDate, string assignmentType, string submissionType)
        {
            gds = new LMS_GRINDEntities1();

            Assignment assignment = gds.Assignments.Where(x => x.assignment_id == id).FirstOrDefault();
            int insCourse = gds.Assignments.Where(x => x.assignment_id == id).Select(x => x.instructor_course_id).FirstOrDefault();
            int courseId = gds.InstructorCourses.Where(x => x.instructor_course_id == insCourse).Select(x => x.course_id).FirstOrDefault();
            try
            {
                assignment.assignment_name = assignmentName;
                assignment.assignment_desc = assignmentDesc;
                assignment.max_points = maxPoints;
                assignment.due_date = dueDate;
                assignment.assignment_type = assignmentType;
                assignment.submission_type = submissionType;
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
                return View("EditAssignmentView");
            }

            AssignmentList.GenerateInstructorAssignmentList(courseId);
            return View("InstructorAssignmentView");
        }

        public ActionResult EditAssignment(int id)
        {
            //var id = int.Parse(Request.Form["CourseId"]);
            //var courseNum = Request.Form["CourseNum"];
            //var courseName = Request.Form["CourseName"];
            gds = new LMS_GRINDEntities1();
            Assignment assignment = gds.Assignments.Where(x => x.assignment_id == id).FirstOrDefault(); 
            int insCourse = gds.Assignments.Where(x => x.assignment_id == id).Select(x => x.instructor_course_id).FirstOrDefault();
            int courseId = gds.InstructorCourses.Where(x => x.instructor_course_id == insCourse).Select(x => x.course_id).FirstOrDefault();
            var courseNum = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_num).FirstOrDefault();
            var courseName = gds.Courses.Where(x => x.course_id == courseId).Select(x => x.course_name).FirstOrDefault();
            ViewBag.selectedAssignment = assignment;

            bool[] assignmentType = new bool[8];
            bool[] submissionType = new bool[2];

            for (int i = 0; i < 8; i++)
            {
                switch (assignment.assignment_type)
                {
                    case "Quiz":
                        assignmentType[0] = true;
                        break;
                    case "Test":
                        assignmentType[1] = true;
                        break;
                    case "Project":
                        assignmentType[2] = true;
                        break;
                    case "Homework":
                        assignmentType[3] = true;
                        break;
                    case "Participation":
                        assignmentType[4] = true;
                        break;
                    case "Presentation":
                        assignmentType[5] = true;
                        break;
                    case "Discussion":
                        assignmentType[6] = true;
                        break;
                    default:
                        assignmentType[7] = false;
                        break;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                switch (assignment.submission_type)
                {
                    case "File":
                        submissionType[0] = true;
                        break;
                    case "Text":
                        submissionType[1] = true;
                        break;
                    default:
                        submissionType[2] = false;
                        break;
                }
            }

            ViewBag.submissionType = submissionType;
            ViewBag.assignmentType = assignmentType;
            ViewBag.CourseNum = courseNum;
            ViewBag.CourseName = courseName;
            ViewBag.CourseId = courseId;
            //DateTime dueDate = DateTime.Parse(assignment.due_date);
            ViewBag.DueDate = assignment.due_date;
            AssignmentList.GenerateInstructorAssignmentList(id);
            return View("EditAssignmentView");
        }
    }
}