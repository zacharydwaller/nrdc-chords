using ChordsInterface.Nrdc;

namespace ChordsInterface.Api
{
    public abstract class Container
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Object { get; set; }
    }

    public class NrdcContainer : Container
    {
        new public NrdcType Object { get; set; }

        public NrdcContainer(NrdcType obj, bool success = true, string message = default(string))
        {
            Object = obj;
            Success = success;
            Message = message;
        }
    }

    public class ChordsContainer : Container
    {
        new public Chords.ChordsType Object { get; set; }

        public ChordsContainer(Chords.ChordsType obj, bool success = true, string message = default(string))
        {
            Object = obj;
            Success = success;
            Message = message;
        }
    }
}
