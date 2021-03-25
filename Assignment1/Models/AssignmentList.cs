using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public static class AssignmentList
    {

        public static List<AssignmentItem> AssignmentItemList;
        public static List<StudentAssignmentSubmissionItem> StudentAssignmentSubmissions;
        public static List<StudentAssignmentItem> StudentAssignments;

        public static void GenerateInstructorAssignmentList(int id)
        {
            AssignmentItemList = new List<AssignmentItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            var query = (from a in gds.Assignments
                        join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                        join c in gds.Courses on ic.course_id equals c.course_id
                        where ic.instructor_id == Name.user_id && ic.course_id == id
                        select new
                        {
                            AssignmentId = a.assignment_id,
                            InstructorCourseId = a.instructor_course_id,
                            AssignmentName = a.assignment_name,
                            AssignmentDesc = a.assignment_desc,
                            AssignmentType = a.assignment_type,
                            MaxPoints = a.max_points,
                            DueDate = a.due_date,
                            CourseId = c.course_id,
                            CourseNum = c.course_num,
                            CourseName = c.course_name
                        }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                AssignmentItemList.Add(new AssignmentItem());
                AssignmentItemList[i].AssignmentId = item.AssignmentId;
                AssignmentItemList[i].AssignmentDesc = item.AssignmentDesc;
                AssignmentItemList[i].InstructorCourseId = item.InstructorCourseId;
                AssignmentItemList[i].AssignmentName = item.AssignmentName;
                AssignmentItemList[i].AssignmentType = item.AssignmentType;
                AssignmentItemList[i].MaxPoints = item.MaxPoints;
                AssignmentItemList[i].DueDate = item.DueDate;
                AssignmentItemList[i].CourseId = item.CourseId;
                AssignmentItemList[i].CourseNum = item.CourseNum;
                AssignmentItemList[i].CourseName = item.CourseName;
                i++;
            }
        }
        public static void GenerateStudentAssignmentList(int id)
        {
            StudentAssignments = new List<StudentAssignmentItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            var query = (from a in gds.Assignments
                         join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                         join sc in gds.StudentCourses on ic.course_id equals sc.course_id 
                         join c in gds.Courses on ic.course_id equals c.course_id
                         where ic.course_id == id && sc.student_id == Name.user_id      //Name.user_id is the student, so find matching student_id
                         select new
                         {
                             AssignmentId = a.assignment_id,
                             InstructorCourseId = a.instructor_course_id,
                             AssignmentName = a.assignment_name,
                             AssignmentDesc = a.assignment_desc,
                             AssignmentType = a.assignment_type,
                             MaxPoints = a.max_points,
                             DueDate = a.due_date,
                             CourseId = c.course_id,
                             CourseNum = c.course_num,
                             CourseName = c.course_name,
                             StudentId = sc.student_id
                         }).ToList();

            int i = 0;
            foreach (var item in query)
            {
                StudentAssignments.Add(new StudentAssignmentItem());
                StudentAssignments[i].AssignmentId = item.AssignmentId;
                StudentAssignments[i].AssignmentDesc = item.AssignmentDesc;
                StudentAssignments[i].InstructorCourseId = item.InstructorCourseId;
                StudentAssignments[i].AssignmentName = item.AssignmentName;
                StudentAssignments[i].AssignmentType = item.AssignmentType;
                StudentAssignments[i].MaxPoints = item.MaxPoints;
                StudentAssignments[i].DueDate = item.DueDate;
                StudentAssignments[i].CourseId = item.CourseId;
                StudentAssignments[i].CourseNum = item.CourseNum;
                StudentAssignments[i].CourseName = item.CourseName;
                StudentAssignments[i].StudentId = item.StudentId;
                i++;
            }
        }

        public static void GenerateStudentSubmissionAssignmentList(int? assignmentId)
        {
            StudentAssignmentSubmissions = new List<StudentAssignmentSubmissionItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            var query = (from a in gds.Assignments
                         join s in gds.StudentAssignments
                         on a.assignment_id equals s.assignment_id
                         where s.student_id == Name.user_id
                         select new
                         {
                            AssignmentGradeId = s.assignment_grade_id,
                            AssignmentId = a.assignment_id,
                            StudentId = s.student_id,
                            Grade = s.grade,
                            SubmissionDate = s.submission_date,
                            TextSubmission = s.text_submission,
                            FileSubmission = s.file_submission
                         }).ToList();


            int i = 0;
            foreach (var item in query)
            {
                StudentAssignmentSubmissions.Add(new StudentAssignmentSubmissionItem());
                StudentAssignmentSubmissions[i].AssignmentGradeId = item.AssignmentGradeId;
                StudentAssignmentSubmissions[i].AssignmentId = item.AssignmentId;
                StudentAssignmentSubmissions[i].StudentId = item.StudentId;
                StudentAssignmentSubmissions[i].Grade = item.Grade;
                StudentAssignmentSubmissions[i].SubmissionDate = (DateTime)item.SubmissionDate;
                StudentAssignmentSubmissions[i].FileSubmission = item.FileSubmission;
                StudentAssignmentSubmissions[i].TextSubmission = item.TextSubmission;
                i++;
            }
        }
      

    }
}