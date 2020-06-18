using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MediaPlayer
{
    class ExportMp3ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DownloadButton { get; set; }
        public ICommand FileLocationButton { get; set; }

        //private String link;
        private String Location;

        /*public string YoutubeLink
        {
            get { return link; }
            set
            {
                if (!string.Equals(this.link, value))
                {
                    this.link = value;
                }
            }
        }*/

        public string YoutubeLink { get; set; }
        public string FileLocation
        {
            get { return Location; }
            set
            {
                if (!string.Equals(this.Location, value))
                {
                    this.Location = value;
                    NotifyPropertyChanged("FileLocation");
                }
            }
        }

        //public string FileLocation { get; set; }

        private async Task DowloadSongExternal()
        {
            if(YoutubeLink == null)
            {
                MessageBox.Show("Voer een URL in");
                return;
            }
            if(FileLocation == null)
            {
                MessageBox.Show("Kies een Locatie");
                return;
            }

            try
            {
                MusicExport musicExport = new MusicExport();
                await musicExport.SaveAudioExternal(FileLocation, YoutubeLink);
            }
            catch
            {
                MessageBox.Show("Ongeldige URL");
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
            DownloadButton = new RelayCommand( async () => await DowloadSongExternal());
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
