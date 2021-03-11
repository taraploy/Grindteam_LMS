using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class AssignmentItem
    {
        public int AssignmentId;
        public int InstructorCourseId;
        public string AssignmentName;
        public string AssignmentDesc;
        public string AssignmentType;
        public Nullable<int> MaxPoints;
        public Nullable<System.DateTime> DueDate;
        public int CourseId;
        public string CourseName;
        public string CourseNum;
    }
}