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
    class MusicExport
    {
        public async Task SaveAudioToDiskAsync(String link, String playListFolder)
        {
            var source = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music/");
            String editedPlayListFolder = playListFolder.Insert(-1, "/");
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
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{source + editedPlayListFolder + legalTitle}.mp3");
                }
            }
            catch
            {
             //TODO: actual working error
            }
        }
    }
}
