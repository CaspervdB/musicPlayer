using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicPlayer
{
    public class Song
    {
        public string SongLocation { get; set; }
        private TagLib.File filetag;

        public string SongTitle
        {
            get { return filetag.Tag.Title; }
            set
            {
                filetag.Tag.Title = value;
            }
        }

        public string ArtistName
        {
            get
            {
                string performersString = "";
                bool first = true;
                foreach (string artist in filetag.Tag.Performers)
                {
                    if (first)
                    {
                        performersString += artist;
                        first = false;
                    }
                    else
                    {
                        performersString += ", ";
                        performersString += artist;
                    }
                }
                return performersString;
            }
            set
            {
                string[] newArtist = new string[1];
                newArtist[0] = value;
                filetag.Tag.Performers = newArtist;
            }
        }

        public void setAlbumArt(string imagePath)
        {

            TagLib.Picture picture = new TagLib.Picture();
            picture.Type = TagLib.PictureType.FrontCover;
            picture.Description = "Cover";
            picture.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;

            try
            {
                picture.Data = TagLib.ByteVector.FromPath(imagePath);
            }
            catch (Exception)
            {
                return;
            }
            this.filetag.Tag.Pictures = new TagLib.IPicture[] { picture };
        }

        public ImageSource getAlbumArt()
        {
            if (this.filetag.Tag.Pictures.Length > 0)
            {
                using (MemoryStream albumImageStream = new MemoryStream(this.filetag.Tag.Pictures[0].Data.Data))
                {
                    try
                    {
                        BitmapImage albumImage = new BitmapImage();
                        albumImage.BeginInit();
                        albumImage.CacheOption = BitmapCacheOption.OnLoad;
                        albumImage.StreamSource = albumImageStream;
                        albumImage.EndInit();
                        return albumImage;
                    }
                    catch (NotSupportedException)
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public Song(string songLocation)
        {
            this.SongLocation = songLocation;
            this.filetag = TagLib.File.Create(songLocation);
        }

        public void saveFileTag()
        {
            if (Player.Instance.CurrentSong == this)
            {
                Player.Instance.stop();
            }
            DbManager dbCreator = new DbManager();
            this.filetag.Save();
            dbCreator.updateSongData(this);

        }
    }
}
