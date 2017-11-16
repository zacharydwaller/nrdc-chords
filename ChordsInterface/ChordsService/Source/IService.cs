using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string CreateMeasurement(ChordsInterface.ChordsTypes.Measurement measurement);
    }
}
