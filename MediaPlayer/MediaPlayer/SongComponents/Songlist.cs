﻿using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Song getRandomSong()
        {
            Random random = new Random();
            int randomIndex = random.Next(this.Count);
            return this[randomIndex];
        }

    }
}