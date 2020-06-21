using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MediaPlayer
{
    class EditSongViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveEditSongButton { get; set; }
        public ICommand ChooseAlbumArt { get; set; }

        public EditSongViewModel()
        {
            SaveEditSongButton = new RelayCommand<Window>(this.saveEditSong);
            ChooseAlbumArt = new RelayCommand(() => this.chooseAlbumArt());
        }

        public string EditArtistName
        {
            get { return Player.Instance.CurrentSong.ArtistName; }
            set { Player.Instance.CurrentSong.ArtistName = value; }
        }

        public string EditSongTitle
        {
            get { return Player.Instance.CurrentSong.SongTitle; }
            set { Player.Instance.CurrentSong.SongTitle = value; }
        }
        private void saveEditSong(Window window)
        {
            Player.Instance.CurrentSong.SaveFileTag();
            Player.Instance.initializePlayerComponents();
            CloseWindow(window);
        }
        private void chooseAlbumArt()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "*.jpg | *.jpg; | *.jpeg; | *.jpeg"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.AlbumArtLink = ofd.FileName;
            }
        }

        private string albumArtLink;
        public string AlbumArtLink
        {
            set
            {
                if (value == null || value == "")
                {
                    return;
                }
                Player.Instance.CurrentSong.setAlbumArt(value);
                this.albumArtLink = value;
                NotifyPropertyChanged();
            }
            get { return this.albumArtLink; }
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
