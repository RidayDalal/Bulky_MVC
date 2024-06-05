using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T - Category

        // Get all the categories.
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        // Retrieve a single category record. Here, we would be using the FirstOrDefault() method which takes 
        // a LINQ operation as input. So, we are passing Expression<Func<T, bool>> as datatype.
        // Here, input to LINQ function is some datatype, but output is always a boolean.
        // Also, tracked = false is set so that no data extracted from the database is immediately updated
        // in the database while being used in the code.
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);

        // Create a category
        void Add(T entity);

        // Remove a single category.
        void Remove(T entity);

        // Remove a list/collection of categories.
        void RemoveRange(IEnumerable<T> entity);


    }
}