using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment1.Models;
using System.Globalization;

namespace Assignment1.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View("Calendar");
        }

        /// <summary>
        /// Returns a json result of assignment events
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAssignmentEvents()
        {
            using (LMS_GRINDEntities1 gds = new LMS_GRINDEntities1())
            {
                JsonResult jResult;

                if (Name.role == "Instructor")
                {
                    var events = (from a in gds.Assignments
                                  join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                                  join c in gds.Courses on ic.course_id equals c.course_id
                                  where ic.instructor_id == Name.user_id
                                  select new
                                  {
                                      title = a.assignment_name + " - " + c.course_num,
                                      description = c.course_num,
                                      start = a.due_date,
                                      assign_id = a.assignment_id,
                                      url = "Grading/InstructorGrading/" + a.assignment_id.ToString()
                                  }).ToList();

                    jResult = new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else // - NAME.ROLE == STUDENT 
                {
                    var events = (from a in gds.Assignments
                                  join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                                  join sc in gds.StudentCourses on ic.course_id equals sc.course_id
                                  join c in gds.Courses on ic.course_id equals c.course_id
                                  where sc.student_id == Name.user_id
                                  select new
                                  {
                                      title = a.assignment_name + " - " + c.course_num,
                                      description = c.course_num,
                                      start = a.due_date,
                                      assign_id = a.assignment_id,
                                      url = "assignment/StudentAssignment/" + a.assignment_id.ToString()
                                  }).ToList();

                    jResult = new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                return jResult;
            }
        }

        /// <summary>
        /// Returns a json result of course meeting events
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMeetingEvents()
        {
            using (LMS_GRINDEntities1 gds = new LMS_GRINDEntities1())
            {
                JsonResult jResult;
                List<Meeting> m;

                if (Name.role == "Instructor")
                {

                    m = new List<Meeting>();
                    int i = 0;
                    foreach (var e in CourseCardList.CourseList)
                    {
                        // copy event into a list of Meeting objects
                        m.Add(new Meeting());
                        m[i].days = GetDaysOfWeek(e.CourseId);
                        m[i].title = e.StartTime + " " + e.CourseNum + " - " + e.Building + ", Room: " + e.RoomNumber;
                        m[i].start = e.Start;
                        m[i].end = e.End;
                        i++;
                    }
                }
                else // - NAME.ROLE == STUDENT 
                {
                    m = new List<Meeting>();
                    int i = 0;
                    foreach (var e in CourseCardList.CourseList)
                    {
                        // copy event into a list of Meeting objects
                        m.Add(new Meeting());
                        m[i].days = GetDaysOfWeek(e.CourseId);
                        m[i].title = e.StartTime + " " + e.CourseNum + " - " + e.Building + ", Room: " + e.RoomNumber;
                        m[i].start = e.Start;
                        m[i].end = e.End;
                        i++;
                    }
                }

                jResult = new JsonResult { Data = m, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return jResult;
            }
        }

        /// <summary>
        /// Returns an string of numerical days of the week
        /// Ex: MTW --> "[1,3,5]"
        /// for a given course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDaysOfWeek(int id)
        {
            string s;
            using (LMS_GRINDEntities1 gds = new LMS_GRINDEntities1())
            {
                var d = (from c in gds.Courses
                         where c.course_id == id
                         select new { days = c.days_of_week }).First();

                s = "[";
                int i = 0;
                foreach (char c in d.days)
                {
                    switch (c)
                    {
                        case 'M':
                            s += "1";
                            break;
                        case 'T':
                            s += "2";
                            break;
                        case 'W':
                            s += "3";
                            break;
                        case 'R':
                            s += "4";
                            break;
                        case 'F':
                            s += "5";
                            break;
                        default:
                            break;
                    }

                    i++;
                    // add comma, else add closing bracket
                    if (!(s.Length - 2 == d.days.Length))
                    {
                        s += ",";
                    }
                }

                s += "]";
            }

            return s;
        }


    }
}