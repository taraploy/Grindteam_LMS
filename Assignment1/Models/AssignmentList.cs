using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public static class AssignmentList
    {

        public static List<AssignmentItem> AssignmentItemList;
        public static List<StudentAssignmentItem> StudentAssignments;

        public static void GenerateInstructorAssignmentList(int? id)
        {
            AssignmentItemList = new List<AssignmentItem>();
            LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();
            var query = (from a in gds.Assignments
                        join ic in gds.InstructorCourses on a.instructor_course_id equals ic.instructor_course_id
                        where ic.instructor_id == Name.user_id && ic.course_id == id
                        select new
                        {
                            AssignmentId = a.assignment_id,
                            InstructorCourseId = a.instructor_course_id,
                            AssignmentName = a.assignment_name,
                            AssignmentDesc = a.assignment_desc,
                            AssignmentType = a.assignment_type,
                            MaxPoints = a.max_points,
                            DueDate = a.due_date

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

                i++;
            }
    }


        public static void GenerateStudentAssignmentList()
        {
            StudentAssignments = new List<StudentAssignmentItem>();
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
                StudentAssignments.Add(new StudentAssignmentItem());
                StudentAssignments[i].AssignmentGradeId = item.AssignmentGradeId;
                StudentAssignments[i].AssignmentId = item.AssignmentId;
                StudentAssignments[i].StudentId = item.StudentId;
                StudentAssignments[i].Grade = item.Grade;
                StudentAssignments[i].SubmissionDate = (DateTime)item.SubmissionDate;
                StudentAssignments[i].FileSubmission = item.FileSubmission;
                StudentAssignments[i].TextSubmission = item.TextSubmission;

                i++;
            }
        }
      

    }
}