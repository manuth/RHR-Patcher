using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a request which updates data on the Project-Database.
    /// </summary>
    /// <typeparam name="T">The type of the items to update.</typeparam>
    public class UpdateRequest<T> : TransferRequest<T> where T : IAPIObject
    {
        /// <summary>
        /// Gets the requestmethod to use.
        /// </summary>
        protected override HttpMethod RequestMethod
        {
            get
            {
                return HttpMethod.Put;
            }
        }

        /// <summary>
        /// Executes the request asynchronous.
        /// </summary>
        /// <param name="request">The request to execute.</param>
        /// <returns>The result of the request.</returns>
        protected override async Task<T> ExecuteAsync(HttpWebRequest request)
        {
            (await base.ExecuteAsync(request)).CloneTo(Data);
            return Data;
        }
    }
}
