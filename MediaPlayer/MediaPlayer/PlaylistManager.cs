using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void createPlaylist()
        {
            //hier moet een methode komen welke een map aanmaakt in de playlist maps en daarna de factory opnieuw aanroept.
        }

        public ObservableCollection<Playlist> Playlists
        {
            get { return this.playlists; }
            set { this.playlists = value; }
        }


        public void deletePlaylist()
        {
            //hier moet een methode komen welke de gekozen playlist map verwijderd icl. inhoud.
        }
    }
}
