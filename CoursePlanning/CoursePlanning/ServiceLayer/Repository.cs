using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using CoursePlanning.Models;
using System.Data.Entity;
using System.Linq.Expressions;

namespace CoursePlanning.ServiceLayer
{
    public abstract class Repository<T> where T : class
    {
        // Fields that are the backing store for properties
        protected DataContext _ds;
        protected readonly IDbSet<T> _dbset;

        // Default constructor
        public Repository(DataContext ds)
        {
            _ds = ds;
            _dbset = ds.Set<T>();
        }

        // Save changes
        public int SaveChanges()
        {
            return _ds.SaveChanges();
        }

        // Standard data handling methods
        // Notice the "protected" visibility

        protected IEnumerable<T> RGetAll()
        {
            return _dbset.AsEnumerable<T>();
        }

        protected T RGetById(int id)
        {
            return _dbset.Find(id);
        }

        protected IEnumerable<T> RGetAllFiltered(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        protected T RAdd(T item)
        {
            // Ensure that we can continue
            if (item == null)
            {
                return null;
            }

            // Attempt to add the new item
            T addedItem = _dbset.Add(item);
            SaveChanges();

            return addedItem;
        }

        protected T REdit(object item)
        {
            // Ensure that we can continue
            if (item == null)
            {
                return null;
            }

            // The 'item' argument is declared as an object type in this generic repository
            // However, the passed-in 'item' will be a specific resource model type
            // We can look for a specific named property in the object by using 'reflection'
            // The following statement looks inside 'item' for a property named 'Id'
            // The return value is an object type, and we can use that in the Find() method

            // Attempt to find the stored item using its identifier
            var storedItem = _dbset.Find(item.GetType().GetProperty("Id").GetValue(item));

            // Ensure that we can continue
            if (storedItem == null)
            {
                return null;
            }
            else
            {
                // Update the stored item's property values
                // SetValues() takes an object of any type, which is ideal
                _ds.Entry(storedItem).CurrentValues.SetValues(item);
                SaveChanges();
            }

            return storedItem;
        }

        public virtual void RDelete(object id)
        {
            // Attempt to find the item to be deleted
            var itemToDelete = _dbset.Find(id);

            if (itemToDelete != null)
            {
                _dbset.Remove(itemToDelete);
                SaveChanges();
            }
        }

    }

}
