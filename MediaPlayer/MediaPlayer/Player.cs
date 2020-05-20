using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MusicPlayer
{
    class Player
    {
        public Song currentSong { get; set; }
        private System.Windows.Media.MediaPlayer musicPlayer;
        public Playlist playlist{ get; set; }

        public Player()
        {
            this.currentSong = null;
            this.musicPlayer = new System.Windows.Media.MediaPlayer();
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
            if (playlistAndSongNotNull())
            {
                musicPlayer.Open(currentSong.songLocation);
                musicPlayer.Play();
            }
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
