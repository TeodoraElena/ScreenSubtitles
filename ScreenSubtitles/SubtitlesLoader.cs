using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSubtitles
{
    class SubtitlesLoader
    {
        SubtitlesParser parser;
        Subtitle currentSubtitle;
        List<Subtitle> subtitles;
        Thread thr;
        // the play time is negative if press back and positive if press next
        int status = 0;

        /**
         * Constructor
         */
        public SubtitlesLoader()
        {
            

        }

        public void LoadSubtitles(String filename) {
            parser = new SubtitlesParser(filename);
            subtitles = parser.GetAllSubtitles();
            //thr = new Thread();
        }

        public List<Subtitle> GetSubtitles()
        {
            return subtitles;
        }
        public void LoadNextSubtitle()
        {

        }

        public void LoadPreviousSubtitle()
        {

        }
        
    }
}
