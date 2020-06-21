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
            Song nextSong = null;
            try
            {
                nextSong = this[indexOfCurrentSong + 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
            return nextSong;
        }

        public Song getPreviousSong(Song currentSong)
        {
            int indexOfCurrentSong = this.IndexOf(currentSong);
            Song previousSong = null;
            try
            {
                previousSong = this[indexOfCurrentSong - 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
            return previousSong;
        }
    }
}
