using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    class Song
    {
        public string SongTitle { get; set; }
        public string ArtistName { get; set; }
        public string SongLocation{ get; set; }

        public Song(string songTitle, string artistName, string songLocation)
        {
            this.SongTitle = songTitle;
            this.ArtistName = artistName;
            this.SongLocation = songLocation;
        }
    }
}
