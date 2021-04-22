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

        public void DeleteCourse2(int id)
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

            Cours course = gds.Courses.Where(x => x.course_id == id).FirstOrDefault();
            InstructorCours insCourse = gds.InstructorCourses.Where(x => x.course_id == id).FirstOrDefault();
            //StudentCours[] enrolledStudents = new StudentCours[32];
            var count = gds.StudentCourses.Where(x => x.course_id == id).Count();
            for (int i = 0; i < count; i++)
            {
                //remove existing enrollments to the class
                StudentCours stdCourse = gds.StudentCourses.Where(x => x.course_id == id).FirstOrDefault();
                gds.StudentCourses.Remove(stdCourse);
                gds.SaveChanges();
            }

            gds.InstructorCourses.Remove(insCourse);
            gds.Courses.Remove(course);
            gds.SaveChanges();
        }

        public void RegisterForCourse2(int studentId, int courseId)
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            Cours course = gds.Courses.Where(x => x.course_id == courseId).FirstOrDefault();
            StudentCours stCourse = new StudentCours();
            stCourse.course_id = courseId;
            stCourse.student_id = studentId;
            stCourse.letter_grade = null;
            gds.StudentCourses.Add(stCourse);
            gds.SaveChanges();
        }

        public void DeleteRegistration2(int studentId, int courseId)
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            Cours course = gds.Courses.Where(x => x.course_id == courseId).FirstOrDefault();
            StudentCours stCourse = gds.StudentCourses.Where(x => (x.course_id == courseId) && (x.student_id == studentId)).FirstOrDefault();

            gds.StudentCourses.Remove(stCourse);
            gds.SaveChanges();
        }

        public void SaveAssignment2(int course_id, string assignemnt_name, string assignment_desc, int max_points, DateTime due_date, string assignment_type, string submission_type)
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            Assignment newAssignment = new Assignment();

            var query = from course in gds.Courses
                        join insCourse in gds.InstructorCourses
                        on course.course_id equals insCourse.course_id
                        where course.course_id == course_id
                        select new
                        {
                            instructorCourseID = insCourse.instructor_course_id
                        };

            foreach (var insCourse in query)
            {
                newAssignment.instructor_course_id = insCourse.instructorCourseID;
            }

            newAssignment.assignment_name = assignemnt_name;
            newAssignment.assignment_desc = assignment_desc;
            newAssignment.max_points = max_points;
            newAssignment.due_date = due_date;
            newAssignment.assignment_type = assignment_type;
            newAssignment.submission_type = submission_type;
            gds.Assignments.Add(newAssignment);
            gds.SaveChanges();
        }

        public void DeleteAssignment2(string assignment_name)
        {
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            Assignment assignment = gds.Assignments.Where(x => x.assignment_name == assignment_name).FirstOrDefault();
            gds.Assignments.Remove(assignment);
            gds.SaveChanges();
        }
    }
}