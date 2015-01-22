using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using System.ComponentModel.DataAnnotations;

// This source code file is used to define your model classes

namespace CoursePlanning.Models
{
    public class Program
    {
        public Program()
        {
            this.DateStarted = DateTime.Now;
            this.DateRetired = DateTime.Now.AddYears(50);
            this.IsVisible = true;
            this.AssoCourses = new List<Course>();
        }
        [Key]
        public int Id { get; set; }
        [Required, StringLength(20)]
        public string Code { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Credential { get; set; }
        public Boolean IsVisible { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateStarted { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateRetired { get; set; }

        // One-to-one, to the same entity
        // Include an int property to hold the identifier of the pointed-to object
        public int? AssoCurriculumPlanId { get; set; }

        // Rules...
        // A program MUST be associated with a curriculum plan (dependent)
        //This is the "dependent" end of program-curriculumPlan relationship
        [Required]
        public CurriculumPlan AssoCurriculumPlan { get; set; }
        // An program has a collection of classes (many to many relationship)
        public ICollection<Course> AssoCourses { get; set; }
    }



    public class Course
    {
        public Course()
        {
            this.DateStarted = DateTime.Now;
            this.DateRetired = DateTime.Now.AddYears(20);
            this.IsVisible = true;
            this.AssoPrograms = new List<Program>();
            this.AssoPositions = new List<Position>();
        }
        [Key]
        public int Id { get; set; }
        [Required, StringLength(20)]
        public string Code { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateStarted { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateRetired { get; set; }
        public Boolean IsVisible { get; set; }
        //A class has a collection of programs (many to many relationship)
        public ICollection<Program> AssoPrograms { get; set; }
        //A class has a collection of positions (one to many relationship)
        public ICollection<Position> AssoPositions { get; set; }
    }

    public class CurriculumPlan
    {
        public CurriculumPlan()
        {
            this.IsVisible = true;
            this.AssoPositions = new List<Position>();
        }
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Code { get; set; }

        public Boolean IsVisible { get; set; }
        //One to one relationship
        // Include an int property to hold the identifier of the pointed-to object
        // It must be nullable, because it is optional (in most situations)
        public int? AssoProgramId { get; set; }

        //Rules...
        // A curriculum plan OPTIONALLY have a program (principal)
        public Program AssoProgram { get; set; }
        //A curriculum plan has a collection of positions (one to many relationship)
        public ICollection<Position> AssoPositions { get; set; }
    }

    public class Position
    {
        public Position()
        {
            this.IsVisible = true;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int Semester { get; set; }
        public int DisplayOrder { get; set; }
        public Boolean IsVisible { get; set; }
        // Include an int property to hold the identifier of the pointed-to object
        // It must be nullable, because it is optional (in most situations)
        public int? AssoCourseId { get; set; }
        public Course AssoCourse { get; set; }
        // Include an int property to hold the identifier of the pointed-to object
        // It must be nullable, because it is optional (in most situations)
        public int? AssoCurriculumPlanId { get; set; }
        public CurriculumPlan AssoCurriculumPlan { get; set; }

    }

}
