using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaPlayer
{
    class MainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand PlayButton { get; set; }
        public ICommand PauseButton { get; set; }
        public ICommand NextButton { get; set; }
        public ICommand PreviousButton { get; set; }
        public ICommand AddSongButton { get; set; }
        public ICommand DownloadCommand { get; set; }
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
        private string link;
        private static Player player;
        private PlaylistManager playlistManager;

        public static Player getPlayerInstance()
        {
            return player;
        }
        public string Link
        {
            get { return link; }
            set
            {
                if (!string.Equals(this.link, value))
                {
                    this.link = value;
                }
                NotifyPropertyChanged("LinkTextBox");
            }
        }

        public ObservableCollection<Playlist> PlaylistCollection
        {
            get { return this.playlistManager.Playlists; }
            set
            {
                this.playlistManager.Playlists = value;
            }
        }
        public Playlist SelectedPlaylist
        {
            get
            {
                return null;
            }
            set {
                SelectedPlaylistSongs = value.SongList;
                Console.WriteLine("Playlist selected");
                NotifyPropertyChanged();
            }
        }


        public ObservableCollection<Song> SelectedPlaylistSongs
        {
            get
            {
                if (player.Songlist != null)
                {
                    return player.Songlist;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                player.Songlist.Clear();
                foreach (Song item in value)
                {
                    player.Songlist.Add(item);
                }


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
        private async Task DownloadSongAsync()
        {
            MusicExport musicExport = new MusicExport();
            await musicExport.SaveAudioToDiskAsync(link, "eerste playlist");
        }


        public MainViewModel()
        {
            PlayButton = new RelayCommand(() => player.play());
            PauseButton = new RelayCommand(() => player.pause());
            NextButton = new RelayCommand(() => next());
            PreviousButton = new RelayCommand(() => previous());
            AddSongButton = new RelayCommand(() => addsong());
            DownloadCommand = new RelayCommand(async () => await DownloadSongAsync());
            playlistManager = new PlaylistManager();
            player = new Player();
            Factory.createPlaylistCollection(this.playlistManager);
            /*this.player.playlist = playlistCollection[0];*/

            

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
