using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordsInterface.Api
{
    public static class Converter
    {
        /** NRDC Data Types **/

        public static Chords.Site Convert(Data.Site dataSite)
        {
            var cSite = new Chords.Site();

            cSite.Name = String.Copy(dataSite.Alias);
            cSite.ID = dataSite.ID;
            cSite.Latitude = dataSite.Latitude;
            cSite.Longitude = dataSite.Longitude;
            cSite.Elevation = dataSite.Elevation;
            cSite.Description = String.Copy(dataSite.Name);

            return cSite;
        }

        public static Chords.Measurement Convert(Data.Measurement dataMeasurement)
        {
            var chordsMeasurement = new Chords.Measurement();

            chordsMeasurement.TimeStamp = string.Copy(dataMeasurement.TimeStamp);
            chordsMeasurement.Value = dataMeasurement.Value;

            return chordsMeasurement;
        }

        /** NRDC Infrastructure Types **/

        public static Chords.SiteList Convert(Infrastructure.SiteList infList)
        {
            Chords.SiteList chordsList = new Chords.SiteList();

            foreach (var nSite in infList.Data)
            {
                var cSite = Convert(nSite);
                chordsList.Data.Add(cSite);
            }

            return chordsList;
        }

        public static Chords.Site Convert(Infrastructure.Site infSite)
        {
            var cSite = new Chords.Site();

            cSite.Name = string.Copy(infSite.Alias);
            cSite.ID = infSite.ID;
            cSite.Latitude = infSite.Latitude;
            cSite.Longitude = infSite.Longitude;
            cSite.Elevation = infSite.Elevation;
            cSite.Description = String.Copy(infSite.Notes);

            return cSite;
        }
    }
}
