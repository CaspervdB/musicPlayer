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
            get { return PlaylistManager.Instance.Playlists; }            
        }
        private async void DownloadSongAsync(Window window)
        {
            if (link == "" && SelectedPlaylistInDownloadWindow == null)
            {
                MessageBox.Show("Voer een URL in en kies een playlist.");
                return;
            }

            if (link == "")
            {
                MessageBox.Show("Voer een URL in.");
                return;
            }

            if (SelectedPlaylistInDownloadWindow == null)
            {
                MessageBox.Show("Selecteer een playlist.");
                return;
            }            
            
            try
            {
                MusicExport musicExport = new MusicExport();
                await musicExport.SaveAudioToDiskAsync(link, SelectedPlaylistInDownloadWindow);
                MessageBox.Show("Downloaden voltooid.");
                CloseWindow(window);

            }
            catch
            {
                MessageBox.Show("De opgegeven URL is ongeldig.");
            }
        }

        public AddMusicViewModel()
        {
            DownloadCommand = new RelayCommand<Window>(this.DownloadSongAsync);
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
