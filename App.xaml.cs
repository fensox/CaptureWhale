using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Screen = System.Windows.Forms.Screen;

namespace CaptureWhale
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private int imageCounter = 0;

        // save the current screenshot to a file sequentially named
        public void saveScreenshot()
        {
            //Create a new bitmap.
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            //Bitmap screenshot = new Bitmap( w, h, PixelFormat.Format32bppArgb);
            Bitmap screenshot = new Bitmap(w, h);

            // Create a graphics object from the bitmap.
            var g = Graphics.FromImage(screenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                Screen.PrimaryScreen.Bounds.Y,
                                0,
                                0,
                                Screen.PrimaryScreen.Bounds.Size,
                                CopyPixelOperation.SourceCopy);

            imageCounter++;
            screenshot.Save("C:\\Users\\fenso\\OneDrive\\Pictures\\Screenshots\\Screenshot" + imageCounter + ".png", ImageFormat.Png);
        }
    }
}
