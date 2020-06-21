using MediaPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace MusicPlayer
{
    public class DbManager
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

        public DbManager()
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
                foreach (Song s in p.SongList)
                {
                    addSongToDatabase(s);
                }
            }
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
        }

        public ObservableCollection<KeyValuePair<string, int>> getTopTenMostListenedSongs()
        {
            ObservableCollection<KeyValuePair<string, int>> data = new ObservableCollection<KeyValuePair<string, int>>();
            string query = "SELECT title, times_played FROM song_data WHERE times_played != 0 ORDER BY times_played DESC LIMIT 10";
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(query, this.dbConnection);
            using (SQLiteDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    string title = rdr.GetString(0);
                    int timesPlayed = rdr.GetInt32(1);
                    if (title.Length > 10)
                    {
                        title = title.Substring(0, 9);
                        title += "...";
                    }
                    data.Add(new KeyValuePair<string, int>(title, timesPlayed));
                }
            }
            this.dbConnection.Close();
            return data;
        }

        public ObservableCollection<KeyValuePair<string, int>> getTopTenMostPlayedArtists()
        {
            ObservableCollection<KeyValuePair<string, int>> data = new ObservableCollection<KeyValuePair<string, int>>();
            string query = "SELECT artist, SUM(times_played) as tp FROM song_data GROUP BY artist, times_played HAVING tp > 0 ORDER BY times_played DESC LIMIT 10";
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(query, this.dbConnection);
            using (SQLiteDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    string artist = rdr.GetString(0);
                    int timesPlayed = rdr.GetInt32(1);
                    if (artist.Length > 15)
                    {
                        artist = artist.Substring(0, 14);
                        artist += "...";
                    }
                    data.Add(new KeyValuePair<string, int>(artist, timesPlayed));
                }
            }
            this.dbConnection.Close();
            return data;
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