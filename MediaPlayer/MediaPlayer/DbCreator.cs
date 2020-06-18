using System;
using System.Data.SQLite;
using System.IO;

namespace Database
{
    public class DbCreator
    {
        private SQLiteConnection dbConnection;
        private string strCon;
        private string sqlCommand;
        private string dbPath = Environment.CurrentDirectory + "\\DB";
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
            createDbFile();
            this.strCon = string.Format("Data Source={0};", dbFilePath);
            this.dbConnection = new SQLiteConnection(strCon);
            createTable();
        }

        /*public string createDbConnection()
        {
            
            this.dbConnection.Open();
            this.command = dbConnection.CreateCommand();
            return strCon;
        }*/

        public void createTable()
        {
            if (!checkIfExist("song_data"))
            {
                string sqlCommand = "CREATE TABLE song_data(song_id INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(100), artist VARCHAR(100), times_played INT DEFAULT 0)";
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

        /*private bool checkIfTableContainsData(string tableName)
        {
            SQLiteCommand selectCommand = new SQLiteCommand();

            selectCommand.CommandText = "SELECT count(*) FROM " + tableName;
            var result = selectCommand.ExecuteScalar();

            return Convert.ToInt32(result) > 0 ? true : false;
        }*/


        public void addSongData(string title, string artist)
        {
            this.dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(this.dbConnection);
            sqlCommand.CommandText = "insert into song_data ('title', 'artist') values (@title, @artist)";
            sqlCommand.Parameters.AddWithValue("@artist", artist);
            sqlCommand.Parameters.AddWithValue("@title", title);
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
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

                        Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetString(2)} {rdr.GetInt32(3)}");
                    }
                }
                this.dbConnection.Close();

         
        }

    }
}