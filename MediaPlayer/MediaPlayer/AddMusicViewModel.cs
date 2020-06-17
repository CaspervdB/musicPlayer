using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MediaPlayer
{
    class AddMusicViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DownloadCommand { get; set; }
        private string link;
        private string error;
        public Playlist SelectedPlaylistInDownloadWindow { get; set; }
        private PlaylistManager playlistManager;
        public string Link
        {
            get { return link; }
            set
            {
                if (!string.Equals(this.link, value))
                {
                    this.link = value;
                }
                NotifyPropertyChanged("LinkTextBox");
            }
        }
        public string Error
        {
            get { return error; }
            set
            {
                if (!string.Equals(this.error, value))
                {
                    this.error = value;
                }
                NotifyPropertyChanged("ErrorLabel");
            }
        }
        public ObservableCollection<Playlist> PlaylistCollection
        {
            get { return this.playlistManager.Playlists; }
            set
            {
                this.playlistManager.Playlists = value;
            }
        }
        private async Task DownloadSongAsync()
        {
            MusicExport musicExport = new MusicExport();
            try
            {
                await musicExport.SaveAudioToDiskAsync(link, SelectedPlaylistInDownloadWindow);
            }
            catch
            {
                error = "Invalid URL";
            }
        }

        public AddMusicViewModel()
        {
            DownloadCommand = new RelayCommand(async () => await DownloadSongAsync());
            playlistManager = new PlaylistManager();
            Factory.createPlaylistCollection();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
