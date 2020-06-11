using NAudio.Wave;
using System;
using System.IO;

namespace MusicPlayer
{
    class Player
    {
        private Song currentSong { get; set; }
        private WaveOut musicPlayer;
        private WaveChannel32 inputStream;
        private WaveStream activeStream;
        public Playlist playlist { get; set; }
        private String musicFolderPath;

        public WaveStream ActiveStream
        {
            get { return activeStream; }
            protected set
            {
                WaveStream oldValue = activeStream;
                activeStream = value;
                //if (oldValue != activeStream) 
                //NotifyPropertyChanged("ActiveStream");
            }
        }

        public Player()
        {
            this.currentSong = null;
            this.musicPlayer = new WaveOut();
            this.playlist = null;
            this.musicFolderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\");
        }

        public Song CurrentSong
        {
            get { return this.currentSong; }
            set
            {
                string filePath = Path.Combine(this.musicFolderPath, value.SongLocation);

                if (File.Exists(filePath))
                {
                    this.currentSong = value;
                    try
                    {
                        musicPlayer = new WaveOut()
                        {
                            DesiredLatency = 100
                        };
                        ActiveStream = new Mp3FileReader(filePath);
                        inputStream = new WaveChannel32(ActiveStream);
                        musicPlayer.Init(inputStream);
<<<<<<< Updated upstream

                        Console.WriteLine("playing");
=======
                        this.isPlaying = true;
                        ChannelLength = inputStream.TotalTime.TotalSeconds;
                        FileTag = TagLib.File.Create(filePath);
                        Console.WriteLine("current song set");
>>>>>>> Stashed changes
                        Console.ReadLine();
                    }
                    catch
                    {
                        ActiveStream = null;

                        Console.WriteLine("catched error");
                        Console.ReadLine();
                    }
                }

            }
        }

        private Boolean playlistAndSongNotNull()
        {
            if (currentSong == null || playlist == null)
            {
                return false;
            }
            return true;
        }



        public void play()
        {
            this.musicPlayer.Play();
        }

        public void pause()
        {
            this.musicPlayer.Pause();
        }

        public void next()
        {
            if (playlistAndSongNotNull())
            {
                Song nextSong = this.playlist.getNextSong(this.currentSong);
                if (nextSong != null)
                {
                    this.currentSong = nextSong;
                    play();
                }
            }
        }

        public void previous()
        {
            if (playlistAndSongNotNull())
            {
                Song previousSong = this.playlist.getPreviousSong(this.currentSong);
                if (previousSong != null)
                {
                    this.currentSong = previousSong;
                    play();
                }
            }
        }

        public void nextRandom()
        {
            if (playlistAndSongNotNull())
            {
                Song nextRandomSong = this.playlist.getRandomSong();
                if (nextRandomSong != null)
                {
                    this.currentSong = nextRandomSong;
                    play();
                }
            }
        }
    }
}
