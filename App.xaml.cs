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
        // Class members that hold information about how to save our screenshots
        public string strSaveDir { get; set; } = getDefaultFolder();
        public string strFilename { get; set; } = "Screenshot";
        public string strExtension { get; set; } = "png";

        // Setter method for strFilename which checks for a valid filename.
        // Returns false if a valid filename is not provided.
        public bool setFilename(String fileName)
        {
            // trim whitespace
            fileName = fileName.Trim();

            // check filename against empty Strings and bad chars
            // we don't care/check here if file exists because we will
            // be appending a number to the filename to make it unique
            var isValid = !string.IsNullOrEmpty(fileName) &&
              (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);

            if (isValid) strFilename = fileName;
            else MessageBox.Show("That filename cannot be used.\n\nBe sure there are no illegal characters in your chosen name.", "CaptureWhale - Invalid Filename");

            return isValid;
        }

        // save the current screenshot to a file sequentially named
        public void saveScreenshot()
        {
            // Append filename with appropriate #
            String strNewPathFile = strSaveDir + "\\" + getNextFilename() + "." + strExtension;

            // get the screenshot into a bitmap
            Bitmap g = copyScreen();

            // save the screenshot
            try
            {
                g.Save(strNewPathFile, ImageFormat.Png);
            } catch (Exception e)
            {
                MessageBox.Show("There was a problem trying to save your screnshot.\n\nBe sure you have permission to save to your chosen folder and that there is space on the drive.", "CaptureWhale - Error Saving Image");
            }

            Debug.WriteLine("File saved as: " + strNewPathFile);
        }

        // Searches the save directory for the highest numbered file (if any) to create a save filename.
        // Returns the save filename with either _001 appended or the next number based on existing files.
        private String getNextFilename()
        {
            // get a file list from the chosen save dir that match our chosen file extension
            DirectoryInfo d = new DirectoryInfo(strSaveDir);
            FileInfo[] files = d.GetFiles("*." + strExtension);

            // collects all files that start with our chosen filename and strips file extension
            var similiarFiles = new List<String>();
            String strTemp = "";
            foreach (FileInfo file in files)
            {
                if (file.Name.StartsWith(strFilename, StringComparison.OrdinalIgnoreCase))
                {                    
                    strTemp = file.Name.Remove (file.Name.Length - (strExtension.Length + 1));
                    similiarFiles.Add( strTemp );
                }
            }

            // Iterate through the filenames to find the highest numbered file
            // Checks that files are our chosen filename + _### in length only
            // Checks that the last four characters are an underscore plus a valid three digit number
            // Stores highest encountered result in intHighest
            int intHighest = 0;
            foreach (String file in similiarFiles)
            {
                if (file.Length == strFilename.Length + 4)
                {
                    if (file.LastIndexOf('_') == file.Length - 4)
                    {
                        strTemp = file.Substring(file.Length - 3);
                        int intTemp = 0;
                        if (int.TryParse(strTemp, out intTemp))
                        {
                            if (intTemp > intHighest) intHighest = intTemp;
                        } 
                    }
                }
            }

            // increment our file number if necessary and format the new filename
            // Note: if no list of numbered files was found default to 001.
            String nextFilename = strFilename + "_001";
            if (intHighest > 0)
            {
                intHighest++;
                String strNextNum = intHighest.ToString().PadLeft(3, '0');
                nextFilename = strFilename + "_" + strNextNum;
            }

            return nextFilename;
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
