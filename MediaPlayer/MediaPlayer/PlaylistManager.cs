using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    class PlaylistManager
    {
        private ObservableCollection<Playlist> playlists;
        public PlaylistManager()
        {
            this.playlists = new ObservableCollection<Playlist>();

        }

        public void addPlaylist(Playlist p)
        {
            this.playlists.Add(p);
        }

        public void createPlaylist(string newPlaylistName)
        {
            //hier moet een methode komen welke een map aanmaakt in de playlist maps en daarna de factory opnieuw aanroept.

            //eerst een map aanmaken met de naam van de meegegeven variabele.
            //daarna playlist object aanmaken.
            //daarna het gemaakte playlist object toevoegen.


            string Path = Factory.musicFolderPath + newPlaylistName;
            Console.WriteLine(Path);
            Directory.CreateDirectory(Path);
            Playlist p = new Playlist(newPlaylistName);
            addPlaylist(p);
        }

        public ObservableCollection<Playlist> Playlists
        {
            get { return this.playlists; }
            set { this.playlists = value; }
        }


        public void deletePlaylist(string playlistName)
        {
            Playlist playlistToRemove = getPlaylistByName(playlistName);
            if(playlistName != null)
            {
                this.Playlists.Remove(playlistToRemove);

                string Path = Factory.musicFolderPath + playlistName;
                Console.WriteLine(Path);
                Directory.Delete(Path);
            }
        }

        public Playlist getPlaylistByName(string name)
        {
            foreach(Playlist p in Playlists)
            {
                if(p.PlaylistName == name)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
