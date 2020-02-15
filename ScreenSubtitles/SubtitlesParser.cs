using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSubtitles
{
    class SubtitlesParser
    {
        private String fileName;
        private List<Subtitle> subtitles;
        private Boolean interrupt = false;

        public SubtitlesParser(String filename) {
            this.fileName = filename;
      
        }

        public List<Subtitle> GetAllSubtitles()
        {
             try 
             {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(this.fileName)) 
                {
                    string line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    int lineCount = 0;
                    int no = 0;
                    string timestamp = "";
                    string textInfo = "";
                    subtitles = new List<Subtitle>();

                    // if is null then add next
                    while ((line = sr.ReadLine()) != null) 
                    {
                        if (line == "")
                        {
                            if (lineCount != 0) {
                                if (no > 0 && timestamp != "" && textInfo != "") {
                                    subtitles.Add(new Subtitle(no, textInfo, timestamp, CalculateTimeStamp(timestamp)));
                                }
                            }
                            // next subtitle;
                            lineCount = 0;
                        }
                        else
                        {
                            lineCount++;
                        }

                        //Console.WriteLine(line);
                        switch (lineCount)
                        {
                            case 1:
                                no = Int32.Parse(line);
                                break;
                            case 2:
                                timestamp = line;
                                break;
                            case 3:
                                textInfo = line;
                                break;
                            case 4:
                                textInfo = textInfo + "\n" + line;
                                break;
                            default:
                                //new line
                                lineCount = 0;
                                break;
                        }
                    }
                }
             }
             catch (Exception e) 
             {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
             }
            return subtitles;
        }

        // every subtitle block is separeted by space
        public int CalculateTimeStamp(String timestamp)
        {
            // no is ok, info is ok
            // the timestamp neds to be calculated and converted :D
            // regexp as timestamp 00:01:13,210 --> 00:01:14,680
            // take ,xxx and subtract + seconds subtract*100
            // if minutes difference then subtract seconds from first and add second*100
            // if hour difference then minutes from first and add minute*600
            string pattern = @"\ba\w*\b";
            string input = "An extraordinary day dawns with each new day.";
            Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

            int time = 0;
            int mseconds1 = Int32.Parse(timestamp.Substring(9, 3));
            int mseconds2 = Int32.Parse(timestamp.Substring(26, 3));

            int seconds1 = Int32.Parse(timestamp.Substring(6, 2));
            int seconds2 = Int32.Parse(timestamp.Substring(23, 2));

            int minutes1 = Int32.Parse(timestamp.Substring(3, 2));
            int minutes2 = Int32.Parse(timestamp.Substring(20, 2));

            //int hour1 = Int32.Parse(timestamp.Substring(0, 2));
            //int hour2 = Int32.Parse(timestamp.Substring(17, 2));

            if (seconds2 - seconds1 < 0)
            {
                time = (60 - seconds1) + seconds2;
                time = time * 1000;

                if (mseconds2 - mseconds1 < 0)
                {
                    time = time + ((1000 - mseconds1) + mseconds2);
                }
                else {
                    time = time + (mseconds2 - mseconds1);
                }

            }
            else {
                time = (seconds2 - seconds1);
                time = time * 1000;

                if (mseconds2 - mseconds1 < 0)
                {
                    time = time + ((1000 - mseconds1) + mseconds2);
                }
                else
                {
                    time = time + (mseconds2 - mseconds1);
                }
            }

            if (time > 1000)
            {
                return time - 500;
            }

            if (time > 500)
            {
                return time - 200;
            }
            return time;
        }
    }
}
