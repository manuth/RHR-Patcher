using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents an error thrown by the Project-Database.
    /// </summary>
    [DataContract]
    public class APIError
    {
        /// <summary>
        /// Gets or sets the identification-code of the error.
        /// </summary>
        [DataMember]
        public int Code { get; set; } = 0;

        /// <summary>
        /// Gets or sets the description of the error.
        /// </summary>
        [DataMember]
        public string Message { get; set; } = null;

        /// <summary>
        /// Gets or sets the inner exception of the error.
        /// </summary>
        [DataMember]
        public APIError InnerException { get; set; } = null;

        /// <summary>
        /// Gets or sets data which are related to the error.
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// Generates an exception depending on the code of the error.
        /// </summary>
        /// <returns>An exception which can be thrown.</returns>
        public Exception GetException()
        {
            Exception exception;
            switch (Code)
            {
                /* Argument-Errors */
                case 0x30000010:
                    exception = new ArgumentNullException(Message, InnerException?.GetException());
                    break;
                case 0x30000011:
                    exception = new ArgumentException(Message, InnerException?.GetException());
                    break;
                case 0x30000012:
                    exception = new ArgumentNullException(Data["PropertyName"], Message);
                    break;
                /* Query-Errors */
                case 0x30000030:
                    exception = new ConstraintException(Message, InnerException?.GetException());
                    break;
                case 0x30000031:
                /* PasswordUpdate-Errors */
                case 0x000000A0:
                    exception = new ObjectNotFoundException(Message, InnerException?.GetException());
                    break;
                /* Security-Errors */
                case 0x30000070:
                case 0x30000071:
                case 0x30000072:
                    exception = new SecurityException(Message, InnerException?.GetException());
                    break;
                /* Login-Errors */
                case 0x30000080:
                    exception = new InvalidCredentialException(Message, InnerException?.GetException());
                    break;
                /* Registration-Errors */
                case 0x30000090:
                    exception = new ArgumentException(Message, InnerException?.GetException());
                    break;
                default:
                    exception = new Exception(Message, InnerException?.GetException());
                    break;
            }

            foreach (KeyValuePair<string, string> entry in Data)
            {
                exception.Data.Add(entry.Key, entry.Value);
            }

            exception.Data.Add("ErrorCode", Code);
            return exception;
        }
    }
}
