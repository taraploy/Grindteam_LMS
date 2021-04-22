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
        public void InstructorCanCreateCourseTest()         //Gabby's Unit Test
        {
            LMS_GRINDEntities gds = new LMS_GRINDEntities();

            // Preparation
            // Start with the known instructor id then find out how many courses this instructor is teaching right now
            // Call it N
            int N = (from x in gds.Courses
                           join ic in gds.InstructorCourses on x.course_id equals ic.course_id
                           where ic.instructor_id == 1012
                           select x).Count();

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

            CourseRegistration cr = new CourseRegistration();
            cr.SaveCourse2(c.course_num, c.course_name, c.course_desc, (int)c.num_credits,
                (int)c.max_capacity, c.building, (int)c.room_no, c.dept_id, "M", "T", "W", "", "F",
                (TimeSpan)c.start_time, (TimeSpan)c.end_time, 1012);

            // Again, Find out how many courses this instructor is teaching now
            int N2 = (from x in gds.Courses
                            join ic in gds.InstructorCourses on x.course_id equals ic.course_id
                            where ic.instructor_id == 1012
                            select x).Count();

            int expected = N + 1;

            // get id of newly created course
            var id = (from x in gds.Courses
                      join ic in gds.InstructorCourses 
                      on x.course_id equals ic.course_id
                      where ic.instructor_id == 1012
                      where x.course_desc == "Prepare to be enlightened"
                      select x).First();

            // delete newly created course
            cr.DeleteCourse2(id.course_id);

            // Expecting N + 1
            // (Started with N courses and added 1 course)
            Assert.AreEqual(N2, expected);


        }

        [TestMethod]
        public void StudentCanRegisterTest()            //Delaney's Unit Test
        {
            LMS_GRINDEntities gds = new LMS_GRINDEntities();

            //Find all the registered courses the student currently has (ulUser_id = 2017)
            int registeredCourseCount = (from sc in gds.StudentCourses where sc.student_id == 2017 select sc.course_id).Count();

            //Register for a course
            int studentId = 2017;
            int courseId = 2026;

            CourseRegistration cr = new CourseRegistration();

            //Register for the course
            cr.RegisterForCourse2(studentId, courseId);

            //Count of registered courses for student should have increased by one
            int x = (from sc in gds.StudentCourses where sc.student_id == 2017 select sc.course_id).Count();
            int expected = registeredCourseCount + 1;

            //Delete the registration
            cr.DeleteRegistration2(studentId, courseId);
                        
            Assert.AreEqual(expected, x);
        }

        [TestMethod]
        public void InstructorCanCreateAssignmentTest()         //Ploy's Unit Test
        {
            LMS_GRINDEntities gds = new LMS_GRINDEntities();

            int instructor_course_id = 2026;
            int course_id = 2029;

            // Count how many assignments instructor already has
            int assignmentsCount1 = (from x in gds.Assignments
                                    where x.instructor_course_id == instructor_course_id
                                    select x).Count();

            // Create new assignment
            DateTime dueDateTime = new DateTime(2021, 4, 30);
            Assignment a = new Assignment();
            a.assignment_name = "Final Exam";
            a.assignment_desc = "Comprehensive Final";
            a.max_points = 300;
            a.due_date = dueDateTime;
            a.assignment_type = "Test";
            a.submission_type = "Text";

            CourseRegistration cr = new CourseRegistration();
            
            // Save assignment
            cr.SaveAssignment2(course_id, a.assignment_name, a.assignment_desc, (int)a.max_points, (DateTime)a.due_date, a.assignment_type, a.submission_type);

            // Count how many assignments instructor has after adding an assignment
            int assignmentsCount2 = (from x in gds.Assignments
                                    where x.instructor_course_id == instructor_course_id
                                     select x).Count();

            // Expecting result
            int expected = assignmentsCount1 + 1;

            // Deleting unit testing assignment
            var assignmentID = (from x in gds.Assignments
                                where x.instructor_course_id == instructor_course_id
                                where x.assignment_name == "Final Exam"
                                select x).First();

            cr.DeleteAssignment2(assignmentID.assignment_name);

            // Test
            Assert.AreEqual(assignmentsCount2, expected);

        }
    }
}
