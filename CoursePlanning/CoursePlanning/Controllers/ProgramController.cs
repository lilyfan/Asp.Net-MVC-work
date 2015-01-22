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

    public class ProgramController : ApiController
    {
        // Unit-of-work
        Worker m = new Worker();

        // GET: api/Program
        public ProgramsLinked Get()
        {
            // Get all
            var fetchedObjects = m.Programs.GetAll();

            // Create an object to be returned
            ProgramsLinked programs = new ProgramsLinked(Mapper.Map<IEnumerable<ProgramWithLink>>(fetchedObjects), Request.RequestUri.AbsolutePath);

            // Tell the user what can be done with this collection
            programs.Links[0].Method = "GET,POST";

            // Return the results
            return programs;
        }

        // GET: api/Program/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get(int id)
        {
            // Get by identifier
            var fetchedObject = m.Programs.GetById(id);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                // Create an object to be returned
                ProgramLinked program = new ProgramLinked(Mapper.Map<ProgramWithLink>(fetchedObject), Request.RequestUri.AbsolutePath);

                // Tell the user what can be done with this item and collection
                program.Links[0].Method = "GET,DELETE";
                program.Links[1].Method = "GET,POST";
                
                // Add another link to tell the user that they can set the curriculum plan value
                Link curriculumPlan = new Link()
                {
                    Rel = "self",
                    Title = "Set the curriculum plan identifier",
                    Href = program.Links[0].Href,
                    Method = "PUT",
                    ContentType = "application/json"
                };
                program.Links.Add(curriculumPlan);
                
                // Return the results
                return Ok(program);
            }

        }

        //GET: apibyname/Program/programbycode?code={code}
        [ActionName("ProgramByCode")]
        public IHttpActionResult GetByCode(string code)
        {
            // Get by identifier
            var fetchedObject = m.Programs.GetByCode(code);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                // Create an object to be returned
                ProgramLinked program = new ProgramLinked(Mapper.Map<ProgramWithLink>(fetchedObject), Request.RequestUri.AbsolutePath);

                // Tell the user what can be done with this item and collection
                program.Links[0].Method = "GET,DELETE";
                program.Links[1].Method = "GET,POST";

                // Add another link to tell the user that they can set the curriculum plan value
                Link curriculumPlan = new Link()
                {
                    Rel = "self",
                    Title = "Set the curriculum plan identifier",
                    Href = program.Links[0].Href,
                    Method = "PUT",
                    ContentType = "application/json"
                };
                program.Links.Add(curriculumPlan);


                // Return the results
                return Ok(program);
            }
        }

        //Get: api/Program/GetCourses?id=5
        [ActionName("GetCourses")]
        public IHttpActionResult GetCoursesByProgram(int programId)
        {
            // Get program list by identifier
            var fetchedObjects = m.Programs.GetCourseList(programId);

            if (fetchedObjects == null)
            {
                return NotFound();
            }
            else
            {
                // Create an object to be returned
                CourseLinked courses = new CourseLinked(Mapper.Map<CourseWithLink>(fetchedObjects), Request.RequestUri.AbsolutePath);

                // Tell the user what can be done with this collection
                courses.Links[0].Method = "GET,POST";

                // Return the results
                return Ok(courses);
            }
        }

        // POST: api/Program
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post([FromBody]ProgramAdd newItem)
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
            //validate the curriculum plan in the add program
            var curriculumPlanId = newItem.AssoCurriculumPlanId;
            var fetchedCP = m.CurriculumPlans.GetById(curriculumPlanId);
            if (fetchedCP == null)
            {
                return BadRequest("Must provide the valid curriculum plan");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                //Add curriculum plan to new item
                //newItem.AssoCurriculumPlan = Mapper.Map<CurriculumPlanList>(fetchedCP);
                // Attempt to add the new item
                var addedItem = m.Programs.AddNew(newItem, Mapper.Map<CurriculumPlanList>(fetchedCP));
                

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
                    ProgramLinked program = new ProgramLinked(Mapper.Map<ProgramWithLink>(addedItem), uri.AbsolutePath);
                    // Return the object
                    return Created(uri, program);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // PUT: api/Program/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Put(int id, [FromBody]ProgramEdit editedItem)
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
                var changedItem = m.Programs.EditExisting(editedItem);
                if (changedItem == null)
                {
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    return Ok<ProgramBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Program/5/update-curriculumplan
        [Authorize(Roles = "Admin")]
        [Route("api/Program/{id}/update-curriculumplan")]
        [HttpPut]
        public IHttpActionResult PutCurriculumPlanByProgram(int id, [FromBody]int curriculumPlanId)
        {
            // Get the program by identifier
            var fetchedCP = m.CurriculumPlans.GetById(curriculumPlanId);
            // Get the course by identifier
            var fetchedProgram = m.Programs.GetById(id);

            // Attempt to save
            if (m.Programs.UpdateCurriculumPlan(id, curriculumPlanId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                //return BadRequest("Unable to set the curriculum plan for this Program");
                return BadRequest(string.Format("Unable to set curriculum plan {0} to Program {1}", fetchedCP.Code, fetchedProgram.Code));
            }
        }

        // PUT: api/Program/5/add-course
        [Authorize(Roles = "Admin")]
        [Route("api/Program/{id}/add-course")]
        [HttpPut]
        public IHttpActionResult PutCourse(int id, [FromBody]int courseId)
        {
            // Get the program by identifier
            var fetchedProgram = m.Programs.GetById(id);
            // Get the course by identifier
            var fetchedCourse = m.Courses.GetById(courseId);

            // Attempt to save
            if (m.Programs.AddCourse(id, courseId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                //return BadRequest("Unable to set the course for this Program");
                return BadRequest(string.Format("Unable to set course {0} to Program {1}", fetchedCourse.Code, fetchedProgram.Code));
            }
        }

        // DELETE: api/Program/5/remove-course
        [Authorize(Roles = "Admin")]
        [Route("api/Program/{id}/remove-course")]
        [HttpDelete]
        public IHttpActionResult RemoveCourse(int id, [FromBody]int courseId)
        {
            // Get the program by identifier
            var fetchedProgram = m.Programs.GetById(id);
            // Get the course by identifier
            var fetchedCourse = m.Courses.GetById(courseId);

            if (m.Programs.RemoveCourse(id, courseId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest(string.Format("Unable to remove course {0} from Program {1}", fetchedCourse.Code, fetchedProgram.Code));
            }

        }

        // DELETE: api/Program/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int id)
        {
            // Get the program by identifier
            var fetchedProgram = m.Programs.GetById(id);

            if (m.Programs.DeleteExisting(id))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest(string.Format("Unable to delete Program {0}", fetchedProgram.Code));
            }
        }
    }
}

// ############################################################
// Resource model classes

namespace CoursePlanning.Controllers
{
    using System.ComponentModel.DataAnnotations;

    public class ProgramList
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
    }

    public class ProgramAdd
    {
        public ProgramAdd()
        {
            this.DateStarted = DateTime.Now;
            this.DateRetired = DateTime.Now.AddYears(50);
            this.IsVisible = true;
        }
        [Required, StringLength(20)]
        public string Code { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Credential { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateRetired { get; set; }
        public int? AssoCurriculumPlanId { get; set; }
        public Boolean IsVisible { get; set; }
    }

    public class ProgramAddTemplate
    {
        public string Code { get { return "Course code, required, string, up to 100 characters"; } }
        public string Title { get { return "Course title, required, string, up to 100 characters"; } }
        public string Description { get { return "Course descripion, string"; } }
        public string Credential { get { return "Course credential, e.g. grad cert, diploma, advanced diploma, degree string"; } }
        public string DateStarted { get { return "Course start date, date"; } }
        public string DateRetired { get { return "Course retire date, date"; } }
    }

    public class ProgramBase : ProgramAdd
    {
        public ProgramBase() {
            this.AssoCourses = new List<CourseList>();
        }
        public int Id { get; set; }
        [Required]
        public CurriculumPlanList AssoCurriculumPlan { get; set; }
        public ICollection<CourseList> AssoCourses { get; set; }
    }

    public class ProgramEdit
    {
        public int Id { get; set; }
        public ProgramEdit()
        {
            this.DateRetired = DateTime.Now.AddYears(50);
        }
        public string Description { get; set; }
        public string Credential { get; set; }
        public DateTime DateRetired { get; set; }

    }

    public class ProgramWithLink : ProgramBase
    {
        public Link Link { get; set; }
    }

    public class ProgramLinked : LinkedItem<ProgramWithLink>
    {
        // A refactoring experiment...
        public ProgramLinked(ProgramWithLink item, string absolutePath)
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

    public class ProgramsLinked : LinkedCollection<ProgramWithLink>
    {
        public ProgramsLinked()
        {
            this.Template = new ProgramAddTemplate();
        }

        // A refactoring experiment...
        public ProgramsLinked(IEnumerable<ProgramWithLink> collection, string absolutePath)
        {
            this.Template = new ProgramAddTemplate();

            this.Collection = collection;

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath });

            // Add 'item' links for each item in the collection
            foreach (var item in this.Collection)
            {
                item.Link = new Link() { Rel = "item", Href = string.Format("{0}/{1}", absolutePath, item.Id) };
            }
        }

        public ProgramAddTemplate Template { get; set; }
    }

}

// ############################################################
// Repository

namespace CoursePlanning.ServiceLayer
{
    using AutoMapper;
    using CoursePlanning.Models;
    using CoursePlanning.Controllers;

    public class Program_repo : Repository<Program>
    {
        // Constructor
        public Program_repo(DataContext ds) : base(ds) { }

        // Methods called by controllers...

        // Get all
        public IEnumerable<ProgramBase> GetAll()
        {
            //var fetchedObjects = RGetAll();
            var fetchedObjects = _ds.Programs
                                    .Include("AssoCurriculumPlan")
                                    .Include("AssoCourses")
                                    .Where(i=>i.IsVisible == true);
            return Mapper.Map<IEnumerable<ProgramBase>>(fetchedObjects.OrderBy(pr => pr.Code));
        }

        //Get a list of courses by a given program id
        public IEnumerable<CourseBase> GetCourseList(int programId)
        {
            var fetchedObject = _ds.Programs
                                    .Include("AssoCourses")
                                    .SingleOrDefault(i => i.Id == programId && i.IsVisible == true);
            if (fetchedObject == null)
            {
                return null;
            }
            else
            {
                var assoCourseList = fetchedObject.AssoCourses;
                return (assoCourseList == null) ? null : Mapper.Map<IEnumerable<CourseBase>>(assoCourseList);
            }
        }


        // Get one, by its identifier
        public ProgramBase GetById(int? id)
        {
            var fetchedObject = _ds.Programs
                .Include("AssoCurriculumPlan")
                .Include("AssoCourses")
                .SingleOrDefault(i => i.Id == id && i.IsVisible == true);

            return (fetchedObject == null) ? null : Mapper.Map<ProgramBase>(fetchedObject);
        }

        // Get one, by its code
        public ProgramBase GetByCode(string code)
        {
            var fetchedObject = _ds.Programs
                .Include("AssoCurriculumPlan")
                .Include("AssoCourses")
                .SingleOrDefault(i => i.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)
                                 && i.IsVisible == true);

            return (fetchedObject == null) ? null : Mapper.Map<ProgramBase>(fetchedObject);
        }

        // Add new program
        public ProgramBase AddNew(ProgramAdd newItem, CurriculumPlanList curriculumPlan)
        {
            System.Diagnostics.Debug.WriteLine("curriculumPlan: id is -->" + curriculumPlan.Id);
            // Get by identifier
            // Add the new object
            //var addedItem = RAdd(Mapper.Map<Program>(newItem));
            ProgramBase toBeAddItem = Mapper.Map<ProgramBase>(newItem);

            var addedItem = new Program(); 
            addedItem = Mapper.Map<Program>(toBeAddItem);
            addedItem.AssoCurriculumPlan = Mapper.Map<CurriculumPlan>(curriculumPlan);
            _ds.Programs.Add(addedItem);
            _ds.SaveChanges();

            // Return the object
            return Mapper.Map<ProgramBase>(addedItem);
        }

        //Edit existing program
        public ProgramBase EditExisting(ProgramEdit editedItem)
        {
            //System.Diagnostics.Debug.WriteLine("edited item id is -->" + editedItem.Id);
            //Update the objects
            //var updatedItem = REdit(editedItem);
            ProgramBase toBeUpdateItem = GetById(editedItem.Id);

            toBeUpdateItem.Credential = editedItem.Credential;
            toBeUpdateItem.Description = editedItem.Description;
            toBeUpdateItem.DateRetired = editedItem.DateRetired;

            var updatedItem = REdit(toBeUpdateItem);
            SaveChanges();

            return Mapper.Map <ProgramBase> (updatedItem);
        }

        //Make existing invisible from front end
        public bool DeleteExisting(int id)
        {
            //Validate course
            var fetchedObject = _ds.Programs
                .Include("AssoCurriculumPlan")
                .Include("AssoCourses")
                .SingleOrDefault(i => i.Id == id && i.IsVisible == true);

            fetchedObject.IsVisible = !fetchedObject.IsVisible;
            var deletedItem = REdit(fetchedObject);
            return (deletedItem == null) ? false : true;
        }

        // Update curriculum plan for a program                  
        public bool UpdateCurriculumPlan(int programId, int curriculumPlanId)
        {
            // Validate the program
            var program = _dbset.Find(programId);

            if (program == null)
            {
                return false;
            }
            else
            {
                // Validate the curriculum plan
                var curriculumPlan = _ds.CurriculumPlans.Find(curriculumPlanId);

                if (curriculumPlan == null)
                {
                    return false;
                }
                else
                {
                    program.AssoCurriculumPlan = curriculumPlan;
                    program.AssoCurriculumPlanId = curriculumPlan.Id;
                    SaveChanges();

                    return true;
                }
            }
        }

        // Add course to a program
        public bool AddCourse(int programId, int courseId)
        {
            // Validate the program
            var program = _dbset.Find(programId);

            if (program == null)
            {
                return false;
            }
            else
            {
                // Validate the course
                var course = _ds.Courses.Find(courseId);

                if (course == null)
                {
                    return false;
                }
                else
                {
                    program.AssoCourses.Add(course);
                    SaveChanges();

                    return true;
                }
            }
        }

        // Remove course from program
        public bool RemoveCourse(int programId, int courseId)
        {
            // Validate the program
            var program = _dbset.Find(programId);

            if (program == null)
            {
                return false;
            }
            else
            {
                // Validate the course
                var course = _ds.Courses.Find(courseId);

                if (course == null)
                {
                    return false;
                }
                else
                {
                    program.AssoCourses.Remove(course);
                    SaveChanges();

                    return true;
                }
            }
        }

    }

}
