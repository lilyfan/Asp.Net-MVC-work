using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class CourseCreate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string CourseCode { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Room { get; set; }
        //public int FacutlyId { get; set; }
        public Faculty Faculty { get; set; }
        //public List<Student> Students { get; set; }

        public virtual ApplicationUser User { get; set; }     
        public CourseCreate()
        {
            CourseName = CourseCode = Room = Date = Time = string.Empty;
            this.Faculty = new Faculty();
            //this.Students = new List<Student>();
        }
        public CourseCreate(string cn, string cc, string r, string d, string t)
        {
            CourseName = cn;
            CourseCode = cc;
            Room = r;
            Date = d;
            Time = t;
            this.Faculty = new Faculty();
            //this.Students = new List<Student>();
        }
    }
    public class CourseBase
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        public string CourseName { get; set; }
        //[Required]
        public string CourseCode { get; set; }
        public virtual ApplicationUser User { get; set; }
        /*public CourseBase()
        {
            CourseName = CourseCode  = string.Empty;
        }
        public CourseBase(string cn, string cc)
        {
            CourseName = cn;
            CourseCode = cc;
        }*/

    }
    public class CourseFull : CourseBase
    {
        //public List<DateTime> Schedule { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Room { get; set; }
        public Faculty Faculty { get; set; }
        //public List<Student> Students { get; set; }

        /*public CourseFull()
        {
            Date = Time = Room = string.Empty;
            //this.Schedule = new List<DateTime>();
            this.Faculty = new Faculty();
            //this.Students = new List<Student>();
        }
        public CourseFull(string cn, string cc, string r, string d, string t) 
        {
            CourseName = cn;
            CourseCode = cc;
            Room = r;
            Date = d;
            Time = t;
            this.Faculty = new Faculty();
            //this.Students = new List<Student>();
        }*/
    }
}