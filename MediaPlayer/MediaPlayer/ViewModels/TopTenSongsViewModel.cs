using MusicPlayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MediaPlayer
{
    class TopTenSongsViewModel
    {
        public ObservableCollection<KeyValuePair<string, int>> Data { get; set; }
        public string Title { get; set; }
        public string LegendTitle { get; set; }
        public TopTenSongsViewModel()
        {
            DbManager db = new DbManager();
            this.Data = db.getTopTenMostListenedSongs();
            this.Title = "Top Ten Played Songs";
            this.LegendTitle = "Songs";
        }
    }
}
