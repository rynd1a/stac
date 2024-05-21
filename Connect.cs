using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Npgsql;


namespace stac
{
    class Connect
    {
        public static NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=root; Database=stac;");

        public static DataSet ds = new DataSet();

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
                MessageBox.Show("Обновление базы данных не было выполнено из-за некорректно указанных" +
                    " обновляемых данных либо отсутствующих, но при этом обязательных", "Ошибка");
                connection.Close(); 
                return false;
            }
            connection.Close();
            return true;
        }
    }
}
