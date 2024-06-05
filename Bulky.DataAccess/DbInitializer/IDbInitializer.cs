using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        // This method will be responsible for creating Admin users and 
        // roles for the web app. This is a function that is essential 
        // to be present as it is neat and can be used in the final code
        // that will be deployed.
        void Initialize();
    }
}
