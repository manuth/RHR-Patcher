using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a request which transfers data from or to the Project-Database.
    /// </summary>
    /// <typeparam name="TInput">The type of the data to send.</typeparam>
    /// <typeparam name="TOutput">The type of the data to receive.</typeparam>
    public abstract class TransferRequest<TInput, TOutput> : APIRequest<TOutput> where TInput : IAPIObject where TOutput : IAPIObject
    {
        /// <summary>
        /// Gets or sets the data to send.
        /// </summary>
        public TInput Data { get; set; }

        /// <summary>
        /// Gets the requestmethod to use.
        /// </summary>
        protected override abstract HttpMethod RequestMethod { get; }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="request">The request to execute.</param>
        /// <returns>The result of the request.</returns>
        protected override TOutput Execute(HttpWebRequest request)
        {
            return ExecuteAsync(request).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Executes the request asynchronous.
        /// </summary>
        /// <param name="request">The request to execute.</param>
        /// <returns>The result of the request.</returns>
        protected override async Task<TOutput> ExecuteAsync(HttpWebRequest request)
        {
            using (Stream stream = await request.GetRequestStreamAsync())
            {
                new DataContractJsonSerializer(typeof(TInput), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true }).WriteObject(stream, Data);
                return await base.ExecuteAsync(request);
            }
        }
    }
}
