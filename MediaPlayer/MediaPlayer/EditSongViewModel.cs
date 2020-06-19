using Database;
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
using System.Windows.Input;

namespace MediaPlayer
{
    class EditSongViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveEditSongButton { get; set; }
        
        public EditSongViewModel()
        {
            SaveEditSongButton = new RelayCommand<Window>(this.saveEditSong);
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

            Player.Instance.CurrentSong.saveFileTag();

            DbCreator dbCreator = new DbCreator();
            dbCreator.updateSongData(Player.Instance.CurrentSong);

            Player.Instance.initializePlayerComponents();
            CloseWindow(window);
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
