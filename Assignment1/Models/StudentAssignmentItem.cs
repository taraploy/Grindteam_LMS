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
        public Nullable<int> Grade;
        public DateTime SubmissionDate;
        public string TextSubmission;
        public string FileSubmission;

    }
}