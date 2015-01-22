using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

// In this code example, we're trying something new...
// This source code file will include the 'resource model' classes and the 'repository' class

namespace CoursePlanning.Controllers
{
    using AutoMapper;
    using CoursePlanning.ServiceLayer;

    public class CourseController : ApiController
    {
        // Unit-of-work
        Worker m = new Worker();

        // GET: api/Course
        public CoursesLinked Get()
        {
            // Get all
            var fetchedObjects = m.Courses.GetAll();

            // Create an object to be returned
            CoursesLinked courses = new CoursesLinked(Mapper.Map<IEnumerable<CourseWithLink>>(fetchedObjects), Request.RequestUri.AbsolutePath);

            // Tell the user what can be done with this collection
            courses.Links[0].Method = "GET,POST";

            // Return the results
            return courses;
        }

        // GET: api/Course/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get(int id)
        {
            // Get by identifier
            var fetchedObject = m.Courses.GetById(id);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                // Create an object to be returned
                CourseLinked course = new CourseLinked(Mapper.Map<CourseWithLink>(fetchedObject), Request.RequestUri.AbsolutePath);

                // Return the results
                return Ok(course);
            }
        }

        //Get: apibyname/Course/coursebycode?code={code}
        [ActionName("CourseByCode")]
        public IHttpActionResult GetByCode(string code)
        {
            //System.Diagnostics.Debug.WriteLine("GetByCode: code is -->" + code);
            // Get by identifier
            var fetchedObject = m.Courses.GetByCode(code);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                // Create an object to be returned
                CourseLinked course = new CourseLinked(Mapper.Map<CourseWithLink>(fetchedObject), Request.RequestUri.AbsolutePath);

                // Return the results
                return Ok(course);
            }
        }

        //Get: api/Course/GetPrograms?id=5
        [ActionName("GetPrograms")]
        public IHttpActionResult GetProgramsByCourse(int courseId)
        {
            System.Diagnostics.Debug.WriteLine("courseId is -->" + courseId);
            // Get program list by identifier
            var fetchedObjects = m.Courses.GetProgramList(courseId);

            if (fetchedObjects == null)
            {
                return NotFound();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("here");
                // Create an object to be returned
                ProgramsLinked programs = new ProgramsLinked(Mapper.Map<IEnumerable<ProgramWithLink>>(fetchedObjects), Request.RequestUri.AbsolutePath);
                // Tell the user what can be done with this collection
                programs.Links[0].Method = "GET,POST";

                // Return the results
                return Ok(programs);
            }
        }

        // POST: api/Course
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post([FromBody]CourseAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null)
            {
                return BadRequest("Invalid request URI");
            }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to add the new item
                var addedItem = m.Courses.AddNew(newItem);

                if (addedItem == null)
                {
                    return BadRequest("Cannot add the object");
                }
                else
                {
                    // HTTP 201 with the new object in the entity body

                    // Build the URI to the new object
                    Uri uri = new Uri(Url.Link("DefaultApi", new { id = addedItem.Id }));

                    // Create an object to be returned
                    CourseLinked course = new CourseLinked(Mapper.Map<CourseWithLink>(addedItem), uri.AbsolutePath);

                    // Return the object
                    return Created(uri, course);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // PUT: api/Course/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Put(int id, [FromBody]CourseEdit editedItem)
        {
            // Ensure that the URI have an id parameter)
            if (Request.GetRouteData().Values["id"] == null)
            {
                return BadRequest("Invalid request URI");
            }
            //Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            //Check the edited item has the same id number with the one from uri. If not, return error message.
            if (id != editedItem.Id)
            {
                return BadRequest("Invalid data in the entity body");
            }

            if (ModelState.IsValid)
            {
                var changedItem = m.Courses.EditExisting(editedItem);
                if (changedItem == null)
                {
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    return Ok<CourseBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Course/5/add-program
        [Authorize(Roles = "Admin")]
        [Route("api/Course/{id}/add-program")]
        [HttpPut]
        public IHttpActionResult PutProgram(int id, [FromBody]int programId)
        {
            // Get the course by identifier
            var fetchedCourse = m.Courses.GetById(id);
            // Get the program by identifier
            var fetchedProgram = m.Programs.GetById(programId);

            // Attempt to save
            if (m.Courses.AddProgram(id, programId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                //return BadRequest("Unable to set the program for this Course");
                return BadRequest(string.Format("Unable to set program {0} to Course {1}", fetchedProgram.Code, fetchedCourse.Code));
            }
        }

        // DELETE: api/Course/5/remove-program
        [Authorize(Roles = "Admin")]
        [Route("api/Course/{id}/remove-program")]
        [HttpDelete]
        public IHttpActionResult RemoveProgram(int id, [FromBody]int programId)
        {
            // Get the course by identifier
            var fetchedCourse = m.Courses.GetById(id);
            // Get the program by identifier
            var fetchedProgram = m.Programs.GetById(programId);

            if (m.Courses.RemoveProgram(id, programId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest(string.Format("Unable to remove program {0} from Course {1}", fetchedProgram.Code, fetchedCourse.Code));
            }

        }

        // DELETE: api/Course/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int id)
        {
            // Get the course by identifier
            var fetchedCourse = m.Courses.GetById(id);

            if (m.Courses.DeleteExisting(id))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest(string.Format("Unable to delete Course {0}", fetchedCourse.Code));
            }
        }
    }
}

// ############################################################
// Resource model classes

namespace CoursePlanning.Controllers
{
    using System.ComponentModel.DataAnnotations;

    public class CourseList
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
    }

    public class CourseAdd
    {
        public CourseAdd()
        {
            this.DateStarted = DateTime.Now;
            this.DateRetired = DateTime.Now.AddYears(20);
            this.IsVisible = true;
        }
        [Required, StringLength(100)]
        public string Code { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateRetired { get; set; }
        public Boolean IsVisible { get; set; }
    }

    public class CourseAddTemplate
    {
        public string Code { get { return "Course code, required, string, up to 100 characters"; } }
        public string Title { get { return "Course title, required, string, up to 100 characters"; } }
        public string Description { get { return "Course descripion, string"; } }
        public string DateStarted { get { return "Course start date, date"; } }
        public string DateRetired { get { return "Course retire date, date"; } }
    }

    public class CourseBase : CourseAdd
    {
        public int Id { get; set; }
        public ICollection<ProgramList> AssoPrograms { get; set; }
        public ICollection<PositionList> AssoPositions { get; set; } 
    }

    public class CourseEdit
    {
        public int Id { get; set; }
        public CourseEdit()
        {
            this.DateRetired = DateTime.Now.AddYears(20);
        }
        public string Description { get; set; }
        public DateTime? DateRetired { get; set; }

    }

    public class CourseWithLink : CourseBase
    {
        public Link Link { get; set; }
    }

    public class CourseLinked : LinkedItem<CourseWithLink>
    {
        // A refactoring experiment...
        public CourseLinked(CourseWithLink item, string absolutePath)
        {
            this.Item = item;

            // Link relation for 'self' in the item
            this.Item.Link = new Link() { Rel = "self", Href = absolutePath };

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath });
            //this.Links.Add(this.Item.Link);

            // Link relation for 'collection'
            string[] u = absolutePath.Split(new char[] { '/' });
            this.Links.Add(new Link() { Rel = "collection", Href = string.Format("/{0}/{1}", u[1], u[2]) });
        }
    }

    public class CoursesLinked : LinkedCollection<CourseWithLink>
    {
        public CoursesLinked()
        {
            this.Template = new CourseAddTemplate();
        }

        //Newly add in week 8, one more constructor which takes two parameter
        // A refactoring experiment...
        public CoursesLinked(IEnumerable<CourseWithLink> collection, string absolutePath)
        {
            this.Template = new CourseAddTemplate();

            this.Collection = collection;

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath });

            // Add 'item' links for each item in the collection
            foreach (var item in this.Collection)
            {
                item.Link = new Link() { Rel = "item", Href = string.Format("{0}/{1}", absolutePath, item.Id) };
            }
        }

        public CourseAddTemplate Template { get; set; }
    }

}

