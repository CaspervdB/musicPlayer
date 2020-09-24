using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        public string EditAlbumTitle
        {
            get { return Player.Instance.CurrentSong.AlbumTitle; }
            set { Player.Instance.CurrentSong.AlbumTitle = value; }
        }

        public int EditYear
        {
            get { return Player.Instance.CurrentSong.Year; }
            set { Player.Instance.CurrentSong.Year = value; }
        }

        public int EditBPM
        {
            get { return Player.Instance.CurrentSong.BPM; }
            set { Player.Instance.CurrentSong.BPM = value; }
        }

        public int EditTrack
        {
            get { return Player.Instance.CurrentSong.AlbumTrack; }
            set { Player.Instance.CurrentSong.AlbumTrack = value; }
        }

        public int EditMaxTrack
        {
            get { return Player.Instance.CurrentSong.MaxAlbumTrack; }
            set { Player.Instance.CurrentSong.MaxAlbumTrack = value; }
        }

        public string EditCopy
        {
            get { return Player.Instance.CurrentSong.Copyright; }
            set { Player.Instance.CurrentSong.Copyright = value; }
        }

        private void saveEditSong(Window window)
        {

            Player.Instance.CurrentSong.saveFileTag();

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
