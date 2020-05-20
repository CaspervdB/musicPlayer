using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MusicPlayer
{
    class Playlist
    {
        public string playlistName { get; set; }
        public List<Song> songList { get; set; }

        public Playlist(string playlistName)
        {
            this.playlistName = playlistName;
            this.songList = new List<Song>();

        }

        public void addSong(Song song)
        {
            this.songList.Add(song);
        }

        public Song getFirstSong()
        {
            return this.songList.First();
        }

        public Song getSongByIndex(int index)
        {
            return this.songList[index];
        }

        public Song getNextSong(Song currentSong)
        {
            int indexOfCurrentSong = this.songList.IndexOf(currentSong);
            Song nextSong = null;
            try
            {
                nextSong = songList[indexOfCurrentSong + 1];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            return nextSong;
        }

        public Song getPreviousSong(Song currentSong)
        {
            int indexOfCurrentSong = this.songList.IndexOf(currentSong);
            Song previousSong = null;
            try
            {
                previousSong = songList[indexOfCurrentSong - 1];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            return previousSong;
        }

        public Song getRandomSong()
        {
            Random random = new Random();
            int randomIndex = random.Next(this.songList.Count);
            return this.songList[randomIndex];
        }
    }
}
