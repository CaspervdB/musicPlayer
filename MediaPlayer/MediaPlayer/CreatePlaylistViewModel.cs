using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaPlayer
{
    class CreatePlaylistViewModel
    {
        public ICommand CreatePlaylistButton { get; set; }

        public string NewPlaylistName { get; set; }

        public void addPlaylist()
        {
            Playlist playlist = new Playlist(NewPlaylistName);
            PlaylistManager.Instance.createPlaylist(playlist);
        }

        public CreatePlaylistViewModel()
        {
            CreatePlaylistButton = new RelayCommand(() => addPlaylist());
        }

    }
}
