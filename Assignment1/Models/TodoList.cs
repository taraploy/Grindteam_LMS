using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public static class ToDoList
    {

        public static List<ToDo> StudentToDoList;
        public static List<ToDo> InstructorToDoList;

        public static void GenerateStudentToDoList()
        {
            StudentToDoList = new List<ToDo>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            // Get a list of top 5 assignments assigned to student
            // sorted by due date
            var query = (from a in gds.Assignments
                         join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join sc in gds.StudentCourses on ic.course_id equals sc.course_id
                         join c in gds.Courses on ic.course_id equals c.course_id
                         where sc.student_id == Name.user_id
                         where a.due_date > DateTime.Now
                         where !(from sa in gds.StudentAssignments
                                 where sa.student_id == Name.user_id
                                 select sa.assignment_id).Contains(a.assignment_id)
                         orderby a.due_date ascending
                         select new
                         {
                             CourseId = c.course_id,
                             CourseNum = c.course_num,
                             AssignmentName = a.assignment_name,
                             DueDate = a.due_date,
                             AssignmentID = a.assignment_id
                         }).Take(5).ToList();

            int i = 0;
            foreach (var item in query)
            {
                StudentToDoList.Add(new ToDo());
                StudentToDoList[i].AssignmentName = item.AssignmentName;
                StudentToDoList[i].CourseNum = item.CourseNum;
                StudentToDoList[i].DueDate = item.DueDate;
                StudentToDoList[i].AssignmentID = item.AssignmentID;
                //LetterGradeList.GenerateCourseLetterGrades(item.CourseId);
                i++;
            }

        }

        public static void GenerateInstructorToDoList()
        {
            InstructorToDoList = new List<ToDo>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            // Get a list of top 5 assignments assigned to student
            // sorted by due date
            var query = (from a in gds.Assignments
                         join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join c in gds.Courses on ic.course_id equals c.course_id
                         where ic.instructor_id == Name.user_id
                         where a.due_date > DateTime.Now
                         orderby a.due_date ascending
                         select new
                         {
                             CourseId = c.course_id,
                             CourseNum = c.course_num,
                             AssignmentName = a.assignment_name,
                             DueDate = a.due_date,
                             AssignmentID = a.assignment_id
                         }).Take(5).ToList();

            int i = 0;
            foreach (var item in query)
            {
                InstructorToDoList.Add(new ToDo());
                InstructorToDoList[i].AssignmentName = item.AssignmentName;
                InstructorToDoList[i].CourseNum = item.CourseNum;
                InstructorToDoList[i].DueDate = item.DueDate;
                InstructorToDoList[i].AssignmentID = item.AssignmentID;
                //LetterGradeList.GenerateCourseLetterGrades(item.CourseId);
                i++;
            }
        }

    }
}