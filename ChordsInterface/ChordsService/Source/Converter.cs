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
            var cSite = new Chords.Site
            {
                Name = String.Copy(dataSite.Alias),
                ID = dataSite.ID,
                Latitude = dataSite.Latitude,
                Longitude = dataSite.Longitude,
                Elevation = dataSite.Elevation,
                Description = String.Copy(dataSite.Name)
            };

            return cSite;
        }

        public static Chords.Measurement Convert(Data.Measurement dataMeasurement)
        {
            var chordsMeasurement = new Chords.Measurement
            {
                TimeStamp = string.Copy(dataMeasurement.TimeStamp),
                Value = dataMeasurement.Value
            };

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
            var cSite = new Chords.Site
            {
                Name = infSite.Alias,
                ID = infSite.ID,
                Latitude = infSite.Latitude,
                Longitude = infSite.Longitude,
                Elevation = infSite.Elevation,
                Description = String.Copy(infSite.Notes)
            };

            return cSite;
        }
    }
}
