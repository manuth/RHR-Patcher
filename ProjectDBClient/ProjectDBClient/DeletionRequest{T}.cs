using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a request which removes data from the Project-Database.
    /// </summary>
    /// <typeparam name="T">The type of the data to delete.</typeparam>
    public class DeletionRequest<T> : TransferRequest<T> where T : IAPIObject
    {
        /// <summary>
        /// Gets the requestmethod to use.
        /// </summary>
        protected override HttpMethod RequestMethod
        {
            get
            {
                return HttpMethod.Delete;
            }
        }
    }
}
