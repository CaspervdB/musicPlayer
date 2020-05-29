using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NAudio.Wave;
using WPFSoundVisualizationLib;

namespace MusicPlayer
{
    class Player
    {
        public Song currentSong { get; set; }
        private WaveOut musicPlayer;
        private WaveChannel32 inputStream;
        private WaveStream activeStream;
        public Playlist playlist{ get; set; }

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
            /*if (playlistAndSongNotNull())
            {
                musicPlayer.Open(currentSong.songLocation);
                musicPlayer.Play();
            }*/
            if (System.IO.File.Exists(currentSong.songLocation))
            {
                try
                {
                    musicPlayer = new WaveOut()
                    {
                        DesiredLatency = 100
                    };
                    ActiveStream = new Mp3FileReader(currentSong.songLocation);
                    inputStream = new WaveChannel32(ActiveStream);
                    //sampleAggregator = new SampleAggregator(fftDataSize);
                    //inputStream.Sample += inputStream_Sample;
                    musicPlayer.Init(inputStream);
                    //ChannelLength = inputStream.TotalTime.TotalSeconds;
                    //FileTag = TagLib.File.Create(path);
                    //GenerateWaveformData(path);
                    //CanPlay = true;
                    musicPlayer.Play();
                }
                catch
                {
                    ActiveStream = null;
                    
                    Console.WriteLine("catched error");
                    Console.ReadLine();
                }
            }
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
                if(nextSong != null)
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
