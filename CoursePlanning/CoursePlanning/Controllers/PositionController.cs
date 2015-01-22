using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoursePlanning.Controllers
{
    using AutoMapper;
    using CoursePlanning.ServiceLayer;
    public class PositionController : ApiController
    {
        Worker m = new Worker();
        
        // GET: api/Position
        public PositionsLinked Get()
        {
            var fetchedObjects = m.Positions.GetAll();

            PositionsLinked positions = new PositionsLinked(Mapper.Map<IEnumerable<PositionWithLink>>(fetchedObjects), 
                                                            Request.RequestUri.AbsolutePath);
            //Tell the user what can be done
            positions.Links[0].Method = "GET, POST";
            return positions;
        }

        // GET: api/Position/5
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get(int id)
        {
            var fetchedObject = m.Positions.GetById(id);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                // Create an object to be returned
                PositionLinked position = new PositionLinked(Mapper.Map<PositionWithLink>(fetchedObject), Request.RequestUri.AbsolutePath);

                // Tell the user what can be done with this item and collection
                position.Links[0].Method = "GET,DELETE";
                position.Links[1].Method = "GET,POST";
                // TODO maybe refactor this, use the API explorer to discover
                // (the same API explorer that's used in the HTTP OPTIONS handler

                // Add another link to tell the user that they can set the course value
                Link course = new Link()
                {
                    Rel = "self",
                    Title = "Set the course identifier",
                    Href = position.Links[0].Href,
                    Method = "PUT",
                    ContentType = "application/json"
                };
                position.Links.Add(course);

                // Add another link to tell the user that they can set the curriculum plan value
                Link curriculumPlan = new Link()
                {
                    Rel = "self",
                    Title = "Set the curriculum plan identifier",
                    Href = position.Links[0].Href,
                    Method = "PUT",
                    ContentType = "application/json"
                };
                position.Links.Add(curriculumPlan);

                // Return the results
                return Ok(position);
            }
        }

        // POST: api/Position
        [Authorize(Roles = "Faculty, Admin")]
        public IHttpActionResult Post([FromBody]PositionAdd newItem)
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
                var addedItem = m.Positions.AddNew(newItem);

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
                    PositionLinked emp = new PositionLinked(Mapper.Map<PositionWithLink>(addedItem), uri.AbsolutePath);

                    // Return the object
                    return Created(uri, emp);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Position/5
        [Authorize(Roles = "Faculty, Admin")]
        public IHttpActionResult Put(int id, [FromBody]PositionEdit editedItem)
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
                var changedItem = m.Positions.EditExisting(editedItem);
                if (changedItem == null)
                {
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    return Ok<PositionBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Position/5
        [Authorize(Roles = "Faculty, Admin")]
        public IHttpActionResult Delete(int id)
        {
            if (m.Programs.DeleteExisting(id))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest(string.Format("Unable to delete Position {0}", id));
            }
        }
    }
}

// ############################################################
// Resource model classes
namespace CoursePlanning.Controllers
{
    using System.ComponentModel.DataAnnotations;
    public class PositionList
    {
        public int Id { get; set; }
        public int Semester { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class PositionAdd
    {
        public PositionAdd()
        {
            this.IsVisible = true;
        }
        public int Semester { get; set; }
        public int DisplayOrder { get; set; }
        public Boolean IsVisible { get; set; }
    }

    public class PositionAddTemplate
    {
        public string Semester { get { return "simply the semester in which the course appears, required, number, up to 100 characters"; } }
        public string DisplayOrder { get { return "define a specific display order for the objects, number"; } }

    }

    public class PositionBase : PositionAdd
    {
        public int Id { get; set; }
        public int? AssoCourseId { get; set; }
        public CourseList AssoCourse { get; set; }
        public int? AssoCurriculumPlanId { get; set; }
        public CurriculumPlanList AssoCurriculumPlan { get; set; }
    }

    public class PositionEdit
    {
        public int Id { get; set; }
        public int Semester { get; set; }
        public int DisplayOrder { get; set; }

    }
    public class PositionWithLink : PositionBase
    {
        public Link Link { get; set; }
    }

    public class PositionLinked : LinkedItem<PositionWithLink>
    {
        // A refactoring experiment...
        public PositionLinked(PositionWithLink item, string absolutePath)
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

    public class PositionsLinked : LinkedCollection<PositionWithLink>
    {
        public PositionsLinked()
        {
            this.Template = new PositionAddTemplate();
        }

        //Newly add in week 8, one more constructor which takes two parameter
        // A refactoring experiment...
        public PositionsLinked(IEnumerable<PositionWithLink> collection, string absolutePath)
        {
            this.Template = new PositionAddTemplate();

            this.Collection = collection;

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath });

            // Add 'item' links for each item in the collection
            foreach (var item in this.Collection)
            {
                item.Link = new Link() { Rel = "item", Href = string.Format("{0}/{1}", absolutePath, item.Id) };
            }
        }

        public PositionAddTemplate Template { get; set; }
    }


}

// ############################################################
// Repository
namespace CoursePlanning.ServiceLayer
{
    using AutoMapper;
    using CoursePlanning.Models;
    using CoursePlanning.Controllers;

    public class Position_repo : Repository<Position>
    {
        public Position_repo(DataContext ds)  : base(ds) { }
        //Methods called by controllers

        //Get all
        public IEnumerable<PositionBase>GetAll() 
        {
            var fetchedObjects = _ds.Positions.Include("AssoCourse")
                                              .Include("AssoCurriculumPlan")
                                              .Where(ps => ps.IsVisible == true);
            return Mapper.Map<IEnumerable<PositionBase>>(fetchedObjects.OrderBy(ps => ps.DisplayOrder));
        }
        //Get one by identifier
        public PositionBase GetById(int id)
        {
            var fetchedObject = _ds.Positions.Include("AssoCourse")
                                              .Include("AssoCurriculumPlan")
                                              .SingleOrDefault(i=>i.Id == id && i.IsVisible == true);
            return (fetchedObject == null) ? null : Mapper.Map<PositionBase>(fetchedObject);
        }
        //Add new position
        public PositionBase AddNew(PositionAdd newItem)
        {
            var addedItem = RAdd(Mapper.Map<Position>(newItem));
            return Mapper.Map<PositionBase>(addedItem);
        }
        //Edit existing postion
        public PositionBase EditExisting(PositionEdit editedItem)
        {
            var updatedItem = REdit(editedItem);
            return Mapper.Map<PositionBase>(updatedItem);
        }
        //Make existing position invisible from the front end
        public bool DeleteExisting(int id)
        {
            var fetchedObject = _ds.Positions.Include("AssoCourse")
                                             .Include("AssoCurriculumPlan")
                                             .SingleOrDefault(i => i.Id == id && i.IsVisible == true);
            fetchedObject.IsVisible = !fetchedObject.IsVisible;
            var deletedItem = REdit(fetchedObject);
            return (deletedItem == null) ? false : true;
        }
    }

}
