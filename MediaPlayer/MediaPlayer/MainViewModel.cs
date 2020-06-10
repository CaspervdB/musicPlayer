using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaPlayer
{
    class MainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Playlist> playlistCollection = new List<Playlist>();
        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }

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

        public List<Song> SelectedPlaylist
        {
            get {
                if (player.playlist != null){
                    return player.playlist.SongList;
                } else
                {
                    return null;
                }
            }
            set { player.playlist.SongList = value;
                NotifyPropertyChanged("Playlist");
            }
        }

        public Song CurrentSong
        {
            get { return player.CurrentSong; }
            set
            {
                player.CurrentSong = value;
                
                NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            PlayButton = new RelayCommand(() => player.play());
            PauseButton = new RelayCommand(() => player.pause());
            this.player = new Player();
            this.playlistCollection = Factory.createPlaylistCollection();
            this.player.CurrentSong = playlistCollection[0].SongList[1];
            Console.WriteLine(player.CurrentSong.SongTitle);
            Console.ReadLine();

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
