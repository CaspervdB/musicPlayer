using MediaPlayer;
using MusicPlayer;
using System;
using System.Data.SQLite;
using System.IO;

namespace MusicPlayer
{
    public class DbCreator
    {
        private SQLiteConnection dbConnection;
        private string strCon;
        private string dbPath;
        private string dbFilePath;
        private void createDbFile()
        {
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            this.dbFilePath = dbPath + "\\songdata.db";

            if (!File.Exists(this.dbFilePath))
            {
                SQLiteConnection.CreateFile(this.dbFilePath);
            }
        }

        public DbCreator()
        {
            this.dbPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"DB");
            createDbFile();
            this.strCon = string.Format("Data Source={0};", dbFilePath);
            this.dbConnection = new SQLiteConnection(strCon);
            createTable();
        }

        public void createTable()
        {
            if (!checkIfExist("song_data"))
            {
                string sqlCommand = "CREATE TABLE song_data(song_location VARCHAR(300) PRIMARY KEY , title VARCHAR(100), artist VARCHAR(100), times_played INT DEFAULT 0)";
                executeQuery(sqlCommand);
            }
        }

        private bool checkIfExist(string tableName)
        {

            this.dbConnection.Open();
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'"; ;
            var result = triggerCommand.ExecuteScalar();
            this.dbConnection.Close();

            return result != null && result.ToString() == tableName ? true : false;
        }

        public void executeQuery(string sqlCommand)
        {
            //SQLiteCommand command = new SQLiteCommand();
            this.dbConnection.Open();
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = sqlCommand;
            triggerCommand.ExecuteNonQuery();
            this.dbConnection.Close();
        }

        public void addSongToDatabase(Song song)
        {
            if (song == null) 
            { 
                return; 
            }

            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(this.dbConnection);
            sqlCommand.CommandText = "insert into song_data (song_location, title, artist) values (@location, @title, @artist)";
            sqlCommand.Parameters.AddWithValue("@artist", song.ArtistName);
            sqlCommand.Parameters.AddWithValue("@title", song.SongTitle);
            sqlCommand.Parameters.AddWithValue("@location", song.SongLocation);
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
            this.dbConnection.Close();
            //getSongData();
        }

        public void deleteSongData(Song song)
        {
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(this.dbConnection);
            sqlCommand.CommandText = "DELETE FROM song_data WHERE song_location = @location";
            sqlCommand.Parameters.AddWithValue("@location", song.SongLocation);
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
            this.dbConnection.Close();
            Console.WriteLine("deleted in db!");

        }

        private void clearDatabase()
        {
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(this.dbConnection);
            sqlCommand.CommandText = "DELETE FROM song_data";
            sqlCommand.ExecuteNonQuery();
            this.dbConnection.Close();
        }

        public void reloadDatabase()
        {
            clearDatabase();

            foreach (Playlist p in PlaylistManager.Instance.Playlists)
            { 
                foreach(Song s in p.SongList)
                {
                    addSongToDatabase(s);
                }
            }
            //getSongData();
        }

        public void updateSongData(Song song)
        {
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(this.dbConnection);
            sqlCommand.CommandText = "UPDATE song_data SET title = @title, artist = @artist WHERE song_location = @location";
            sqlCommand.Parameters.AddWithValue("@location", song.SongLocation);
            sqlCommand.Parameters.AddWithValue("@title", song.SongTitle);
            sqlCommand.Parameters.AddWithValue("@artist", song.ArtistName);
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
            this.dbConnection.Close();
        }

        public void incrementTimesPlayed(Song song)
        {
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(this.dbConnection);
            sqlCommand.CommandText = "UPDATE song_data SET times_played = times_played + 1 WHERE song_location = @location";
            sqlCommand.Parameters.AddWithValue("@location", song.SongLocation);
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
            this.dbConnection.Close();
            getSongDataBySong(song);
        }

        public void getTopTenMostListenedSongs()
        {
            string query = "SELECT title, artist, times_played FROM song_data ORDER BY times_played DESC LIMIT 10";
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(query, this.dbConnection);
            using (SQLiteDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    string title = rdr.GetString(0);
                    string artist = rdr.GetString(1);
                    int timesPlayed = rdr.GetInt32(2);
                    Console.WriteLine($"{title} {artist} {timesPlayed}");
                }
            }
            this.dbConnection.Close();
        }

        public void getSongData()
        {
            string query = "SELECT * FROM song_data";
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(query, this.dbConnection);
            using (SQLiteDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.WriteLine($"{rdr.GetString(0)} {rdr.GetString(1)} {rdr.GetString(2)} {rdr.GetInt32(3)}");
                }
            }
            this.dbConnection.Close();
        }

        public void getSongDataBySong(Song s)
        {
            string query = "SELECT * FROM song_data WHERE `song_location` = @location";
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(this.dbConnection);
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@location", s.SongLocation);
            sqlCommand.Prepare();
            using (SQLiteDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.WriteLine($"{rdr.GetString(0)} {rdr.GetString(1)} {rdr.GetString(2)} {rdr.GetInt32(3)}");
                }
            }
            this.dbConnection.Close();
        }
    }
}