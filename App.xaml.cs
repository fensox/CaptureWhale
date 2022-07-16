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
        // save the current screenshot to a file sequentially named
        public void saveScreenshot()
        {
            // get the screenshot into a bitmap
            Bitmap g = copyScreen();

            // get the next filename, properly numbered in sequence
            String fileName = getNextFilename();

            // save the screenshot
            g.Save(fileName, ImageFormat.Png);
        }

        // searches the save directory for the highest numbered file (if any) to create a save filename.
        // Returns the save filename with either 001 appeded or the next number based on existing files.
        private String getNextFilename()
        {
            String fileName = "asdf";

            return fileName;
        }

        // returns the current path to the save directory, getting default path if none exists
        public String getFilePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        // Takes a screen capture and returns the capture as a Bitmap
        private Bitmap copyScreen()
        {
            // Create a new bitmap
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            Bitmap screenshot = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            Graphics g = Graphics.FromImage(screenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                Screen.PrimaryScreen.Bounds.Y,
                                0,
                                0,
                                Screen.PrimaryScreen.Bounds.Size,
                                CopyPixelOperation.SourceCopy);

            return screenshot;
        }
    }
}
