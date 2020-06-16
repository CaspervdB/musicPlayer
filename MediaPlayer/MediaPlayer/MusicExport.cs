using AngleSharp;
using GalaSoft.MvvmLight.Command;
using MusicPlayer;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace MediaPlayer
{
    class MusicExport
    {
        public async Task SaveAudioToDiskAsync(String link, Playlist playList)
        {
            var source = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\");
            playList.PlaylistName += @"\"; //playlistname is the foldername
            var youtube = new YoutubeClient();
            try
            {
                Video video = await youtube.Videos.GetAsync(link);
                string legalTitle = string.Join("", video.Title.Split(Path.GetInvalidFileNameChars())); // Removes all possible illegal filename characetrs from the title
                StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);
                IStreamInfo streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
                if (streamInfo != null)
                {
                    // Get the actual stream
                    var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                    // Download the stream to file
                    string songlocation = $"{source + playList.PlaylistName + legalTitle}.mp3";
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, songlocation);
                    Song song = new Song(songlocation);
                    song.ArtistName = video.Author;
                    song.SongTitle = video.Title;
                    playList.addSong(song); 
                }
            }
            catch
            {
             //TODO: actual working error
            }
        }
    }
}