// ############################################################
// Repository

namespace CoursePlanning.ServiceLayer
{
    using AutoMapper;
    using CoursePlanning.Models;
    using CoursePlanning.Controllers;

    public class Course_repo : Repository<Course>
    {
        // Constructor
        public Course_repo(DataContext ds) : base(ds) { }

        // Methods called by controllers...

        // Get all
        public IEnumerable<CourseBase> GetAll()
        {
            //var fetchedObjects = RGetAll();
            var fetchedObjects = _ds.Courses
                                    .Include("AssoPrograms")
                                    .Include("AssoPositions") 
                                    .Where(cs => cs.IsVisible == true);
            return Mapper.Map<IEnumerable<CourseBase>>(fetchedObjects.OrderBy(cs => cs.Code));

        }

        //Get a list of programs by a given course id
        public IEnumerable<ProgramBase> GetProgramList(int courseId)
        {
            var fetchedObject = _ds.Courses
                                    .Include("AssoPrograms")
                                    .SingleOrDefault(i => i.Id == courseId && i.IsVisible == true);
            if (fetchedObject == null)
            {
                return null;
            }
            else
            {
                var assoProgramList = fetchedObject.AssoPrograms;
                return (assoProgramList == null) ? null : Mapper.Map<IEnumerable<ProgramBase>>(assoProgramList);
            }
        }

