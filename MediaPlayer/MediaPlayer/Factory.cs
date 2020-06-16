using MusicPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    class Factory
    {
        public static void createPlaylistCollection(PlaylistManager pm)
        {
            var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            Playlist p1 = new Playlist("eerste playlist");
            p1.addSong(new Song("Symphony", "Clean Bandit ft. Zara Larsson", "Clean Bandit - Symphony feat. Zara Larsson.mp3"));
            p1.addSong(new Song("Shape of You", "Ed Sheeran", "Ed Sheeran - Shape of You.mp3"));
            pm.addPlaylist(p1);
            
            Playlist p2 = new Playlist("Tweede playlist");
            p2.addSong(new Song("Cold", "Maroon 5", "Maroon 5 - Cold ft. Future.mp3"));
            pm.addPlaylist(p2);
        }
    }
}
