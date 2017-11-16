using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordsInterface.Api
{
    public static class Converter
    {
        public static Chords.Site Convert(Nrdc.Site nrdcSite)
        {
            Chords.Site chordsSite = new Chords.Site();

            chordsSite.ID = nrdcSite.ID;
            chordsSite.Name = nrdcSite.Alias;

            // etc, etc

            return chordsSite;
        }
    }
}
