using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordsInterface.Api
{
    public static class Converter
    {
        public static Chords.Site Convert(Data.Site nrdcSite)
        {
            Chords.Site chordsSite = new Chords.Site();

            chordsSite.ID = nrdcSite.ID;
            chordsSite.Name = nrdcSite.Alias;

            // etc, etc

            return chordsSite;
        }

        public static Chords.Measurement Convert(Data.Measurement nrdcMeasurement)
        {
            var chordsMeasurement = new Chords.Measurement();

            chordsMeasurement.TimeStamp = nrdcMeasurement.TimeStamp;
            chordsMeasurement.Value = nrdcMeasurement.Value;

            return chordsMeasurement;
        }
    }
}
