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
using System.IO; // I added to use functions to check valid filenames
using Screen = System.Windows.Forms.Screen; // I added to do screen capture work

namespace CaptureWhale
{
    // A screen capture utility with nice custom image saving capabilities.
    public partial class App : Application
    {
        public string strSaveDir { get; set; } = getDefaultFolder();
        public string strFilename { get; set; } = "Screenshot";

        // Setter method for strFilename which checks for a valid filename.
        // Returns false if a valid filename is not provided.
        public bool setFilename(String fileName)
        {
            // check filename against empty Strings and bad chars
            // we don't care/check here if file exists because we will
            // be appending a number to the filename to make it unique
            var isValid = !string.IsNullOrEmpty(fileName) &&
              (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);

            if (isValid) strFilename = fileName;
            else MessageBox.Show("The filename is invalid.\n\nBe sure there are no illegal characters in your chosen name.", "CaptureWhale - Invalid Filename");

            return isValid;
        }

        // save the current screenshot to a file sequentially named
        public void saveScreenshot()
        {
            Debug.WriteLine("Screenshot saved using filename: " + strFilename);

            /*
            // get the screenshot into a bitmap
            Bitmap g = copyScreen();

            // get the next filename, properly numbered in sequence
            fileName = getNextFilename();

            // save the screenshot
            g.Save(fileName, ImageFormat.Png);
            */
        }

        // searches the save directory for the highest numbered file (if any) to create a save filename.
        // Returns the save filename with either 001 appended or the next number based on existing files.
        private String getNextFilename()
        {
            String fileName = "asdf.png";
            
            return fileName;
        }

        // returns the current path to the save directory, getting default path if none exists
        public static String getDefaultFolder()
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
