using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class FacultyCreate
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string FacultyId { get; set; }
        public List<Course> Courses { get; set; }
        //public virtual ApplicationUser User { get; set; }

        //public List<CourseBase> Courses { get; set; }
        //public List<Message> Messages { get; set; }
    }
    public class FacultyBase
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        //public virtual ApplicationUser User { get; set; }
    }
    public class FacultyFull : FacultyBase
    {
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string FacultyId { get; set; }

        public List<CourseBase> Courses { get; set; }
        //public List<Message> Messages { get; set; }
        public FacultyFull()
        {
            this.Courses = new List<CourseBase>();
        }
    }
}