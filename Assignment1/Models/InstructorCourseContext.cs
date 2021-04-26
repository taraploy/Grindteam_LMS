using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class InstructorCourseContext
    {
        public static int instructorCourseId { get; set; }
        public static int courseId { get; set; }
        public static int instructorId { get; set; }
        public static string firstName { get; set; }
        public static string lastName { get; set; }

        public virtual Cours Course { get; set; }
        public virtual ulUser User { get; set; }
    }
}