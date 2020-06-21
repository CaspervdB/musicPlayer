using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaPlayer
{
    class DeletePlaylistViewModel
    {
        public ICommand DeletePlaylist { get; set; }  

        public ObservableCollection<Playlist> Playlists
        {
            get { return PlaylistManager.Instance.Playlists; }
        }

        public Playlist SelectedPlaylistInDeleteWindow { get; set; }

        public void deletePlaylist()
        {
            PlaylistManager.Instance.deletePlaylist(SelectedPlaylistInDeleteWindow);
        }

        public DeletePlaylistViewModel()
        {
            DeletePlaylist = new RelayCommand(() => deletePlaylist());
        }
    }
}
