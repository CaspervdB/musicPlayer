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
        bool isPlaying = false;
        bool isPaused = false;

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
                        if(isPlaying)
                        {
                            stop();
                        }
                        musicPlayer = new WaveOut()
                        {
                            DesiredLatency = 100
                        };
                        ActiveStream = new Mp3FileReader(filePath);
                        inputStream = new WaveChannel32(ActiveStream);
                        musicPlayer.Init(inputStream);
                        this.isPlaying = true;
                        Console.WriteLine("current song set");
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
            if(CurrentSong == null)
            {
                if(playlist == null)
                {
                    return;
                } else
                {
                    CurrentSong = playlist.getFirstSong();
                }
            }
            this.musicPlayer.Play();
            Console.WriteLine("playing");
            Console.ReadLine();
        }

        public void pause()
        {
            this.musicPlayer.Pause();
        }

        public void stop()
        {
            if(musicPlayer != null)
            {
                musicPlayer.Stop();
            }
            if(activeStream != null)
            {
                inputStream.Close();
                inputStream = null;
                activeStream.Close();
                activeStream = null;
            }
            if (musicPlayer != null)
            {
                musicPlayer.Dispose();
                musicPlayer = null;
            }
        }

        public void next()
        {
            if (playlistAndSongNotNull())
            {
                Song nextSong = this.playlist.getNextSong(this.CurrentSong);
                if (nextSong != null)
                {
                    this.CurrentSong = nextSong;
                    play();
                }
            }
        }

        public void previous()
        {
            if (playlistAndSongNotNull())
            {
                Song previousSong = this.playlist.getPreviousSong(this.CurrentSong);
                if (previousSong != null)
                {
                    this.CurrentSong = previousSong;
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
                    this.CurrentSong = nextRandomSong;
                    play();
                }
            }
        }
    }
}
