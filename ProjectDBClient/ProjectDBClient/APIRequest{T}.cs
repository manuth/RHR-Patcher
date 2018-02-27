using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ManuTh.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Provides the functionallity to communicate with the Project-Database.
    /// </summary>
    /// <typeparam name="T">The type of the data to work with.</typeparam>
    public class APIRequest<T>
    {
        /// <summary>
        /// Gets or sets the page to send the request to.
        /// </summary>
        public virtual string Page { get; set; } = null;

        /// <summary>
        /// Gets or sets the parameters to send.
        /// </summary>
        public virtual Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the connection to the Project-Database.
        /// </summary>
        public APIConnection Connection { get; set; } = new APIConnection();

        /// <summary>
        /// Gets the web-request which is to be sent to the Project-Database.
        /// </summary>
        protected virtual HttpWebRequest Request
        {
            get
            {
                UriBuilder uriBuilder = new UriBuilder(Connection.DataSource);
                NameValueCollection parameters = HttpUtility.ParseQueryString(Page);
                foreach (KeyValuePair<string, object> parameter in Parameters)
                {
                    parameters.Add(parameter.Key, parameter.Value.ToString());
                }

                uriBuilder.Query = parameters.ToString();
                HttpWebRequest request = WebRequest.Create(uriBuilder.Uri) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Connection.Username + ":" + Connection.Password)));

                return request;
            }
        }

        /// <summary>
        /// Gets the requestmethod to use.
        /// </summary>
        protected virtual HttpMethod RequestMethod
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the serializer to use.
        /// </summary>
        protected virtual DataContractJsonSerializer Serializer
        {
            get
            {
                return new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            }
        }

        /// <summary>
        /// Starts the request.
        /// </summary>
        /// <returns>The result of the request.</returns>
        public T Start()
        {
            return StartAsync().RunSync();
        }

        /// <summary>
        /// Starts the request asynchronous.
        /// </summary>
        /// <returns>The result of the request.</returns>
        public async Task<T> StartAsync()
        {
            HttpWebRequest request = Request;
            if (RequestMethod != null)
            {
                request.Method = RequestMethod.Method;
            }

            return await ExecuteAsync(request);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="request">The request to execute.</param>
        /// <returns>The result of the request.</returns>
        protected virtual T Execute(HttpWebRequest request)
        {
            return ExecuteAsync(request).RunSync();
        }

        /// <summary>
        /// Executes the request asynchronous.
        /// </summary>
        /// <param name="request">The request to execute.</param>
        /// <returns>The result of the request.</returns>
        protected virtual async Task<T> ExecuteAsync(HttpWebRequest request)
        {
            try
            {
                T result = default(T);
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                using (Stream responseStream = response.GetResponseStream())
                {
                    result = (T)Serializer.ReadObject(responseStream);
                }

                return result;
            }
            catch (WebException e)
            {
                Exception exception = e;
                using (HttpWebResponse response = e.Response as HttpWebResponse)
                {
                    if (
                        response.StatusCode == HttpStatusCode.InternalServerError &&
                        response.ContentType == "application/json")
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            exception = (
                                    new DataContractJsonSerializer(
                                        typeof(APIError),
                                        new DataContractJsonSerializerSettings
                                        {
                                            UseSimpleDictionaryFormat = true
                                        }).ReadObject(responseStream) as APIError).GetException();
                        }
                    }
                }

                throw exception;
            }
        }
    }
}
