using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MediaPlayer
{
    class ExportMp3ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DownloadButton { get; set; }
        public ICommand FileLocationButton { get; set; }

        private String fileLocation;

        public string YoutubeLink { get; set; }
        public string FileLocation
        {
            get { return this.fileLocation; }
            set
            {
                if (!string.Equals(this.fileLocation, value))
                {
                    this.fileLocation = value;
                    NotifyPropertyChanged("FileLocation");
                }
            }
        }

        private async void DowloadSongExternal(Window window)
        {
            if(YoutubeLink == null)
            {
                System.Windows.MessageBox.Show("Voer een URL in");
                return;
            }
            if(FileLocation == null)
            {
                System.Windows.MessageBox.Show("Kies een Locatie");
                return;
            }

            try
            {
                MusicExport musicExport = new MusicExport();
                await musicExport.SaveAudioExternal(FileLocation, YoutubeLink);
                System.Windows.MessageBox.Show("Downloaden voltooid!");
                CloseWindow(window);
            }
            catch
            {
                System.Windows.MessageBox.Show("Ongeldige URL");
            }
        }

        private void FileManager()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(folderBrowserDialog.SelectedPath);
                this.FileLocation = folderBrowserDialog.SelectedPath;

            }
        }

        public ExportMp3ViewModel() 
        {
            FileLocationButton = new RelayCommand(() => FileManager());
            DownloadButton = new RelayCommand<Window>(this.DowloadSongExternal);
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
