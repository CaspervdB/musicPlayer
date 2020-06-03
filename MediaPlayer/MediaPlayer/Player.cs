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
                if(value == null)
                {
                    return;
                }
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

                        Console.WriteLine("playing");
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

<<<<<<< Updated upstream
        public void next()
=======
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

        public Song getNextSong()
>>>>>>> Stashed changes
        {
            if (playlistAndSongNotNull())
            {
                Song nextSong = this.playlist.getNextSong(this.currentSong);
                if (nextSong != null)
                {
<<<<<<< Updated upstream
                    this.currentSong = nextSong;
                    play();
=======
                    return nextSong;
>>>>>>> Stashed changes
                }
            }
            return null;
        }

        public Song getPreviousSong()
        {
            if (playlistAndSongNotNull())
            {
                Song previousSong = this.playlist.getPreviousSong(this.currentSong);
                if (previousSong != null)
                {
<<<<<<< Updated upstream
                    this.currentSong = previousSong;
                    play();
=======
                    return previousSong;
>>>>>>> Stashed changes
                }
            }
            return null;
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
