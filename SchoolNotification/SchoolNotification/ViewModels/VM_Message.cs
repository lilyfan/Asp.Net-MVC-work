using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class MessageCreate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CourseCode { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public string MsgContent { get; set; }

        //public Cancellation Cancellation { get; set; }
        //public virtual UserProfile User { get; set; }

        public MessageCreate()
        {
            CourseCode = Date = Time = MsgContent = string.Empty;
        }
        public MessageCreate(string cn, string d, string t, string msg)
        {
            CourseCode = cn;
            Date = d;
            Time = t;
            MsgContent = msg;
        }
        

    }
    public class MessageBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CourseCode { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Time { get; set; }
        //public virtual ApplicationUser User { get; set; }

    }
    public class MessageFull : MessageBase
    {
        [Required]
        public string MsgContent { get; set; }

    }
}