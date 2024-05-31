using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        // This repository implements all methods from the IRepository, but with a 
        // specific data type of Product. It also adds in two additional methods, 
        // the update and save methods.

        // Updates the product records provided by the user.
        void Update(Product obj);
    }
}
