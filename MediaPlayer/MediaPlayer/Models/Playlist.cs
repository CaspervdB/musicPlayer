using MediaPlayer;

namespace MusicPlayer
{
    class Playlist
    {
        public string PlaylistName { get; set; }
        public Songlist SongList { get; set; }

        public Playlist(string playlistName)
        {
            this.PlaylistName = playlistName;
            this.SongList = new Songlist();

        }

        public void addSong(Song song)
        {
            this.SongList.Add(song);
        }

        public void removeSong(Song song)
        {
            this.SongList.Remove(song);
        }


    }
}
