using System;
using System.Collections.Generic;
using System.Web.Http;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;

namespace NCInterface.Controllers
{ 
    //Creates an API to return version number
    public class NCInterfaceController : ApiController
    {
        // GET: NCInterface
        public Container<string> Get()
        {
            return new Container<string>("NRDC-CHORDS Interface Service. " + Utilities.Version.GetString(), true);
        }
    }
}