        // Get one, by its identifier
        public CourseBase GetById(int id)
        {
            //var fetchedObject = RGetById(id);
            var fetchedObject = _ds.Courses
                .Include("AssoPrograms")
                .Include("AssoPositions")
                .SingleOrDefault(i => i.Id == id && i.IsVisible ==true);

            return (fetchedObject == null) ? null : Mapper.Map<CourseBase>(fetchedObject);
        }

        // Get one, by its code
        public CourseBase GetByCode(string code)
        {
            //var fetchedObject = RGetById(id);
            var fetchedObject = _ds.Courses
                .Include("AssoPrograms")
                .Include("AssoPositions")
                .SingleOrDefault(i => i.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase) 
                                 && i.IsVisible == true);

            return (fetchedObject == null) ? null : Mapper.Map<CourseBase>(fetchedObject);
        }

        // Add new course
        public CourseBase AddNew(CourseAdd newItem)
        {
            // Add the new object
            var addedItem = RAdd(Mapper.Map<Course>(newItem));

            // Return the object
            return Mapper.Map<CourseBase>(addedItem);
        }

        //Edit existing course
        public CourseBase EditExisting(CourseEdit editedItem)
        {
            //Update the objects
            var updatedItem = REdit(editedItem);

            return Mapper.Map<CourseBase>(updatedItem);
        }

        //Make existing course invisible from front end
        public bool DeleteExisting(int id)
        {
            //Validate course
            var fetchedObject = _ds.Courses
                .Include("AssoPrograms")
                .Include("AssoPositions")
                .SingleOrDefault(i => i.Id == id && i.IsVisible == true);

            fetchedObject.IsVisible = !fetchedObject.IsVisible;
            var deletedItem = REdit(fetchedObject);
            return (deletedItem == null) ? false : true;
        }


        // Add program to a course
        public bool AddProgram(int courseId, int programId)
        {
            // Validate the course
            var course = _dbset.Find(courseId);

            if (course == null)
            {
                return false;
            }
            else
            {
                // Validate the program
                var program = _ds.Programs.Find(programId);

                if (program == null)
                {
                    return false;
                }
                else
                {
                    course.AssoPrograms.Add(program);
                    SaveChanges();

                    return true;
                }
            }
        }

        //Remove program to a course
        public bool RemoveProgram(int courseId, int programId)
        {
            // Validate the course
            var course = _dbset.Find(courseId);

            if (course == null)
            {
                return false;
            }
            else
            {
                // Validate the program
                var program = _ds.Programs.Find(programId);

                if (program == null)
                {
                    return false;
                }
                else
                {
                    course.AssoPrograms.Remove(program);
                    SaveChanges();

                    return true;
                }
            }
        }

    }

}
