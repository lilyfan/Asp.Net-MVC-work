using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class StudentCreate
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string StudentId { get; set; }
        public List<Course> Courses { get; set; }
        //public List<Contact> Contacts { get; set; }
        //public virtual ApplicationUser User { get; set; }

        public StudentCreate()
        {
            FirstName = LastName = StudentId = string.Empty;
            this.Courses = new List<Course>();
        }
        public StudentCreate(string f, string l, string sid)
        {
            FirstName = f;
            LastName = l;
            StudentId = sid;
            this.Courses = new List<Course>();
        }
    }
    public class StudentBase
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string StudentId { get; set; }
        //public virtual ApplicationUser User { get; set; }

    }
    public class StudentFull : StudentBase
    {
        public List<Course> Courses { get; set; }
        public List<Contact> Contacts { get; set; }


    }
}