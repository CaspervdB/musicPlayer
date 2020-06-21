using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MediaPlayer
{
    class DeleteSongViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DeleteSongButton { get; set; }

        private  Playlist selectedPlaylist;

        public Playlist SelectedPlaylist
        {
            get { return selectedPlaylist; }
            set { selectedPlaylist = value;
                NotifyPropertyChanged("Songlist");
            }
        }

        public Song SelectedSong { get; set; }

        public ObservableCollection<Playlist> Playlists
        {
            get { return PlaylistManager.Instance.Playlists; }
        }

        public ObservableCollection<Song> Songlist
        {
            get
            {
                if (SelectedPlaylist != null)
                {
                    return SelectedPlaylist.SongList;
                }
                else
                {
                    return null;
                }
            }
        }

        public DeleteSongViewModel()
        {
            DeleteSongButton = new RelayCommand(() => deleteSong());
        }
        private void deleteSong()
        {
            if(SelectedSong == null)
            {
                MessageBox.Show("Selecteer een nummer.");
                return;
            }
            if(Player.Instance.CurrentSong == SelectedSong)
            {
                Player.Instance.stop();
            }

            DbManager dbman = new DbManager();
            dbman.deleteSongData(SelectedSong);

            File.Delete(SelectedSong.SongLocation);
            SelectedPlaylist.removeSong(SelectedSong);
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
