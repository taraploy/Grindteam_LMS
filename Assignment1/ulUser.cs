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
    using System.Web;

    public partial class ulUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ulUser()
        {
            this.InstructorCourses = new HashSet<InstructorCours>();
            this.StudentCourses = new HashSet<StudentCours>();
        }
    
        public int ulUser_id { get; set; }
        public string user_password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email_address { get; set; }
        public System.DateTime birthdate { get; set; }
        public string role { get; set; }
        public string street_address { get; set; }
        public string phone_num { get; set; }
        public string bio { get; set; }
        public string link1 { get; set; }
        public string link2 { get; set; }
        public string link3 { get; set; }
        public string profileImage { get; set; }
        public string linkTitle1 { get; set; }
        public string linkTitle2 { get; set; }
        public string linkTitle3 { get; set; }
        public HttpPostedFileBase File { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstructorCours> InstructorCourses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentCours> StudentCourses { get; set; }
    }
}
