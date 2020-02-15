using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSubtitles
{
    class RoundedButton : Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            GraphicsPath grPath = new GraphicsPath();

            using (var brush = new LinearGradientBrush
               (DisplayRectangle, Color.IndianRed, Color.IndianRed, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, DisplayRectangle);
            }

            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(e);
        }
    }
}
