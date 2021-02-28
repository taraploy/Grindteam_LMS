using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class Course
    {

        //public SelectList DropDownList { get; set; }

        public int courseId { get; set; }
        public string courseName { get; set; }
        public string instructor { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public string meetingDays { get; set; }
        public string location { get; set; }
        public string courseType { get; set; }
        public string courseDesc { get; set; }
        public string courseNum { get; set; }
        public int maxCapacity { get; set; }
        public int deptId { get; set; }
        public int numCredits { get; set; }
        public int roomNum { get; set; }

        //public ICollection<Department> Department { get; set; }
    }
}