using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Assignment1;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using Assignment1.Controllers;

namespace Assignment1Test2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InstructorCanCreateCourseTest()
        {
            LMS_GRINDEntities gds = new LMS_GRINDEntities();

            // Preparation
            // Start with the known instructor id then find out how many courses this instructor is teaching right now
            var courses = (from x in gds.Courses
                           join ic in gds.InstructorCourses on x.course_id equals ic.course_id
                           where ic.instructor_id == 1012
                           select x).ToList();

            // Call that course number N
            int N = courses.Count();

            // Create a new course for this instructor using the application code from test
            Cours c = new Cours();
            c.course_num = "MATH 1010";
            c.course_name = "Intermediate Algebra";
            c.course_desc = "Prepare to be enlightened";
            c.building = "Math Hub";
            c.days_of_week = "MTWF";
            c.dept_id = 5;
            c.start_time = new TimeSpan(9, 30, 0);
            c.end_time = new TimeSpan(10, 20, 0);
            c.num_credits = 3;
            c.max_capacity = 30;
            c.room_no = 112;

            CourseRegistration controller = new CourseRegistration();
            controller.SaveCourse2(c.course_num, c.course_name, c.course_desc, (int)c.num_credits,
                (int)c.max_capacity, c.building, (int)c.room_no, c.dept_id, "M", "T", "W", "", "F",
                (TimeSpan)c.start_time, (TimeSpan)c.end_time, 1012);

            // Again, Find out how many courses this instructor is teaching now
            var courses2 = (from x in gds.Courses
                            join ic in gds.InstructorCourses on x.course_id equals ic.course_id
                            where ic.instructor_id == 1012
                            select x).ToList();

            int N2 = courses2.Count();
            int expected = N + 1;

            // Expecting N + 1
            // (Started with N courses and added 1 course)
            Assert.AreEqual(N2, expected);


        }
    }
}
