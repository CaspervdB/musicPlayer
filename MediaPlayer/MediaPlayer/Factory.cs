using MusicPlayer;
using System.IO;

namespace MediaPlayer
{
    class Factory
    {
        public static string musicFolderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\");
        public static void createPlaylistCollection(PlaylistManager pm)
        {
            string[] playlists = Directory.GetDirectories(musicFolderPath);
            foreach (string playlistPath in playlists)
            {
                string playlistName = new DirectoryInfo(playlistPath).Name;
                Playlist p = new Playlist(playlistName);
                string playlistFolder = Path.Combine(musicFolderPath, playlistPath);
                string[] songs = Directory.GetFiles(playlistFolder);
                foreach (string songName in songs)
                {
                    string songLocation = Path.Combine(playlistFolder, songName);
                    p.addSong(new Song(songLocation));
                }
                pm.addPlaylist(p);
            }
        }
    }
}
