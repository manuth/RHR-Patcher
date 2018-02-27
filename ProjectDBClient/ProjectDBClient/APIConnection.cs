using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Provides the functionallity to read or write from or to the project-database.
    /// </summary>
    public partial class APIConnection : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="APIConnection"/> class.
        /// </summary>
        public APIConnection() : base()
        {
        }

        /// <summary>
        /// Gets or sets the data-source.
        /// </summary>
        public Uri DataSource { get; set; } = null;

        /// <summary>
        /// Gets or sets the username to access the Project-Database.
        /// </summary>
        public string Username { get; set; } = null;

        /// <summary>
        /// Gets or sets the password to access the Project-Database.
        /// </summary>
        public string Password { get; set; } = null;

        /// <summary>
        /// Gets the user which is currently logged in to the Project-Database.
        /// </summary>
        public User CurrentUser
        {
            get
            {
                return new AccessRequest<User>
                {
                    Connection = this,
                    Page = "API/VerifyLogin"
                }.Start();
            }
        }

        /// <summary>
        /// Creates a request which receives data from the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the items to return.</typeparam>
        /// <returns>A request which is able to access the items located on the Project-Database.</returns>
        public AccessRequest<List<T>> GetAccessRequest<T>() where T : IAPIObject, new()
        {
            return GetAccessRequest<T>(new T().Page);
        }

        /// <summary>
        /// Creates a request which receives data from the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the items to return.</typeparam>
        /// <param name="page">The page to send the request to.</param>
        /// <returns>A request which receives data from the Project-Database.</returns>
        public AccessRequest<List<T>> GetAccessRequest<T>(string page)
        {
            return GetAccessRequest<T>(page, new Dictionary<string, object>());
        }

        /// <summary>
        /// Creates a request which receives data from the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the items to return.</typeparam>
        /// <param name="page">The page to send the request to.</param>
        /// <param name="parameters">The parameters to send.</param>
        /// <returns>A request which receives data from the Project-Database.</returns>
        public AccessRequest<List<T>> GetAccessRequest<T>(string page, Dictionary<string, object> parameters)
        {
            return new AccessRequest<List<T>>
            {
                Connection = this,
                Page = page,
                Parameters = parameters
            };
        }

        /// <summary>
        /// Creates a request which updates data on the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to update.</typeparam>
        /// <returns>A request which updates data on the Project-Database.</returns>
        public UpdateRequest<T> GetUpdateRequest<T>() where T : IAPIObject, new()
        {
            return GetUpdateRequest(new T());
        }

        /// <summary>
        /// Creates a request which updates data on the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to update.</typeparam>
        /// <param name="item">The item to update.</param>
        /// <returns>A request which updates data on the Project-Database.</returns>
        public UpdateRequest<T> GetUpdateRequest<T>(T item) where T : IAPIObject
        {
            return GetUpdateRequest(item.Page, item);
        }

        /// <summary>
        /// Creates a request which updates data on the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to update.</typeparam>
        /// <param name="page">The page to send the request to.</param>
        /// <param name="item">The item to update.</param>
        /// <returns>A request which updates data on the Project-Database.</returns>
        public UpdateRequest<T> GetUpdateRequest<T>(string page, T item) where T : IAPIObject
        {
            return new UpdateRequest<T>
            {
                Connection = this,
                Page = page,
                Data = item
            };
        }

        /// <summary>
        /// Creates a request which creates data on the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to create.</typeparam>
        /// <returns>A request which creates data on the Project-Database.</returns>
        public CreationRequest<T> GetCreationRequest<T>() where T : IAPIObject, new()
        {
            return GetCreationRequest(new T());
        }

        /// <summary>
        /// Creates a request which creates data on the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to create.</typeparam>
        /// <param name="item">The item to create.</param>
        /// <returns>A request which creates data on the Project-Database.</returns>
        public CreationRequest<T> GetCreationRequest<T>(T item) where T : IAPIObject
        {
            return GetCreationRequest(item.Page, item);
        }

        /// <summary>
        /// Creates a request which creates data on the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to create.</typeparam>
        /// <param name="page">The page to save the item to.</param>
        /// <param name="item">The item to create.</param>
        /// <returns>A request which creates data on the Project-Database.</returns>
        public CreationRequest<T> GetCreationRequest<T>(string page, T item) where T : IAPIObject
        {
            return new CreationRequest<T>
            {
                Connection = this,
                Page = page,
                Data = item
            };
        }

        /// <summary>
        /// Gets a request which removes data from the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove.</typeparam>
        /// <returns>A request which removes data from the Project-Database.</returns>
        public DeletionRequest<T> GetDeletionRequest<T>() where T : IAPIObject, new()
        {
            return GetDeletionRequest(new T());
        }

        /// <summary>
        /// Gets a request which removes data from the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove.</typeparam>
        /// <param name="item">The item to remove.</param>
        /// <returns>A request which removes data from the Project-Database.</returns>
        public DeletionRequest<T> GetDeletionRequest<T>(T item) where T : IAPIObject
        {
            return GetDeletionRequest(item.Page, item);
        }

        /// <summary>
        /// Gets a request which removes data from the Project-Database.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove.</typeparam>
        /// <param name="page">The page to send the request to.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns>A request which removes data from the Project-Database.</returns>
        public DeletionRequest<T> GetDeletionRequest<T>(string page, T item) where T : IAPIObject
        {
            return new DeletionRequest<T>
            {
                Connection = this,
                Page = page,
                Data = item
            };
        }
    }
}
