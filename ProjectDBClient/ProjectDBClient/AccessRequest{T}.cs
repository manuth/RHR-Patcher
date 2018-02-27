using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a request which receives data from the Project-Database.
    /// </summary>
    /// <typeparam name="T">The type of the data to work with.</typeparam>
    public class AccessRequest<T> : APIRequest<T>
    {
    }
}
