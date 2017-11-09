using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChordsService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string CreateMeasurement(Measurement measurement);
    }

    [DataContract]
    public class Measurement
    {
        [DataMember]
        public uint Instrument { get; set; }

        [DataMember]
        public int Value { get; set; }
    }
}
