using MusicPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    class MainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Playlist> playlistCollection = new List<Playlist>();

        private Player player;

        public List<Playlist> PlaylistCollection
        {
            get { return playlistCollection; }
            set
            {
                playlistCollection = value;
                NotifyPropertyChanged();
            }
        }

        public Playlist SelectedPlaylist
        {
            get { return player.playlist; }
            set { player.playlist = value;
                NotifyPropertyChanged();
            }
        }

        public Song CurrentSong
        {
            get { return player.currentSong; }
            set
            {
                player.currentSong = value;
                player.play();
                NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            this.player = new Player();
            this.playlistCollection = Factory.createPlaylistCollection();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
