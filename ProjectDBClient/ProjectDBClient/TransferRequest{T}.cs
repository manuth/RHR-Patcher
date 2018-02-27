using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a request which transfers data from or to the Project-Database.
    /// </summary>
    /// <typeparam name="T">The type of the data to send and to receive.</typeparam>
    public abstract class TransferRequest<T> : TransferRequest<T, T> where T : IAPIObject
    {
        /// <summary>
        /// Gets the requestmethod to use.
        /// </summary>
        protected abstract override HttpMethod RequestMethod { get; }
    }
}
