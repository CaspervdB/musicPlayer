using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        public ICommand NextButton { get; set; }
        public ICommand PreviousButton { get; set; }
        public ICommand AddSongButton { get; set; }
        public ICommand WindowClosing
        {
            get
            {
                return new RelayCommand<CancelEventArgs>(
                    (args) => {
                        MainViewModel.getPlayerInstance().Dispose();
                    });
            }
        }

        private static Player player;

        public static Player getPlayerInstance()
        {
            return player;
        }

        public List<Playlist> PlaylistCollection
        {
            get { return playlistCollection; }
            set
            {
                playlistCollection = value;
            }
        }
        public Playlist SelectedPlaylist
        {
            get {
                if (player.playlist != null){
                    return player.playlist;
                } else
                {
                    return null;
                }
            }
            set { 
                player.playlist = value;
                Console.WriteLine("Playlist selected");
                NotifyPropertyChanged();
            }
        }
        public Song CurrentSong
        {
            get { return player.CurrentSong; }
            set
            {
                if(value == null)
                {
                    return;
                }
                player.CurrentSong = value;
                player.play();
                /*
                NotifyPropertyChanged();*/
            }
        }

        private void previous()
        {
            CurrentSong = player.getPreviousSong();
            NotifyPropertyChanged("CurrentSong");
        }

        private void next()
        {
            CurrentSong = player.getNextSong();
            NotifyPropertyChanged("CurrentSong");
        }
        private void addsong()
        {
            AddMusicWindow addMusicWindow = new AddMusicWindow();
            addMusicWindow.ShowDialog();
        }


        public MainViewModel()
        {
            PlayButton = new RelayCommand(() => player.play());
            PauseButton = new RelayCommand(() => player.pause());
            NextButton = new RelayCommand(() => next());
            PreviousButton = new RelayCommand(() => previous());
            AddSongButton = new RelayCommand(() => addsong());
            player = new Player();
            this.playlistCollection = Factory.createPlaylistCollection();
            /*this.player.playlist = playlistCollection[0];*/

            

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
