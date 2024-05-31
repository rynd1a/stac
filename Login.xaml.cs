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
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        public static string id_userVrach = "";

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Connect.ds.Tables["User"].Rows.Count; i++)
            {
                if ((loginTextBox.Text == Connect.ds.Tables["User"].Rows[i]["Логин"].ToString()))
                {
                    Connect.Table_Fill("Pas", "select password = crypt('" + PasswordBox.Password + "', password) as pas from users where login ='" + loginTextBox.Text + "'");
                    if (Connect.ds.Tables["Pas"].Rows[0]["pas"].ToString() == "True")
                    {
                        if (Connect.ds.Tables["User"].Rows[i]["Тип"].ToString() == "Администратор")
                        {
                            this.Hide();
                            Admin admin = new Admin();
                            admin.Show();
                            this.Close();
                            return;
                        }
                        else if (Connect.ds.Tables["User"].Rows[i]["Тип"].ToString() != "Администратор")
                        {
                            Connect.Table_Fill("UserVrach", "select * from medic_user where user_id = " + Connect.ds.Tables["User"].Rows[i]["Номер"].ToString());

                            if (Connect.ds.Tables["UserVrach"].Rows.Count > 0) id_userVrach = Connect.ds.Tables["UserVrach"].Rows[0]["medic_id"].ToString();

                            this.Hide();
                            Employee employee = new Employee();
                            employee.Show();
                            this.Close();
                            return;
                        }
                    }

                }
            }
            MessageBox.Show("Неверно введены логин или пароль!", "Ошибка");
            PasswordBox.Password = "";
        }
    

        private void loginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            loginTextBox.Text = "";
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            pass.Content = "";
        }

        private void loginTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("User", "select id as Номер, login as Логин, password as Пароль, " +
                "type as Тип from users order by id");
        }

        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                for (int i = 0; i < Connect.ds.Tables["User"].Rows.Count; i++)
                {
                    if ((loginTextBox.Text == Connect.ds.Tables["User"].Rows[i]["Логин"].ToString()))
                    {
                        Connect.Table_Fill("Pas", "select password = crypt('" + PasswordBox.Password + "', password) as pas from users where login ='" + loginTextBox.Text + "'");
                        if (Connect.ds.Tables["Pas"].Rows[0]["pas"].ToString() == "True")
                        {
                            if (Connect.ds.Tables["User"].Rows[i]["Тип"].ToString() == "Администратор")
                            {
                                this.Hide();
                                Admin admin = new Admin();
                                admin.Show();
                                this.Close();
                                return;
                            }
                            else if (Connect.ds.Tables["User"].Rows[i]["Тип"].ToString() != "Администратор")
                            {
                                Connect.Table_Fill("UserVrach", "select * from medic_user where user_id = " + Connect.ds.Tables["User"].Rows[i]["Номер"].ToString());

                                if (Connect.ds.Tables["UserVrach"].Rows.Count > 0) id_userVrach = Connect.ds.Tables["UserVrach"].Rows[0]["medic_id"].ToString();

                                this.Hide();
                                Employee employee = new Employee();
                                employee.Show();
                                this.Close();
                                return;
                            }
                        }
                        
                    }
                }
                MessageBox.Show("Неверно введены логин или пароль!", "Ошибка");
                PasswordBox.Password = "";
            }
        }
    }
}
