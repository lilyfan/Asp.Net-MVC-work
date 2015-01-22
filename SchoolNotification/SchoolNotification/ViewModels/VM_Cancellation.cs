using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class CancellationCreate
    {
        [Key]
        public int Id { get; set; }
        //public List<DateTime> Dates { get; set; }
        //public List<Course> Courses { get; set; }
        public string Date { get; set; }
        public Course Course { get; set; }
        public Message Message { get; set; }
        public Faculty Faculty { get; set; }
        public virtual ApplicationUser User { get; set; }

        public CancellationCreate()
        {
            Date  = string.Empty;
            this.Course = new Course();
            this.Message = new Message();
            this.Faculty = new Faculty();
        }
    }
    public class CancellationBase
    {
        [Key]
        public int Id { get; set; }
        public string Date { get; set; }
        public Course Course { get; set; }
        
        public virtual ApplicationUser User { get; set; }

    }
    public class CancellationFull : CancellationBase
    {
        public Faculty Faculty { get; set; }
        //public List<Message> Messages { get; set; }
        public Message Message { get; set; }

    }
}