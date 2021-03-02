using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class StudentCourseContext
    {
        public int studentCourseId { get; set; }
        public int courseId { get; set; }
        public int studentId { get; set; }
        public Grade? letterGrade { get; set; }

        public virtual Cours Course { get; set; }
        public virtual ulUser User { get; set; }
    }
}