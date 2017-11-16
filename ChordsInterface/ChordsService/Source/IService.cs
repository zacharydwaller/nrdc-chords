﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChordsInterface.Service
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string CreateMeasurement(Chords.Measurement measurement);

        [OperationContract]
        string PullSite(int siteID);
    }
}