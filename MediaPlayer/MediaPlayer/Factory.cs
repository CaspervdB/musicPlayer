using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    class Factory
    {
        public static List<Playlist> createPlaylistCollection()
        {
            List<Playlist> playlistCollection = new List<Playlist>();
            Playlist p1 = new Playlist("eerste playlist");
            p1.addSong(new Song("Tuesday", "Burak", "C:/Users/Sietze Koonstra/Downloads/Burak Yeter - Tuesday (Fafaq Remix) (Bass Boosted).mp3"));
            p1.addSong(new Song("Shape of You", "Ed Sheeran", "C:/Users/Sietze Koonstra/Downloads/Ed Sheeran - Shape of You.mp3"));
            playlistCollection.Add(p1);
            Playlist p2 = new Playlist("Tweede playlist");
            p2.addSong(new Song("Tuesday", "Burak", "C:/Users/Sietze Koonstra/Downloads/Burak Yeter - Tuesday (Fafaq Remix) (Bass Boosted).mp3"));
            playlistCollection.Add(p2);
            return playlistCollection;
        }
    }
}
