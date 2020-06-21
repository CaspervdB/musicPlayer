using MusicPlayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MediaPlayer
{
    class TopTenArtistsViewModel
    {
        public ObservableCollection<KeyValuePair<string, int>> Data { get; set; }
        public string Title { get; set; }
        public string LegendTitle { get; set; }
        public TopTenArtistsViewModel()
        {
            DbManager db = new DbManager();
            this.Data = db.getTopTenMostPlayedArtists();
            this.Title = "Top Ten Played Artists";
            this.LegendTitle = "Artists";
        }
    }
}
