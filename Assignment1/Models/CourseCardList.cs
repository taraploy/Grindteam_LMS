using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1.Models
{
    /// <summary>
    /// For dynamic course card funcitonality
    /// </summary>
    public static class CourseCardList
    {
        /// <summary>
        /// Static List of CourseCard Objects
        /// </summary>
        public static List<CourseCard> CourseList;


        public static void GenerateStudentCourseList()
        {
            CourseList = new List<CourseCard>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            string sStart;
            string sEnd;
            DateTime dtStartTime;
            DateTime dtEndTime;
            int i = 0;

            //Select all course IDs associated with logged in user
            var query = (from c in gds.Courses
                         join ic in gds.InstructorCourses
                         on c.course_id equals ic.course_id
                         join u in gds.ulUsers
                         on ic.instructor_id equals u.ulUser_id
                         join cid in gds.StudentCourses
                         on c.course_id equals cid.course_id
                         where cid.student_id == Name.user_id
                         select new
                         {
                             CourseName = c.course_name,
                             CourseNum = c.course_num,
                             InstructorFirstName = u.first_name,
                             InstructorLastName = u.last_name,
                             Building = c.building,
                             RoomNumber = c.room_no,
                             StartTime = c.start_time,
                             EndTime = c.end_time,
                             Days = c.days_of_week

                         }).ToList();

            foreach (var item in query)
            {
                // convert time format
                dtStartTime = DateTime.Today.Add((TimeSpan)item.StartTime);
                sStart = dtStartTime.ToString("hh:mm tt");
                dtEndTime = DateTime.Today.Add((TimeSpan)item.EndTime);
                sEnd = dtEndTime.ToString("hh:mm tt");

                CourseList.Add(new CourseCard());
                CourseList[i].CourseName = item.CourseName;
                CourseList[i].CourseNum = item.CourseNum;
                CourseList[i].InstructorFirstName = item.InstructorFirstName;
                CourseList[i].InstructorLastName = item.InstructorLastName;
                CourseList[i].Building = item.Building;
                CourseList[i].RoomNumber = (int)item.RoomNumber;
                CourseList[i].StartTime = sStart;
                CourseList[i].EndTime = sEnd;
                CourseList[i].Days = item.Days;

                i++;
            }
        }

        public static void GenerateInstructorCourseList()
        {
            CourseList = new List<CourseCard>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            string sStart;
            string sEnd;
            DateTime dtStartTime;
            DateTime dtEndTime;
            int i = 0;

            //Select all course IDs associated with logged in user
            var query = (from c in gds.Courses
                         join ic in gds.InstructorCourses
                         on c.course_id equals ic.course_id
                         join u in gds.ulUsers
                         on ic.instructor_id equals u.ulUser_id
                         join iid in gds.InstructorCourses
                         on c.course_id equals iid.course_id
                         where iid.instructor_id == Name.user_id
                         select new
                         {
                             CourseName = c.course_name,
                             CourseNum = c.course_num,
                             InstructorFirstName = u.first_name,
                             InstructorLastName = u.last_name,
                             Building = c.building,
                             RoomNumber = c.room_no,
                             StartTime = c.start_time,
                             EndTime = c.end_time,
                             Days = c.days_of_week

                         }).ToList();

            foreach (var item in query)
            {
                // convert time format
                dtStartTime = DateTime.Today.Add((TimeSpan)item.StartTime);
                sStart = dtStartTime.ToString("hh:mm tt");
                dtEndTime = DateTime.Today.Add((TimeSpan)item.EndTime);
                sEnd = dtEndTime.ToString("hh:mm tt");

                CourseList.Add(new CourseCard());
                CourseList[i].CourseName = item.CourseName;
                CourseList[i].CourseNum = item.CourseNum;
                CourseList[i].InstructorFirstName = item.InstructorFirstName;
                CourseList[i].InstructorLastName = item.InstructorLastName;
                CourseList[i].Building = item.Building;
                CourseList[i].RoomNumber = (int)item.RoomNumber;
                CourseList[i].StartTime = sStart;
                CourseList[i].EndTime = sEnd;
                CourseList[i].Days = item.Days;

                i++;
            }
        }
    }
}