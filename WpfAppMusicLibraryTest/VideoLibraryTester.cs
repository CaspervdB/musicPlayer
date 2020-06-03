using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace WpfAppMusicLibraryTest
{
    class VideoLibraryTester
    {
        private MainWindowViewModel mainWindowViewModel;
        public String url
        {
            get { return mainWindowViewModel.Link; }
            set
            {
                mainWindowViewModel.Link = value;
            }
        }

        public async Task SaveAudioToDisk2Async()
        {
            var source = @"C:\xampp\htdocs\Lora\musicPlayer\WpfAppMusicLibraryTest\test\";
            var youtube = new YoutubeClient();
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
    }
}
