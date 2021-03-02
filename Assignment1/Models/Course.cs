using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment1.Models
{
    //[Table("Course")]
    public class Course
    {
        //public SelectList DropDownList { get; set; }

        public static int courseId { get; set; }
        public static string courseName { get; set; }
        public static string instructor { get; set; }
        public static TimeSpan startTime { get; set; }
        public static TimeSpan endTime { get; set; }
        public static string meetingDays { get; set; }
        public static string location { get; set; }
        public static string courseType { get; set; }
        public static string courseDesc { get; set; }
        public static string courseNum { get; set; }
        public static int maxCapacity { get; set; }
        public static int numCredits { get; set; }
        public static int roomNum { get; set; }
        public static int instructorId { get; set; }
        public static string instructorFullName { get; set; }
        public static string instructorFirstName { get; set; }
        public static string instructorLastName { get; set; }
        public static int deptId { get; set; }
        public static string department { get; set; }

        public List<Cours> allCourses { get; set; }
        //public ICollection<InstructorCours> InstructorCourses { get; set; }     //Teaches
        //public ICollection<StudentCours> StudentCourses { get; set; }           //Enrollments
    }
}