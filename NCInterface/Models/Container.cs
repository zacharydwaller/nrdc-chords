using System.Collections.Generic;

namespace NCInterface.Structures
{
    public class Container
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        /// <summary>
        ///     Success constructor. Sets Success to true with an empty Message string.
        /// </summary>
        public Container()
        {
            Success = true;
            Message = "";
        }

        /// <summary>
        ///     Failure/Error constructor. Must provide an error message.
        ///     Success set to false.
        /// </summary>
        /// <param name="errorMessage"></param>
        public Container(string errorMessage)
        { 
            Success = false;
            Message = errorMessage;
        }

        /// <summary>
        ///     Contructor to set a flag and message;
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public Container(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    /// <summary>
    ///     Generic container type for use with both our API and the NRDC APIs.
    ///     Contains a success flag, data list, and a message string for error reporting.
    /// </summary>
    public class Container<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<T> Data { get; set; }

        /// <summary>
        /// Default constructor. Should really only be used for serialization purposes.
        /// </summary>
        public Container() { }

        /// <summary>
        ///     Default success/list constructor.
        ///     Can provide all or none of the class' properties.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public Container(IList<T> data, bool success = true, string message = "")
        {
            Data = data;
            Success = success;
            Message = message;
        }

        /// <summary>
        ///     Default success/single-item constructor.
        ///     Can provide all or none of the class' properties.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public Container(T obj, bool success = true, string message = "")
        {
            Data = new List<T>
            {
                obj
            };
            Success = success;
            Message = message;
        }

        /// <summary>
        ///     Failure/Error constructor. Must provide an error message.
        ///     Success set to false, Data set to null.
        /// </summary>
        /// <param name="errorMessage"></param>
        public Container(string errorMessage)
        {
            Data = null;
            Success = false;
            Message = errorMessage;
        }
    }
}