using System.Data.SQLite;
using System.IO;
using MTDBFramework.IO;

namespace MTDBCreator.DmsExporter.IO
{
    class TextToMtdbConverter : ITextToDbConverter
    {
        public void Convert(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            SQLiteConnection.CreateFile(path);
            var connection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            connection.Open();
            string sql = "create table ConensusProteinPair(PairId int primary key, CleavageState smallint, TerminusState smallint, ResidueStart smallint, ResidueEnd smallint, TargetId int foreign key, ProteinId int foreign key)";
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            //sql = "insert into highscores (name, score) values('Goku', 9001)";
            //command = new SQLiteCommand(sql, connection);
            //command.ExecuteNonQuery();    
        }
    }
}

