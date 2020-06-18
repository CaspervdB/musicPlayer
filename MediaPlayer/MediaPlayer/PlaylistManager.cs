using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaPlayer
{
    class PlaylistManager
    {
        public PlaylistManager()
        {
            this.Playlists = new ObservableCollection<Playlist>();

        }

        private static PlaylistManager instance = null;
        private static readonly object padlock = new object();

        public static PlaylistManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PlaylistManager();
                    }
                    return instance;
                }
            }
        }


        public void addPlaylist(Playlist p)
        {
            this.Playlists.Add(p);
        }

        public void createPlaylist(Playlist p)
        {
            //hier moet een methode komen welke een map aanmaakt in de playlist maps en daarna de factory opnieuw aanroept.

            //eerst een map aanmaken met de naam van de meegegeven variabele.
            //daarna playlist object aanmaken.
            //daarna het gemaakte playlist object toevoegen.
            if (playlistExists(p))
            {
                MessageBox.Show("Er bestaat al een playlist met deze naam.");
                return;
            }

            string Path = Factory.musicFolderPath + p.PlaylistName;
            Console.WriteLine(Path);
            Directory.CreateDirectory(Path);
            this.Playlists.Add(p);
        }

        public ObservableCollection<Playlist> Playlists { get; set; }

        public void deletePlaylist(Playlist p)
        {
            if(p != null)
            {
                this.Playlists.Remove(p);

                string Path = Factory.musicFolderPath + p.PlaylistName;
                Console.WriteLine(Path);
                Directory.Delete(Path, true);
            }
        }

        public bool playlistExists(Playlist playlist)
        {
            foreach(Playlist p in Playlists)
            {
                if(p.PlaylistName == playlist.PlaylistName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
