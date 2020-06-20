using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    class TopTenArtistsViewModel
    {
        public ObservableCollection<KeyValuePair<string, int>> Data { get; set; }
        public string Title { get; set; }
        public string LegendTitle { get; set; }
        public TopTenArtistsViewModel()
        {
            DbCreator db = new DbCreator();
            this.Data = db.getTopTenMostPlayedArtists();
            this.Title = "Top Ten Played Artists";
            this.LegendTitle = "Artists";
        }
    }
}
