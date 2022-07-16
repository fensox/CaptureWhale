using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;           // i added
using System.Diagnostics;               // I added for Debug.WriteLine functionality
using System.Runtime.InteropServices;   // I added for global hot keys hook
using WinForms = System.Windows.Forms;  // I added for folder chooser dialog

namespace CaptureWhale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // setup our P/Invoke functions to register our global hotkey
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // variables to handle global hotkey
        private IntPtr windowHandle;            // Our window handle
        private HwndSource? windowSource;       // Allows Win32 window functionality for our WPF window
        private const int HOTKEY_ID = 9000;     // An ID for our registered global hotkey
        private const uint VK_SNAPSHOT = 0x2C;  // The PrtScn hotkey we want to register and look for presses
        
        public MainWindow()
        {
            InitializeComponent();

            // initialize out UI elements with default content
            txtSaveDir.Text = App.getDefaultFolder();
            txtFilename.Text = ((App)Application.Current).strFilename;
        }

        // Override the OnSourceInitialized method so we can register our hotkey with the OS
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // get our windows handle and add our event listener delegate method
            windowHandle = new WindowInteropHelper(this).Handle;
            windowSource = HwndSource.FromHwnd(windowHandle);
            windowSource.AddHook(WindowListener);

            RegisterHotKey(windowHandle, HOTKEY_ID, 0, VK_SNAPSHOT); // P/Invoke function call to register PrtScn
        }

        // Event handler delegate methos set in OnSourceInitialized to handle key press messages from the OS
        private IntPtr WindowListener(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == VK_SNAPSHOT)
                            {
                                validateDataAndSave();
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        // Override the OnClosed method so we can unregister our global hotkey
        protected override void OnClosed(EventArgs e)
        {
            if (windowSource is not null) windowSource.RemoveHook(WindowListener);
            UnregisterHotKey(windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }

        // Handle presses to our screenshot button in the UI
        private void btnSaveScreenshot(object sender, RoutedEventArgs e)
        {
            validateDataAndSave();
        }

        // Open a folder choosing dialog to pick a save folder
        private void btnSaveDir(object sender, RoutedEventArgs e)
        { 
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = ((App)Application.Current).strSaveDir;
            WinForms.DialogResult result = folderDialog.ShowDialog();

            if (result == WinForms.DialogResult.OK)
            {
                ((App)Application.Current).strSaveDir = folderDialog.SelectedPath;
                txtSaveDir.Text = folderDialog.SelectedPath;
            }
        }

        // Initiates the process of saving a screenshot by first checking for valid data
        private void validateDataAndSave()
        {
            App mainApp = (App)Application.Current;
            if (mainApp.setFilename(txtFilename.Text))
            {
                mainApp.saveScreenshot();
            }
        }
    }
}
