﻿using GalaSoft.MvvmLight.Command;
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
        public ICommand CreatePlaylist { get; set; }
        public ICommand DeletePlaylist { get; set; }
        public ICommand AddSong { get; set; }
        public ICommand DeleteSong { get; set; }
        public ICommand ExportSong { get; set; }
        public ICommand EditSongContextMenuItem { get; set; }
        public ICommand SaveEditSongButton { get; set; }

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
        private PlaylistManager playlistManager;

        public static Player getPlayerInstance()
        {
            return Player.Instance;
        }

        public Playlist SelectedPlaylistInDownloadWindow { get; set; }
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

        public string EditArtistName
        {
            get { return CurrentSong.ArtistName; }
            set { CurrentSong.ArtistName = value; }
        }

        public string EditSongTitle
        {
            get { return CurrentSong.SongTitle; }
            set { CurrentSong.SongTitle = value; }
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
           // EditSongWindow esw = new EditSongWindow();
          //  esw.ShowDialog();
        }
        private async Task DownloadSongAsync()
        {
            MusicExport musicExport = new MusicExport();
            await musicExport.SaveAudioToDiskAsync(link, SelectedPlaylistInDownloadWindow);
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
        public MainViewModel()
        {
            PlayButton = new RelayCommand(() => Player.Instance.play());
            PauseButton = new RelayCommand(() => Player.Instance.pause());
            NextButton = new RelayCommand(() => next());
            PreviousButton = new RelayCommand(() => previous());
            AddSongButton = new RelayCommand(() => addsong());
            EditSongContextMenuItem = new RelayCommand(() => editSong());
            SaveEditSongButton = new RelayCommand(() => saveEditSong());
            DownloadCommand = new RelayCommand(async () => await DownloadSongAsync());
            CreatePlaylist = new RelayCommand(() => addPlaylist());
            DeletePlaylist = new RelayCommand(() => deletePlaylist());
            playlistManager = new PlaylistManager();
            Factory.createPlaylistCollection(this.playlistManager);

        }

        private void saveEditSong()
        {
            Console.WriteLine("Hello im here");
            Console.WriteLine(CurrentSong.SongTitle);
            Console.WriteLine(CurrentSong.ArtistName);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
