using MusicPlayer;
using NReco.VideoConverter;
using System.IO;
using System.Net;
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
                // Download the stream to file
                string fileName = $"{source + playList.PlaylistName + legalTitle}";

                await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName + ".mp4"); //downloaden van mp4

                FFMpegConverter ffMpeg = new FFMpegConverter();
                ffMpeg.ConvertMedia(fileName + ".mp4", fileName + ".mp3", "mp3"); //converteren van mp4 naar mp3
                File.Delete(fileName + ".mp4");

                Song newSong = new Song(fileName + ".mp3"); //aanmaken van songobject
                newSong.ArtistName = video.Author; //zetten van de filetags
                newSong.SongTitle = video.Title;
                /* downloaden van thumbnail*/
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(video.Thumbnails.HighResUrl, fileName + ".jpg");
                }

                newSong.setAlbumArt(fileName + ".jpg"); //zetten van albumart metadata

                File.Delete(fileName + ".jpg"); //deleten van thumbnail image file

                newSong.SaveFileTag(); //opslaan van filetags

                playList.addSong(newSong);

                //toevoegen aan database
                DbManager db = new DbManager();
                db.addSongToDatabase(newSong);

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
