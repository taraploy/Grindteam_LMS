//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Assignment1
{
    using System;
    using System.Collections.Generic;
    
    public partial class InstructorCours
    {
        public int instructor_course_id { get; set; }
        public int course_id { get; set; }
        public int instructor_id { get; set; }
    
        public virtual Cours Cours { get; set; }
        public virtual ulUser ulUser { get; set; }
    }
}
