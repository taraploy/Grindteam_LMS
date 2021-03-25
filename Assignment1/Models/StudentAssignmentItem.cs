using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class StudentAssignmentItem
    {
        public int AssignmentGradeId;
        public int AssignmentId;
        public int StudentId;
        public string AssignmentDesc;
        public int InstructorCourseId;
        public string AssignmentName;
        public string AssignmentType;
        public int? MaxPoints;
        public System.DateTime? DueDate;
        public int CourseId;
        public string CourseNum;
        public string CourseName;
    }
}