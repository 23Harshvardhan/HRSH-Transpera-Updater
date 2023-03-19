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
        WebClient updater = new WebClient();

        public MainWindow()
        {
            InitializeComponent();

            updater.DownloadProgressChanged += Updater_DownloadProgressChanged;
            updater.DownloadDataCompleted += Updater_DownloadDataCompleted;
        }

        #region ========== Updater Web Client Events ==========

        private void Updater_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            MessageBox.Show("Client successfully updated to latest version.", "Happy Hacking!", MessageBoxButton.OK);
            this.Close();
        }

        private void Updater_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progUpdater.Value = e.ProgressPercentage;
            lblPercentage.Content = e.ProgressPercentage.ToString() + "%";
        }

        #endregion

        private void mainWind_Loaded(object sender, RoutedEventArgs e)
        {
            btnFinish.IsEnabled = false;
            btnUpdate.IsEnabled = false;

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
            lblStatusLable.Visibility = Visibility.Hidden;
            lblStatus.Visibility = Visibility.Hidden;
            lblPercentage.Visibility = Visibility.Visible;
            progUpdater.Visibility = Visibility.Visible;
            btnUpdate.IsEnabled = false;

            foreach (var process in Process.GetProcessesByName("HRSH-Transpera"))
            {
                process.Kill();
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");
            }

            if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\");
            }

            Uri uri = new Uri("https://an0maly.blob.core.windows.net/transpera/HRSH-Transpera.exe");
            updater.DownloadDataAsync(uri, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
