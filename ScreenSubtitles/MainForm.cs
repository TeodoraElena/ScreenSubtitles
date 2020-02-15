using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSubtitles
{
    public partial class MainForm : CustomTransparentForm
    {
        private delegate void SafeCallDelegate(string text);
        SubtitlesLoader loader;
        Subtitle currentSubtitle;
        List<Subtitle> subtitles;
        String filename = "C:\\Users\\TEO\\Downloads\\SUBDL.com__love.clinic1539341\\Love.Clinic.2014.UNCUT-HDRip.english.srt";
        Boolean pause = false;
        int skip = 0;

        public MainForm()
        {
            InitializeComponent();
            loader = new SubtitlesLoader();
            // test if thread works
            // pass file and get subtitles
            loader.LoadSubtitles(filename);
            subtitles = loader.GetSubtitles();

            Thread thread = new Thread(() => TestThread(this.subtileLabel));
            thread.Start();
            thread.IsBackground = true;

            Console.WriteLine("Main Thread Ends!!");
        }

        private void WriteTextSafe(string text)
        {
            if (subtileLabel.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                subtileLabel.Invoke(d, new object[] { text });
            }
            else
            {
                subtileLabel.Text = text;
            }
        }


        public void TestThread(Label label)
        {
            for(int i = 246; i < subtitles.Count; i++)
            {
                if (skip == 0)
                {
                    // show subtitles
                    WriteTextSafe(subtitles[i].GetSubtitle());
                    Thread.Sleep(subtitles[i].GetTime());
                    Console.WriteLine("No:" + subtitles[i].GetSubtitleNo() + "Time: " + subtitles[i].GetTime());
                    // show the pauses too
                    if (i != 0 || i != subtitles.Count - 1)
                    {
                        WriteTextSafe("");
                        Thread.Sleep(CalculatePause(subtitles[i].GetTimeStamp(), subtitles[i + 1].GetTimeStamp()));
                        Console.WriteLine("No:" + subtitles[i].GetSubtitleNo() +
                            "Pause: " + CalculatePause(subtitles[i].GetTimeStamp(), subtitles[i+1].GetTimeStamp()));
                    }
                }
                else {
                    skip--;
                }
            }
        }

        public int CalculatePause(String timestamp1, String timestamp2)
        {
            // regexp as timestamp1 00:01:13,210 --> 00:01:14,680
            // regexp as timestamp2 00:01:13,210 --> 00:01:14,680
            int time = 0;
            int mseconds2 = Int32.Parse(timestamp2.Substring(9, 3));
            int mseconds1 = Int32.Parse(timestamp1.Substring(26, 3));

            int seconds2 = Int32.Parse(timestamp2.Substring(6, 2));
            int seconds1 = Int32.Parse(timestamp1.Substring(23, 2));

            int minutes2 = Int32.Parse(timestamp2.Substring(3, 2));
            int minutes1 = Int32.Parse(timestamp1.Substring(20, 2));

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
                else
                {
                    time = time + (mseconds2 - mseconds1);
                }

            }
            else
            {
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
            if (time > 1000) {
                return time - 500;
            }

            if (time > 500)
            {
                return time - 200;
            }

            return time;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                Console.WriteLine(file);
                loader.LoadSubtitles(file);
            }
        }
        
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //MessageBox.Show(e.KeyChar.ToString());
            switch (e.KeyChar.ToString())
            {
                case "q":
                    // quit
                    this.Close();
                    break;
                case "+":
                    // size
                    this.subtileLabel.Font = new Font(this.subtileLabel.Font.OriginalFontName,
                        this.subtileLabel.Font.Size + 1, FontStyle.Bold); 
                    break;
                case "-":
                    // size
                    this.subtileLabel.Font = new Font(this.subtileLabel.Font.OriginalFontName,
                        this.subtileLabel.Font.Size - 1, FontStyle.Bold);
                    break;
                case "s":
                    // style
                    this.subtileLabel.Font = new Font(this.subtileLabel.Font.OriginalFontName,
                        this.subtileLabel.Font.Size - 1, FontStyle.Bold);
                    break;
                case "p":
                    if (pause)
                    {
                        pause = false;
                    }
                    else
                    {
                        pause = true;
                    }
                    break;
                case "k":
                    skip++;
                    break;
            }

        }
    }
}
