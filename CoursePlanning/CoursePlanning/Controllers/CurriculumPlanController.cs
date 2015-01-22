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

    [Authorize(Roles = "Admin")]
    public class CurriculumPlanController : ApiController
    {
        // Unit-of-work
        Worker m = new Worker();

        // GET: api/CurriculumPlan
        public CurriculumPlansLinked Get()
        {
            // Get all
            var fetchedObjects = m.CurriculumPlans.GetAll();

            // Create an object to be returned
            CurriculumPlansLinked curriculumPlans = new CurriculumPlansLinked(Mapper.Map<IEnumerable<CurriculumPlanWithLink>>(fetchedObjects), Request.RequestUri.AbsolutePath);

            // Tell the user what can be done with this collection
            curriculumPlans.Links[0].Method = "GET,POST";

            // Return the results
            return curriculumPlans;
        }

        // GET: api/CurriculumPlan/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get(int id)
        {
            // Get by identifier
            var fetchedObject = m.CurriculumPlans.GetById(id);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                // Create an object to be returned
                CurriculumPlanLinked curriculumPlan = new CurriculumPlanLinked(Mapper.Map<CurriculumPlanWithLink>(fetchedObject), Request.RequestUri.AbsolutePath);

                curriculumPlan.Links[0].Method = "GET,DELETE";
                curriculumPlan.Links[1].Method = "GET,POST";

                // Add another link to tell the user that they can set the program value
                Link program = new Link()
                {
                    Rel = "self",
                    Title = "Set the curriculum plan identifier",
                    Href = curriculumPlan.Links[0].Href,
                    Method = "PUT",
                    ContentType = "application/json"
                };
                curriculumPlan.Links.Add(program);

                // Return the results
                return Ok(curriculumPlan);
            }
        }

        // POST: api/CurriculumPlan
        public IHttpActionResult Post([FromBody]CurriculumPlanAdd newItem)
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
                var addedItem = m.CurriculumPlans.AddNew(newItem);

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
                    CurriculumPlanLinked curriculumPlan = new CurriculumPlanLinked(Mapper.Map<CurriculumPlanWithLink>(addedItem), uri.AbsolutePath);

                    // Return the object
                    return Created(uri, curriculumPlan);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        
        // PUT: api/CurriculumPlan/5
        public IHttpActionResult Put(int id, [FromBody]CurriculumPlanEdit editedItem)
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
                var changedItem = m.CurriculumPlans.EditExisting(editedItem);
                if (changedItem == null)
                {
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    return Ok<CurriculumPlanBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/CurriculumPlan/5/update-curriculumplan
        [Route("api/CurriculumPlan/{id}/update-curriculumplan")]
        public IHttpActionResult PutCurriculumPlanByProgram(int id, [FromBody]int programId)
        {
            // Get the program by identifier
            var fetchedCP = m.CurriculumPlans.GetById(id);
            // Get the course by identifier
            var fetchedProgram = m.Programs.GetById(programId);

            // Attempt to save
            if (m.Programs.UpdateCurriculumPlan(id, programId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                //return BadRequest("Unable to set the program for this Curriculum Plan");
                return BadRequest(string.Format("Unable to set program {0} to Curriculum Plan {1}", fetchedProgram.Code, fetchedCP.Code));
            }
        }

        // POST: api/CurriculumPlan/5/add-position
        [Route("api/CurriculumPlan/{id}/add-position")]
        [HttpPut]
        public IHttpActionResult PutPosition(int id, [FromBody]int positionId)
        {
            // Get the curriculum plan by identifier
            var fetchedCP = m.CurriculumPlans.GetById(id);

            // Attempt to save
            if (m.CurriculumPlans.AddPosition(id, positionId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                //return BadRequest("Unable to set the position for this Curriculum Plan");
                return BadRequest(string.Format("Unable to set position {0} to Curriculum Plan {1}", positionId, fetchedCP.Code));
            }
        }

        // DELETE: api/CurriculumPlan/5/remove-position
        [Route("api/CurriculumPlan/{id}/remove-position")]
        [HttpDelete]
        public IHttpActionResult RemovePosition(int id, [FromBody]int positionId)
        {
            // Get the curriculum plan by identifier
            var fetchedCP = m.CurriculumPlans.GetById(id);

            if (m.Programs.RemoveCourse(id, positionId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest(string.Format("Unable to remove position {0} from Curriculum Plan {1}", positionId, fetchedCP.Code));
            }

        }

        // DELETE: api/CurriculumPlans/5
        public IHttpActionResult Delete(int id)
        {
            // Get the program by identifier
            var fetchedCP = m.CurriculumPlans.GetById(id);

            if (m.CurriculumPlans.DeleteExisting(id))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest(string.Format("Unable to delete Curriculum Plan {0}", fetchedCP.Code));
            }
        }
    }
}

// ############################################################
// Resource model classes

namespace CoursePlanning.Controllers
{
    using System.ComponentModel.DataAnnotations;

    public class CurriculumPlanList
    {
        public int Id { get; set; }
        public string Code { get; set; }

    }

    public class CurriculumPlanAdd
    {
        public CurriculumPlanAdd()
        {
            this.IsVisible = true;
        }
        [Required, StringLength(100)]
        public string Code { get; set; }
        public Boolean IsVisible { get; set; }
    }

    public class CurriculumPlanBase : CurriculumPlanAdd
    {
        public int Id { get; set; }
        public int? AssoProgramId { get; set; }
        public ProgramList AssoProgram { get; set; }
        public ICollection<PositionList> AssoPositions { get; set; }

    }

    public class CurriculumPlanEdit
    {
        public int Id { get; set; }
        public int? AssoProgramId { get; set; }
    }

    public class CurriculumPlanAddTemplate
    {
        public string Code { get { return "CurriculumPlan, required, string, up to 100 characters"; } }
    }


    public class CurriculumPlanWithLink : CurriculumPlanBase
    {
        public Link Link { get; set; }
    }

    public class CurriculumPlanLinked : LinkedItem<CurriculumPlanWithLink>
    {
        // A refactoring experiment...
        public CurriculumPlanLinked(CurriculumPlanWithLink item, string absolutePath)
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

    public class CurriculumPlansLinked : LinkedCollection<CurriculumPlanWithLink>
    {
        public CurriculumPlansLinked()
        {
            this.Template = new CurriculumPlanAddTemplate();
        }

        // A refactoring experiment...
        public CurriculumPlansLinked(IEnumerable<CurriculumPlanWithLink> collection, string absolutePath)
        {
            this.Template = new CurriculumPlanAddTemplate();

            this.Collection = collection;

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath });

            // Add 'item' links for each item in the collection
            foreach (var item in this.Collection)
            {
                item.Link = new Link() { Rel = "item", Href = string.Format("{0}/{1}", absolutePath, item.Id) };
            }
        }

        public CurriculumPlanAddTemplate Template { get; set; }
    }

}

