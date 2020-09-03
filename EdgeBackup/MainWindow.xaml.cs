using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace EdgeBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string EdgeProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Edge\User Data");
        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(EdgeProfilePath))
            {
                RbBackup.IsEnabled = false;
                RbRestore.IsChecked = true;
                MessageBox.Show("No Edge Chromium profile directory found. Backup is disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (RbBackup.IsChecked == true)
            {
                var sfd = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "edgebackup",
                    Filter = "Backup Files|*.edgebackup",
                    FileName = $"EdgeProfile_{DateTime.Now:MM-dd-yyyy_hh-mm-ss}"
                };
                if (sfd.ShowDialog() != true) return;
                if (File.Exists(sfd.FileName))
                    File.Delete(sfd.FileName);
                ZipFile.CreateFromDirectory(EdgeProfilePath, sfd.FileName);
                MessageBox.Show("Backup complete", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (RbRestore.IsChecked == true)
            {
                var sfd = new OpenFileDialog
                {
                    DereferenceLinks = true,
                    CheckFileExists = true,
                    Filter = "Backup Files|*.edgebackup",
                };
                if (sfd.ShowDialog() != true) return;
                ZipFile.ExtractToDirectory(sfd.FileName, EdgeProfilePath,true);
                MessageBox.Show("Restore complete", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }      
    }
}
