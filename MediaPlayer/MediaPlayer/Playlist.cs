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
        public string PlaylistName { get; set; }
        public List<Song> SongList { get; set; }

        public Playlist(string playlistName)
        {
            this.PlaylistName = playlistName;
            this.SongList = new List<Song>();

        }

        public void addSong(Song song)
        {
            this.SongList.Add(song);
        }

        public Song getFirstSong()
        {
            return this.SongList.First();
        }

        public Song getSongByIndex(int index)
        {
            return this.SongList[index];
        }

        public Song getNextSong(Song currentSong)
        {
            int indexOfCurrentSong = this.SongList.IndexOf(currentSong);
            Song nextSong = null;
            try
            {
                nextSong = SongList[indexOfCurrentSong + 1];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            return nextSong;
        }

        public Song getPreviousSong(Song currentSong)
        {
            int indexOfCurrentSong = this.SongList.IndexOf(currentSong);
            Song previousSong = null;
            try
            {
                previousSong = SongList[indexOfCurrentSong - 1];
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
            int randomIndex = random.Next(this.SongList.Count);
            return this.SongList[randomIndex];
        }
    }
}
