using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace MediaPlayer
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand PlayPauseButton { get; set; }
        public ICommand NextButton { get; set; }
        public ICommand PreviousButton { get; set; }
        public ICommand AddSong { get; set; }
        public ICommand CreatePlaylist { get; set; }
        public ICommand DeletePlaylist { get; set; }
        public ICommand DeleteSong { get; set; }
        public ICommand ExportSong { get; set; }
        public ICommand ImportSong { get; set; }
        public ICommand ReloadDatabase { get; set; }
        public ICommand TopTenPlayedSongsMenuItem { get; set; }
        public ICommand TopTenPlayedArtistsMenuItem { get; set; }
        public ICommand EditSongContextMenuItem { get; set; }
        private Playlist selectedPlaylist;

        public ICommand WindowClosing
        {
            get
            {
                return new RelayCommand<CancelEventArgs>(
                    (args) =>
                    {
                        Player.Instance.Dispose();
                    });
            }
        }

        public ObservableCollection<Playlist> PlaylistCollection
        {
            get { return PlaylistManager.Instance.Playlists; }
        }
        public Playlist SelectedPlaylist
        {
            get
            {
                return this.selectedPlaylist;
            }
            set
            {
                this.selectedPlaylist = value;
                updateSonglist();
            }
        }


        public ImageSource AlbumImage
        {
            get
            {
                if (CurrentSong != null)
                {
                    return CurrentSong.getAlbumArt();
                }
                else
                {
                    return null;
                }
            }
        }


        public ObservableCollection<Song> SelectedPlaylistSongs
        {
            get
            {
                if (Player.Instance.Songlist == null)
                {
                    return null;
                }
                return Player.Instance.Songlist;
            }
        }


        public Song CurrentSong
        {
            get { return Player.Instance.CurrentSong; }
            set
            {
                if (value == null)
                {
                    return;
                }
                Player.Instance.CurrentSong = value;
                Player.Instance.play();
                DbManager db = new DbManager();
                db.incrementTimesPlayed(value);
                NotifyPropertyChanged("AlbumImage");
                NotifyPropertyChanged("PlayPauseButtonContent");
            }
        }


        public string PlayPauseButtonContent
        {
            get
            {
                if (Player.Instance.IsPlaying)
                {
                    return ";";
                }
                else
                {
                    return "4";
                }
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
            updateSonglist();
        }
        private void editSong()
        {
            EditSongWindow esw = new EditSongWindow();
            esw.ShowDialog();
            updateSonglist();
            NotifyPropertyChanged("AlbumImage");
        }
        private void addPlaylist()
        {
            CreatePlaylist NewPlaylist = new CreatePlaylist();
            NewPlaylist.ShowDialog();
        }

        private void deleteSong()
        {
            DeleteSong deleteSongWindow = new DeleteSong();
            deleteSongWindow.ShowDialog();
            updateSonglist();
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

        private void importSong()
        {
            ImportSongWindow import = new ImportSongWindow();
            import.ShowDialog();
            updateSonglist();
        }


        private void topTenPlayedArtists()
        {
            TopTenArtistsWindow ttaw = new TopTenArtistsWindow();
            ttaw.ShowDialog();
        }

        private void topTenPlayedSongs()
        {
            TopTenSongsWindow ttsw = new TopTenSongsWindow();
            ttsw.ShowDialog();
        }

        private void reloadDatabase()
        {
            DbManager dbCreator = new DbManager();
            dbCreator.reloadDatabase();
        }
        public MainViewModel()
        {
            PlayPauseButton = new RelayCommand(() => setPlayPause());
            NextButton = new RelayCommand(() => next());
            PreviousButton = new RelayCommand(() => previous());
            AddSong = new RelayCommand(() => addsong());
            EditSongContextMenuItem = new RelayCommand(() => editSong());
            CreatePlaylist = new RelayCommand(() => addPlaylist());
            DeletePlaylist = new RelayCommand(() => deletePlaylist());
            DeleteSong = new RelayCommand(() => deleteSong());
            ExportSong = new RelayCommand(() => exportSong());
            ImportSong = new RelayCommand(() => importSong());
            ReloadDatabase = new RelayCommand(() => reloadDatabase());
            TopTenPlayedSongsMenuItem = new RelayCommand(() => topTenPlayedSongs());
            TopTenPlayedArtistsMenuItem = new RelayCommand(() => topTenPlayedArtists());

            Player.Instance.SongEnded += PlayerInstance_SongEnded;

            Factory.createPlaylistCollection();

        }

        private void setPlayPause()
        {
            if (Player.Instance.IsPlaying)
            {
                Player.Instance.pause();
                NotifyPropertyChanged("PlayPauseButtonContent");
            }
            else
            {
                Player.Instance.play();
                NotifyPropertyChanged("PlayPauseButtonContent");
            }
        }

        private void PlayerInstance_SongEnded(object sender, EventArgs e)
        {
            next();
        }

        private void updateSonglist()
        {
            if (SelectedPlaylist != null)
            {
                Player.Instance.Songlist.Clear();
                foreach (Song item in SelectedPlaylist.SongList)
                {
                    Player.Instance.Songlist.Add(item);
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
