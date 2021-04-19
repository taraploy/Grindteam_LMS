using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections;
using System.Linq;
using Assignment1;
using Assignment1.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace Assignment1
{
    public class CourseRegistration
    {
        public void SaveCourse2(string courseNum, string courseName, string courseDesc,
 int courseCredits, int maxCapacity, string courseLocation,
 int courseRoom, int departments, string monday, string tuesday, string wednesday,
 string thursday, string friday, TimeSpan startTime, TimeSpan endTime, int instructor_id)
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
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
            InstructorCourseContext.instructorId = instructor_id;
            insCourse.instructor_id = instructor_id;
            Course.instructorFullName = Name.first_name + " " + Name.last_name;
            Course.instructorLastName = Name.last_name;
            Course.instructorFirstName = Name.first_name;

            gds.InstructorCourses.Add(insCourse);
            gds.SaveChanges();

            CourseCardList.GenerateInstructorCourseList();

        }
    }
}