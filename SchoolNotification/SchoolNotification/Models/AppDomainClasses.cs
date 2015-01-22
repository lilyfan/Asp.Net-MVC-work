using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolNotification.Models
{
    public class ApplicationUser : UserProfile//IdentityUser
    {
        public ApplicationUser()
        {
            FirstName = LastName = string.Empty;
        }
        public ApplicationUser(string f, string l)
        {
            FirstName = f;
            LastName = l;
        }
        //[Key]
        //public int Id { get; set; }
        //[Required]
        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }

    }
    public class Admin : ApplicationUser//IdentityUser
    {
        public Admin() { FirstName = LastName = EmployeeId =string.Empty; }
        public Admin(string f, string l, string eId)
        {
            FirstName = f;
            LastName = l;
            EmployeeId = eId;
        }
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string EmployeeId { get; set; }

    }
    public class Faculty : ApplicationUser//IdentityUser
    {
        public Faculty()
        {
            FirstName = LastName = FacultyId = string.Empty;
            this.Courses = new List<Course>();
            //this.Messages = new List<Message>();
        }
        public Faculty(string f, string l, string fid)
        {
            FirstName = f;
            LastName = l;
            FacultyId = fid;
            this.Courses = new List<Course>();
            //this.Messages = new List<Message>();
        }
        //public int Id { get; set; }
        //[Required]
        //public string FirstName { get; set; }
        //[Required]
        //public string LastName { get; set; }
        //[Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string FacultyId { get; set; }

        public List<Course> Courses { get; set; }
        //public List<Message> Messages { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }

    }

    public class Student : ApplicationUser//IdentityUser
    {
        public Student()
        {
            FirstName = LastName = StudentId = string.Empty;
            this.Courses = new List<Course>();
            this.Contacts = new List<Contact>();
        }
        public Student(string f, string l, string sid)
        {
            FirstName = f;
            LastName = l;
            StudentId = sid;
            this.Courses = new List<Course>();
            this.Contacts = new List<Contact>();
        }

        //[Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string StudentId { get; set; }
        public List<Course> Courses { get; set; }
        public List<Contact> Contacts { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }

    }
    //[Table("DataContext.Courses")]
    public class Course
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        public string CourseName { get; set; }
        //[Required]
        public string CourseCode { get; set; }
        //public List<DateTime> Schedule {get;set;}
        public string Date { get; set; }
        public string Time { get; set; }
        public string Room { get; set; }
        //public string Faculty_Id { get; set; }
        public Faculty Faculty { get; set; }
        public List<Student> Students { get; set; }
        public virtual UserProfile User { get; set; }
        public Course()
        {
            CourseName = CourseCode = Room = Date = Time = string.Empty;
            this.Faculty = new Faculty();
            this.Students = new List<Student>();
        }
        public Course(string cn, string cc, string r, string d, string t)
        {
            CourseName = cn;
            CourseCode = cc;
            Room = r;
            Date = d;
            Time = t;
            this.Faculty = new Faculty();
            this.Students = new List<Student>();
        }
    }

    public class Cancellation
    {
        [Key]
        public int Id { get; set; }
        //public DateTime Date { get; set; }
        //public Course Course { get; set; }
        public string Date { get; set; }
        public Course Course { get; set; }
        public Message Message { get; set; }
        public Faculty Faculty { get; set; }
        //public virtual ApplicationUser User { get; set; }
        public Cancellation()
        {
            Date  = string.Empty;
            this.Course = new Course();
            this.Message = new Message();
            this.Faculty = new Faculty();
        }
        public Cancellation(string d)
        {
            Date = d;
            this.Course = new Course();
            this.Message = new Message();
            this.Faculty = new Faculty();
        }
    }

    public class Message
    {
        public Message()
        {
            CourseCode = Date = Time  = MsgContent = string.Empty;
        }
        //public Message(string cn, string d, string t, string fn, string mgc)
        public Message(string cn, string d, string t,  string mgc)
        {
            CourseCode = cn;
            Date = d;
            Time = t;
            //FacultyName = fn;
            MsgContent = mgc;
        }
        [Key]
        public int Id { get; set; }
        //[Required]
        public string CourseCode { get; set; }
        //[Required]
        public string Date { get; set; }
        //[Required]
        public string Time { get; set; }
        [Required]
        public string MsgContent { get; set; }
        //public Cancellation Cancellation { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class Contact
    {
        public Contact()
        {
            ContactInfo = string.Empty;
        }
        public Contact(string cm)
        {
            ContactInfo = cm;
            //StudentId = sid;
        }
        [Key]
        public int Id { get; set; }
        [Display(Name="Contact Info")]
        public string ContactInfo { get; set; }
        public Student Student { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }



}