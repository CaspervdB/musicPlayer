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
        public ICommand CreatePlaylist { get; set; }
        public ICommand DeletePlaylist { get; set; }
        public ICommand AddSong { get; set; }
        public ICommand DeleteSong { get; set; }
        public ICommand ExportSong { get; set; }
        public ICommand EditSongContextMenuItem { get; set; }

        public ICommand WindowClosing
        {
            get
            {
                return new RelayCommand<CancelEventArgs>(
                    (args) => {
                        Player.Instance.Dispose();
                    });
            }
        }

        private string link;
        
        public ObservableCollection<Playlist> PlaylistCollection
        {
            get { return PlaylistManager.Instance.Playlists; }
            set
            {
                PlaylistManager.Instance.Playlists = value;
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
                if (Player.Instance.Songlist != null)
                {
                    return Player.Instance.Songlist;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                Player.Instance.Songlist.Clear();
                foreach (Song item in value)
                {
                    Player.Instance.Songlist.Add(item);
                }


            }
        }


        public Song CurrentSong
        {
            get { return Player.Instance.CurrentSong; }
            set
            {
                if(value == null)
                {
                    return;
                }
                Player.Instance.CurrentSong = value;
                Player.Instance.play();
            }
        }

        private void previous()
        {
            CurrentSong = Player.Instance.getPreviousSong();
            NotifyPropertyChanged("CurrentSong");
        }

        private void next()
        {
            CurrentSong = Player.Instance.getNextSong();
            NotifyPropertyChanged("CurrentSong");
        }
        private void addsong()
        {
            AddMusicWindow addMusicWindow = new AddMusicWindow();
            addMusicWindow.ShowDialog();
        }
        private void editSong()
        {
            EditSongWindow esw = new EditSongWindow();
            esw.ShowDialog();
        }
        private void addPlaylist()
        {
            Window1 NewPlaylist = new Window1();
            NewPlaylist.ShowDialog();
        }

        private void deletePlaylist()
        {
            DeletePlaylist deletePlaylist = new DeletePlaylist();
            deletePlaylist.ShowDialog();
        }

        private void exportSong() 
        {
            ExportMp3Window export = new ExportMp3Window();
            export.ShowDialog();
        }
        public MainViewModel()
        {
            PlayButton = new RelayCommand(() => Player.Instance.play());
            PauseButton = new RelayCommand(() => Player.Instance.pause());
            NextButton = new RelayCommand(() => next());
            PreviousButton = new RelayCommand(() => previous());
            AddSongButton = new RelayCommand(() => addsong());
            EditSongContextMenuItem = new RelayCommand(() => editSong());
            CreatePlaylist = new RelayCommand(() => addPlaylist());
            DeletePlaylist = new RelayCommand(() => deletePlaylist());
            ExportSong = new RelayCommand(() => exportSong());
                        
            Factory.createPlaylistCollection();  
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
