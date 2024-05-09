using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UsersTable.ItemsSource = Connect.ds.Tables["User"].DefaultView;
            UsersTable.AutoGenerateColumns = true;
            UsersTable.HeadersVisibility = DataGridHeadersVisibility.Column;
            // UsersTable.Columns[0].Visibility = Visibility.Hidden;
            UsersTable.CanUserAddRows = false;
            UsersTable.Columns[0].IsReadOnly = true;
        }

        private void UsersTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
        }

        private void UsersTable_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string result;
                string sql;
                DataRowView row = (DataRowView)UsersTable.SelectedItems[0];
                int id = UsersTable.SelectedIndex;
                MessageBoxButton buttons = MessageBoxButton.YesNo;

                if (id == -1) MessageBox.Show("Выберите строку!");

                result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
                if (result == "No") return;
                else if (result == "Yes")
                {
                    if (row["Номер"].ToString() != "")
                    {
                        sql = "update users set login='" +
                        row["Логин"] + "', password='" + row["Пароль"] + "', type='" +
                        row["Тип"] + "' where id = " + row["Номер"];

                        if (!Connect.Modification_Execute(sql))
                            return;
                        Connect.ds.Tables["User"].Rows[id].ItemArray = new object[] { row["Номер"], row["Логин"], row["Пароль"], row["Тип"] };
                    }
                    else
                    {
                        sql = "insert into users(login, password, type) values('" +
                            row["Логин"] + "', '" + row["Пароль"] + "', '" + row["Тип"] +
                            "')";
                        if (!Connect.Modification_Execute(sql))
                            return;
                        Connect.ds.Tables["User"].Rows[id].ItemArray = new object[] { row["Номер"], row["Логин"], row["Пароль"], row["Тип"] };
                    }
                }
            }
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int id = UsersTable.SelectedIndex;
            if (id == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }

            string result;
            string sql = "";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                sql = "delete from users where id = " + id;
                if (!Connect.Modification_Execute(sql))
                    return;
                Connect.ds.Tables["User"].Rows.RemoveAt(id);
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            Connect.ds.Tables["User"].Rows.Add(new object[] { });
        }

    }
}
