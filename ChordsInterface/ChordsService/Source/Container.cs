using System.Runtime.Serialization;

namespace ChordsInterface.Api
{
    [DataContract]
    public class Container<T>
    {
        [DataMember] public bool Success { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public T Object { get; set; }

        public Container(T obj = default(T), bool success = true, string message = "")
        {
            Object = obj;
            Success = success;
            Message = message;
        }

        public Container<T> Pass(T obj)
        {
            Object = obj;
            Success = true;
            Message = "";
            return this;
        }

        public Container<T> Fail(string message)
        {
            Object = default(T);
            Success = false;
            Message = message;
            return this;
        }
    }
}
