using System.Data;
using System.Windows;
using Npgsql;
using System.IO;
using System;


namespace stac
{
    class Connect
    {
        public static DataSet ds = new DataSet();

        public static NpgsqlConnection Connection()
        {
            string line = "";
            StreamReader sr = new StreamReader(@"Config.txt");
            line = sr.ReadLine();
            NpgsqlConnection connection = new NpgsqlConnection(line);
            return connection;
        }

        public static void Table_Fill(string name, string sql)
        {
            if (ds.Tables[name] != null)
                ds.Tables[name].Clear();
            NpgsqlDataAdapter dat;
            dat = new NpgsqlDataAdapter(sql, Connection());
            dat.Fill(ds, name);
            Connection().Close();
        }

        public static bool Modification_Execute(string sql)
        {
            NpgsqlCommand com = new NpgsqlCommand(sql, Connection());
            Connection().Open();
            try {
                com.ExecuteNonQuery();
            } catch (NpgsqlException) {
                MessageBox.Show("Обновление базы данных не было выполнено", "Ошибка");
                Connection().Close(); 
                return false;
            }
            Connection().Close();
            return true;
        }
    }
}
