using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class AdminCreate
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string EmployeeId { get; set; }

    }
    public class AdminBase
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

    }
    public class AdminFull : AdminBase
    {
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string EmployeeId { get; set; }

    }
}