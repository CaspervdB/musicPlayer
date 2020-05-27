using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    class Song
    {
        public string songTitle { get; set; }
        public string artistName { get; set; }
        public string songLocation{ get; set; }

        public Song(string songTitle, string artistName, string songLocation)
        {
            this.songTitle = songTitle;
            this.artistName = artistName;
            this.songLocation = songLocation;
        }
    }
}
