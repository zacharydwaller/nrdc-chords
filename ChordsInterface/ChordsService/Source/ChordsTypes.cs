using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChordsInterface.Chords
{
    [DataContract]
    public class Measurement
    {
        [DataMember]
        public uint Instrument { get; set; }

        [DataMember]
        public int Value { get; set; }
    }
}
