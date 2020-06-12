using AngleSharp;
using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace MediaPlayer
{
    class MusicExport : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string link;
        private string error;

        public string Link
        {
            get { return link; }
            set
            {
                if (!string.Equals(this.link, value))
                {
                    this.link = value;
                }
                NotifyPropertyChanged("LinkTextBox");
            }
        }
        public string Error
        {
            get { return error; }
            set
            {
                this.error = value;
                NotifyPropertyChanged("Error");
            }
        }
        public ICommand ButtonCommand { get; set; }

        public MusicExport()
        {
            ButtonCommand = new RelayCommand(async () => await SaveAudioToDiskAsync(link));
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task SaveAudioToDiskAsync(String link)
        {
            var source = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\");
            var youtube = new YoutubeClient();
            try
            {
                var video = await youtube.Videos.GetAsync(link);
                var title = video.Title;
                var legalTitle = string.Join("", title.Split(Path.GetInvalidFileNameChars())); // Removes all possible illegal filename characetrs from the title
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);
                var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
                if (streamInfo != null)
                {
                    // Get the actual stream
                    var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                    // Download the stream to file
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{source + legalTitle}.mp3");
                }
            }
            catch
            {
                this.error = "Invalid Youtube URL";
            }
        }
    }
}
