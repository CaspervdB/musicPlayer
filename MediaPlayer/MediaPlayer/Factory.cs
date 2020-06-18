using MusicPlayer;
using System.IO;
using System.Data.SQLite;
using System;
using System.Windows;

namespace MediaPlayer
{
    class Factory
    {
        public static string musicFolderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\");
        public static void createPlaylistCollection()
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
                PlaylistManager.Instance.addPlaylist(p);
            }
        }

        public static void setupDatabase()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";
            
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(stm, con);
            string version = cmd.ExecuteScalar().ToString();

            MessageBox.Show($"SQLite version: {version}");

            Console.WriteLine($"SQLite version: {version}");

        }



    }
}
