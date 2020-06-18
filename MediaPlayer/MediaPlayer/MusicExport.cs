using MusicPlayer;
using NReco.VideoConverter;
using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace MediaPlayer
{
    class MusicExport
    {
        public async Task SaveAudioToDiskAsync(string link, Playlist playList)
        {
            string source = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"music\");
            playList.PlaylistName += @"\"; //playlistname is the foldername
            YoutubeClient youtube = new YoutubeClient();
            Video video = await youtube.Videos.GetAsync(link);
            string legalTitle = string.Join("", video.Title.Split(Path.GetInvalidFileNameChars())); // Removes all possible illegal filename characetrs from the title
            StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);
            IStreamInfo streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
            if (streamInfo != null)
            {
                // Get the actual stream
                //var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                // Download the stream to file
                Console.WriteLine("Ik begin nu aan het downloaden jochem en sietze!");
                string fileName = $"{source + playList.PlaylistName + legalTitle}";
                await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName + ".mp4");
                FFMpegConverter ffMpeg = new FFMpegConverter();
                ffMpeg.ConvertMedia(fileName + ".mp4", fileName + ".mp3", "mp3");
                File.Delete(fileName + ".mp4");
                Song newSong = new Song(fileName + ".mp3");
                newSong.ArtistName = video.Author;
                newSong.SongTitle = video.Title;
                playList.addSong(newSong);
                
                //TODO
                //toevoegen aan database

            }

        }

        public async Task SaveAudioExternal(string Location, string link)
        {
            YoutubeClient youtube = new YoutubeClient();
            Location += @"\";
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
                    string fileName = $"{Location + legalTitle}";
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName + ".mp4");
                    FFMpegConverter ffMpeg = new FFMpegConverter();
                    ffMpeg.ConvertMedia(fileName + ".mp4", fileName + ".mp3", "mp3");
                    File.Delete(fileName + ".mp4");
                }
            }
            catch
            {
                //TODO: actual working error
            }
        }
    }
}
