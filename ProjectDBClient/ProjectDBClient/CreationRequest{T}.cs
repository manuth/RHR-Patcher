using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a request which creates data on the Project-Database.
    /// </summary>
    /// <typeparam name="T">The type of the data to create.</typeparam>
    public class CreationRequest<T> : TransferRequest<T> where T : IAPIObject
    {
        /// <summary>
        /// Gets the requestmethod to use.
        /// </summary>
        protected override HttpMethod RequestMethod
        {
            get
            {
                return HttpMethod.Post;
            }
        }
    }
}
