using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    class Song
    {
        
        public string SongTitle
        {
            get { return filetag.Tag.Title; }
            set { filetag.Tag.Title = value;
                filetag.Save();
            }
        }
        
        public string ArtistName
        {
            get {
                string performersString = "";
                bool first = true;
                foreach (string artist in filetag.Tag.Performers)
                {
                    if (first)
                    {
                        performersString += artist;
                        first = false;
                    }
                    else
                    {
                        performersString += ", ";
                        performersString += artist;
                    }
                }
                return performersString;
            }
            set {
                string[] newArtist = new string[1];
                newArtist[0] = value;
                filetag.Tag.Performers = newArtist;
            }
        }

        public string SongLocation{ get; set; }
        private TagLib.File filetag;

        public Song(string songLocation)
        {
            this.SongLocation = songLocation;
            this.filetag = TagLib.File.Create(songLocation);
        }
    }
}
