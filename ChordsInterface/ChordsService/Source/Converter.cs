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
            Chords.Site chordsSite = new Chords.Site();

            chordsSite.ID = dataSite.ID;
            chordsSite.Name = dataSite.Alias;

            // etc, etc

            return chordsSite;
        }

        public static Chords.Measurement Convert(Data.Measurement dataMeasurement)
        {
            var chordsMeasurement = new Chords.Measurement();

            chordsMeasurement.TimeStamp = dataMeasurement.TimeStamp;
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

        }
    }
}
