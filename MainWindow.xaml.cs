using HRSH_Transpera_Updater.Tools;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.IO;

namespace HRSH_Transpera_Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWind_Loaded(object sender, RoutedEventArgs e)
        {
            btnFinish.IsEnabled = false;
            //btnUpdate.IsEnabled = false;

            lblStatus.Content = "Checking for updates...";

            IniFile config = new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\HRSH\Transpera\config.ini");
            string currentVersion = config.Read("Version", "Settings");

            WebClient client = new WebClient();
            string latestVersion = client.DownloadString("https://an0maly.blob.core.windows.net/transpera/version.txt");

            if(currentVersion != latestVersion)
            {
                lblStatus.Content = "Updates Found!";
                btnUpdate.IsEnabled = true;
            }
            else if(currentVersion == latestVersion)
            {
                lblStatus.Content = "No updates found.";
                btnFinish.IsEnabled = true;
            }
            else
            {
                lblStatus.Content = "Error! Reinstall.";
                btnFinish.IsEnabled = true;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            btnUpdate.IsEnabled = false;

            foreach (var process in Process.GetProcessesByName("HRSH-Transpera"))
            {
                process.Kill();
            }

            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");

            WebClient client = new WebClient();
            client.DownloadFile("https://an0maly.blob.core.windows.net/transpera/HRSH-Transpera.exe", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");

            lblStatus.Content = "Updated!";
            btnFinish.IsEnabled = true;
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