// ############################################################
// Repository

namespace CoursePlanning.ServiceLayer
{
    using AutoMapper;
    using CoursePlanning.Models;
    using CoursePlanning.Controllers;

    public class CurriculumPlan_repo : Repository<CurriculumPlan>
    {
        Worker m = new Worker();
        // Constructor
        public CurriculumPlan_repo(DataContext ds) : base(ds) { }

        // Methods called by controllers...

        // Get all
        public IEnumerable<CurriculumPlanBase> GetAll()
        {
            var fetchedObjects = RGetAll();

            return Mapper.Map<IEnumerable<CurriculumPlanBase>>(fetchedObjects.OrderBy(cp => cp.Id));
        }

        // Get one, by its identifier
        public CurriculumPlanBase GetById(int? id)
        {
            //var fetchedObject = RGetById(id);
            var fetchedObject = _ds.CurriculumPlans
                                   .Include("AssoProgram")
                                   .Include("AssoPositions")
                                   .SingleOrDefault(i => i.Id == id);

            return (fetchedObject == null) ? null : Mapper.Map<CurriculumPlanBase>(fetchedObject);
        }

        // Add new curriculum plan
        public CurriculumPlanBase AddNew(CurriculumPlanAdd newItem)
        {
            // Add the new object
            var addedItem = RAdd(Mapper.Map<CurriculumPlan>(newItem));

            // Return the object
            return Mapper.Map<CurriculumPlanBase>(addedItem);

        }

        // Update/edit existing curriculum plan
        public CurriculumPlanBase EditExisting(CurriculumPlanEdit editedItem)
        {
            // Add the new object
            //var updatedItem = REdit(Mapper.Map<CurriculumPlan>(editedItem));
            CurriculumPlanBase toBeUpdateItem = this.GetById(editedItem.Id);

            toBeUpdateItem.AssoProgramId = editedItem.AssoProgramId;
            toBeUpdateItem.AssoProgram = Mapper.Map<ProgramList>(m.Programs.GetById(editedItem.AssoProgramId));
            var updatedItem = REdit(toBeUpdateItem);

            // Return the object
            return Mapper.Map<CurriculumPlanBase>(updatedItem);

        }

        //Make existing course invisible from front end
        public bool DeleteExisting(int id)
        {
            //Validate curriculum plan
            var fetchedObject = _ds.CurriculumPlans
                .Include("AssoProgram")
                .Include("AssoPositions")
                .SingleOrDefault(i => i.Id == id && i.IsVisible == true);

            fetchedObject.IsVisible = !fetchedObject.IsVisible;
            var deletedItem = REdit(fetchedObject);
            return (deletedItem == null) ? false : true;
        }

        //Add a position to a curriculum plan
        public bool AddPosition(int curriculumPlanId, int positionId)
        {
            // Validate the course
            var curriculumPlan = _dbset.Find(curriculumPlanId);

            if (curriculumPlan == null)
            {
                return false;
            }
            else
            {
                // Validate the program
                var position = _ds.Positions.Find(positionId);
                if (position == null)
                {
                    return false;
                }
                else
                {
                    curriculumPlan.AssoPositions.Add(position);
                    SaveChanges();
                    return true;
                }
            }
        }

        //Remove a position from a curriculum plan
        public bool RemovePosition(int curriculumPlanId, int positionId)
        {
            //validate curriculumPlanId
            var curriculumPlan = _ds.CurriculumPlans.Find(curriculumPlanId);

            if (curriculumPlan == null)
            {
                return false;
            }
            else //validate position id
            {
                var position = _ds.Positions.Find(positionId);
                if (position == null)
                {
                    return false;
                }
                else
                {
                    curriculumPlan.AssoPositions.Remove(position);
                    SaveChanges();
                    return true;
                }
            }
        }

    }

}