using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class ContactCreate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Contact Info")]
        public string ContactInfo { get; set; }
        public Student Student { get; set; }

    }
    public class ContactBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Contact Info")]
        public string ContactInfo { get; set; }

    }

    public class ContactFull : ContactBase
    {
        public Student Student { get; set; }
    }
}