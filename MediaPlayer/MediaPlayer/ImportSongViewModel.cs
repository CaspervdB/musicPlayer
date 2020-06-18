using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MediaPlayer
{
    class ImportSongViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string fileLocation;
        public ICommand ImportCommand { get; set; }
        public ICommand FileLocationButton { get; set; }
        public Playlist SelectedPlaylistInImportWindow { get; set; }
        public ObservableCollection<Playlist> PlaylistCollection
        {
            get { return PlaylistManager.Instance.Playlists; }
        }

        public string FileLocation
        {
            get { return fileLocation; }
            set
            {
                if (!string.Equals(this.fileLocation, value))
                {
                    this.fileLocation = value;
                    NotifyPropertyChanged("FileLocation");
                }
            }
        }
        private void FileManager()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "All Supported Audio | *.mp3; | MP3s | *.mp3"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                this.FileLocation = ofd.FileName;

            }
        }

        private void songImport(string FileLocation, Playlist playlist)
        {
            string songFilePath = FileLocation;
            string destPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\" + playlist.PlaylistName + @"\"); //FIX DIT!! DOET NIET!! KADUUK!!
            File.Copy(songFilePath, destPath, true);
        }

        public ImportSongViewModel()
        {
            FileLocationButton = new RelayCommand(() => FileManager());
            ImportCommand = new RelayCommand(() => songImport(FileLocation, SelectedPlaylistInImportWindow));
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
