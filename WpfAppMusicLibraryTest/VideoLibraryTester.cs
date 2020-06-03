using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VideoLibrary;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace WpfAppMusicLibraryTest
{
    class VideoLibraryTester : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string link;

        public string Link
        {
            get { return link; }
            set
            {
                if (value.Contains("youtube.com/watch?v="))
                {
                    link = value;
                }
                NotifyPropertyChanged("");
            }
        }

        public async Task SaveAudioToDisk2Async()
        {
            var source = @"C:\xampp\htdocs\Lora\musicPlayer\WpfAppMusicLibraryTest\test\";
            var youtube = new YoutubeClient();
            var url = "https://www.youtube.com/watch?v=4z2yNxfBRM4";
            var video = await youtube.Videos.GetAsync(url);
            var title = video.Title;
            var legalTitle = string.Join("", title.Split(Path.GetInvalidFileNameChars())); // Removes all possible illegal filename characetrs from the title
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url);
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
            if (streamInfo != null)
            {
                // Get the actual stream
                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                // Download the stream to file
                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{source + legalTitle}.mp3");
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
