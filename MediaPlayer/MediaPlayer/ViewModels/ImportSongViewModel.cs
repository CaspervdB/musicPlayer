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
        public string ArtistName { get; set; }
        public string SongTitle { get; set; }
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

        private void songImport(Window window)
        {
            if(FileLocation == null)
            {
                System.Windows.MessageBox.Show("Kies een nummer om te importeren");
                return;
            }
            if(SelectedPlaylistInImportWindow == null)
            {
                System.Windows.MessageBox.Show("Kies een playlist");
                return;
            }
            if(ArtistName == null)
            {
                System.Windows.MessageBox.Show("Vul de artiest van het nummer in");
                return;
            }
            if (SongTitle == null)
            {
                System.Windows.MessageBox.Show("Vul de titel van het nummer in");
                return;
            }
            string songFilePath = FileLocation;
            string destPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\" + SelectedPlaylistInImportWindow.PlaylistName + @"\" + ArtistName + " - " + SongTitle + ".mp3");
            File.Copy(songFilePath, destPath);
            Song newSong = new Song(destPath);
            newSong.ArtistName = ArtistName;
            newSong.SongTitle = SongTitle;
            newSong.saveFileTag();
            SelectedPlaylistInImportWindow.addSong(newSong);
            DbManager db = new DbManager();
            db.addSongToDatabase(newSong);
            System.Windows.MessageBox.Show("Import succesvol");
            CloseWindow(window);
        }

        public ImportSongViewModel()
        {
            FileLocationButton = new RelayCommand(() => FileManager());
            ImportCommand = new RelayCommand<Window>(this.songImport);
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
