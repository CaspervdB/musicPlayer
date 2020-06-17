using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaPlayer
{
    class ExportMp3ViewModel
    {
        public ICommand DownloadButton { get; set; }
        public ICommand FileLocationButton { get; set; }

        private String link;
        private String Location;// = "C:/Users/User/Documents/GitHub/musicPlayer/MediaPlayer/MediaPlayer/music/playlist 2/";

        public string YoutubeLink
        {
            get { return link; }
            set
            {
                if (!string.Equals(this.link, value))
                {
                    this.link = value;
                }
            }
        }
        public string FileLocation
        {
            get { return Location; }
            set
            {
                if (!string.Equals(this.Location, value))
                {
                    this.Location = value;
                }
            }
        }

        private async Task DowloadSongExternal()
        {
            MusicExport musicExport = new MusicExport();
            //Console.WriteLine("Text");
            await musicExport.SaveAudioExternal(Location, link);
        }

        public ExportMp3ViewModel() 
        {
            //FileLocationButton = new RelayCommand(() => FileManager());
            DownloadButton = new RelayCommand( async () => await DowloadSongExternal());
        }
    }
}
