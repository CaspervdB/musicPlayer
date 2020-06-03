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
<<<<<<< Updated upstream
                NotifyPropertyChanged();
            }
        }

        public List<Song> SelectedPlaylist
=======
            }
        }
        public Playlist SelectedPlaylist
>>>>>>> Stashed changes
        {
            get {
                if (player.playlist != null){
                    return player.playlist.SongList;
                } else
                {
                    return null;
                }
            }
<<<<<<< Updated upstream
            set { player.playlist.SongList = value;
                NotifyPropertyChanged("Playlist");
=======
            set { 
                player.playlist = value;
                Console.WriteLine("Playlist selected");
                NotifyPropertyChanged();
>>>>>>> Stashed changes
            }
        }

        public Song CurrentSong
        {
            get { return player.CurrentSong; }
            set
            {
                player.CurrentSong = value;
<<<<<<< Updated upstream
=======
                player.play();
>>>>>>> Stashed changes
                
                NotifyPropertyChanged();
            }
        }

<<<<<<< Updated upstream
=======
        private void previous()
        {
            CurrentSong = player.getPreviousSong();
            NotifyPropertyChanged("CurrentSong");
        }

        private void next()
        {
            CurrentSong = player.getNextSong();
        }

>>>>>>> Stashed changes
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
