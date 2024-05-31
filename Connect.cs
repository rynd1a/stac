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

        public static string line = "";

        public static NpgsqlConnection connection = new NpgsqlConnection(LineConnect());

        public static string LineConnect()
        {
            StreamReader sr = new StreamReader(@"Config.txt");
            line = sr.ReadLine();
            return line;
        }

        public static void Table_Fill(string name, string sql)
        {
            if (ds.Tables[name] != null)
                ds.Tables[name].Clear();
            NpgsqlDataAdapter dat;
            dat = new NpgsqlDataAdapter(sql, connection);
            dat.Fill(ds, name);
            connection.Close();
        }

        public static bool Modification_Execute(string sql)
        {
            NpgsqlCommand com = new NpgsqlCommand(sql, connection);
            connection.Open();
            try {
                com.ExecuteNonQuery();
            } catch (NpgsqlException) {
                MessageBox.Show("Обновление базы данных не было выполнено", "Ошибка");
                connection.Close(); 
                return false;
            }
            connection.Close();
            return true;
        }
    }
}
