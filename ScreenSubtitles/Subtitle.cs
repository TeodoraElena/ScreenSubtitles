using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSubtitles
{
    class Subtitle
    {
        private int subtitleNo = 0;
        private String subtitle;
        private String timeStamp;
        private int showTime;

        public Subtitle()
        {
        }
        public Subtitle(int no, String sub, String timestamp, int showTime)
        {
            this.subtitleNo = no;
            this.subtitle = sub;
            this.timeStamp = timestamp;
            this.showTime = showTime;
        }


        public void SetSubtitleNo(int subtitleNo)
        {
            this.subtitleNo = subtitleNo;
        }

        public int GetSubtitleNo()
        {
            return subtitleNo;
        }

        public void SetTime(int showTime)
        {
            this.showTime = showTime;
        }

        public int GetTime()
        {
            // calculate show time from timestamp string

            return showTime;
        }

        public void SetSubtitle(String nextsubtitle)
        {
            this.subtitle = nextsubtitle;
        }

        public String GetSubtitle()
        {
            return subtitle;
        }

        public void SetTimeStamp(String timeStamp)
        {
            this.timeStamp = timeStamp;
        }

        public String GetTimeStamp()
        {
            return timeStamp;
        }
    }
}
