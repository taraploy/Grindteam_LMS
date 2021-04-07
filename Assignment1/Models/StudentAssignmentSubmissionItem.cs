using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class StudentAssignmentSubmissionItem
    {
        public int AssignmentGradeId;
        public int AssignmentId;
        public int StudentId;
        public string FirstName;
        public string LastName;
        public Nullable<int> Grade;
        public DateTime SubmissionDate;
        public string TextSubmission;
        public string FileSubmission;
        public bool isGraded;
        public int MaxPoints;
        public DateTime DueDate;
        public string InstructorFeedback;
    }
}