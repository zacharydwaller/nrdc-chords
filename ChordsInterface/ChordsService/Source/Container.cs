using System.Runtime.Serialization;

namespace ChordsInterface.Api
{
    /// <summary>
    ///     Generic container type for use with web service and API. Contains a success flag, object, and a message string for error reporting.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class Container<T>
    {
        [DataMember] public bool Success { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public T Object { get; set; }

        /// <summary>
        ///     Parameterised constructor. Can provide all or none of the class' properties.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public Container(T obj = default(T), bool success = true, string message = "")
        {
            Object = obj;
            Success = success;
            Message = message;
        }

        /// <summary>
        ///     Constructor used to build a Container that only contains a fail message. Object set to default value, Success set to false.
        /// </summary>
        /// <param name="errorMessage"></param>
        public Container(string errorMessage)
        {
            Object = default(T);
            Success = false;
            Message = errorMessage;
        }

        /// <summary>
        ///     Shorthand for setting the Container to a success state with the provided Object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Container<T> Pass(T obj)
        {
            Object = obj;
            Success = true;
            Message = "";
            return this;
        }

        /// <summary>
        ///     Shorthand for setting the Container to a fail state with the provided error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Container<T> Fail(string errorMessage)
        {
            Object = default(T);
            Success = false;
            Message = errorMessage;
            return this;
        }
    }
}
