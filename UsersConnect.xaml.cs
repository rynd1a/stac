using System.Windows;

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для UsersConnect.xaml
    /// </summary>
    public partial class UsersConnect : Window
    {
        public UsersConnect()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("UserCon", "select * from users");
            User.ItemsSource = Connect.ds.Tables["UserCon"].DefaultView;

            Connect.Table_Fill("MedCon", "select id, (fam || ' ' || name || ' ' || patr) as name from medic");
            Medic.ItemsSource = Connect.ds.Tables["MedCon"].DefaultView;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;
            string id_user = "";
            string id_med = "";

            Connect.Table_Fill("UserConnect", "select medic_user.*, users.login from medic_user join users on medic_user.user_id=users.id");

            for (int i = 0; i < Connect.ds.Tables["UserCon"].DefaultView.Count; i++)
                if (Connect.ds.Tables["UserCon"].DefaultView[i]["id"].ToString() == User.SelectedValue.ToString())
                    id_user = Connect.ds.Tables["UserCon"].DefaultView[i]["id"].ToString();

            for (int i = 0; i < Connect.ds.Tables["MedCon"].DefaultView.Count; i++)
                if (Connect.ds.Tables["MedCon"].DefaultView[i]["id"].ToString() == Medic.SelectedValue.ToString())
                    id_med = Connect.ds.Tables["MedCon"].DefaultView[i]["id"].ToString();

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                for (int i = 0; i < Connect.ds.Tables["UserConnect"].DefaultView.Count; i++)
                    if (Connect.ds.Tables["UserConnect"].DefaultView[i]["medic_id"].ToString() == Medic.SelectedValue.ToString())
                    {
                        result = MessageBox.Show("К данному врачу уже привязан пользователь с логином " + 
                            Connect.ds.Tables["UserConnect"].DefaultView[i]["login"].ToString() + ". Обновить пользователя для данного врача?", "Внимание", buttons).ToString();
                        if (result == "No") return;
                        else if (result == "Yes")
                        {
                            sql = "update medic_user set user_id = " + id_user + " where medic_id = " + id_med;
                            if (!Connect.Modification_Execute(sql)) return;
                        }
                    }
                sql = "insert into medic_user(medic_id, user_id) values (" + id_med + ", " + id_user + ")";
                if (!Connect.Modification_Execute(sql)) return;
            }

            this.Close();
        }
    }
}
