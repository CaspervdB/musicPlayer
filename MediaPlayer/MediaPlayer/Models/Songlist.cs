using MusicPlayer;
using System;
using System.Collections.ObjectModel;

namespace MediaPlayer
{
    class Songlist : ObservableCollection<Song>
    {
        public Song getNextSong(Song currentSong)
        {
            int indexOfCurrentSong = this.IndexOf(currentSong);
            try
            {
                return this[indexOfCurrentSong + 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public Song getPreviousSong(Song currentSong)
        {
            int indexOfCurrentSong = this.IndexOf(currentSong);
            try
            {
                return this[indexOfCurrentSong - 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
