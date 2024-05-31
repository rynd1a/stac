using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace stac
{
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Users", "select id as Номер, login as Логин, '********' as Пароль, " +
                "type as Тип from users order by id");

            UsersTable.ItemsSource = Connect.ds.Tables["Users"].DefaultView;
            UsersTable.AutoGenerateColumns = true;
            UsersTable.HeadersVisibility = DataGridHeadersVisibility.Column;
            UsersTable.CanUserAddRows = false;
            UsersTable.Columns[0].IsReadOnly = true;
        }

        private void UsersTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            string result;
            string sql = "";

            int id = UsersTable.SelectedIndex;
            DataRowView row = (DataRowView)UsersTable.Items[id - 1];
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            if (id == -1) MessageBox.Show("Выберите строку!");

            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (row["Логин"].ToString() == "")
                {
                    MessageBox.Show("Логин является обязательным для заполнения", "Внимание");
                    return;
                }

                if (row["Пароль"].ToString() == "")
                {
                    MessageBox.Show("Пароль является обязательным для заполнения", "Внимание");
                    return;
                }

                if (row["Номер"].ToString() != "")
                {
                    if (row["Номер"].ToString() != "********")
                    {
                        sql = "update users set login='" +
                            row["Логин"] + "', password=crypt('" + row["Пароль"] + "', gen_salt('md5')), type='" +
                            row["Тип"] + "' where id = " + row["Номер"];
                    }
                    else if (row["Номер"].ToString() == "********")
                    {
                        sql = "update users set login='" +
                            row["Логин"] + "', type='" + row["Тип"] + "' where id = " + row["Номер"];
                    }

                    if (!Connect.Modification_Execute(sql))
                        return;
                    NavigationService.Navigate(new Users());
                }
                else
                {
                    sql = "insert into users(login, password, type) values('" +
                        row["Логин"] + "', crypt('" + row["Пароль"] + ",gen_salt('md5'))', '" + row["Тип"] +
                        "')";
                    if (!Connect.Modification_Execute(sql))
                        return;
                    NavigationService.Navigate(new Users());
                }
            }

            e.Handled = true;
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int index = UsersTable.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            int id = Convert.ToInt32(Connect.ds.Tables["Users"].Rows[index]["Номер"]);
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                string sql = "delete from users where id = " + id;
                if (!Connect.Modification_Execute(sql))
                    return;
                Connect.ds.Tables["Users"].Rows.RemoveAt(index);
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            Connect.ds.Tables["Users"].Rows.Add(new object[] { });
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            UsersConnect usersConnect = new UsersConnect();
            usersConnect.ShowDialog();
        }
    }
}
