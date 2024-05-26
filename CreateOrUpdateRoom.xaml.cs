using System;
using System.Windows;

namespace stac
{
    public partial class CreateOrUpdateRoom : Window
    {
        public CreateOrUpdateRoom()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            BedPlaceAndRoom.id_pal = -1;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            BedPlaceAndRoom.id_pal = -1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Deps", "select id, name from department");
            Dep.ItemsSource = Connect.ds.Tables["Deps"].DefaultView;

            if (BedPlaceAndRoom.getCurrentPalRowNumber() == -1) return;

            Connect.Table_Fill("UpdPal", "select * from hospital_room where id=" + BedPlaceAndRoom.getCurrentPalRowNumber());

            for (int i = 0; i < Connect.ds.Tables["Deps"].DefaultView.Count; i++)
            {
                if (
                    Connect.ds.Tables["Deps"].DefaultView[i]["id"].ToString()
                    != Connect.ds.Tables["UpdPal"].Rows[0]["department_id"].ToString()
                    )
                {
                    continue;
                }

                Dep.SelectedIndex = i;
            }

            for (int i = 0; i < Status.Items.Count; i++)
            {
                if (Status.Items[i].ToString() == Connect.ds.Tables["UpdPal"].DefaultView[0]["status"].ToString()) continue;
                Status.SelectedIndex = i;
            }

            for (int i = 0; i < Type.Items.Count; i++)
            {
                if (Type.Items[i].ToString() == Connect.ds.Tables["UpdPal"].DefaultView[0]["type"].ToString()) continue;
                Type.SelectedIndex = i;
            }

            for (int i = 0; i < Gender.Items.Count; i++)
            {
                if (Gender.Items[i].ToString() == Connect.ds.Tables["UpdPal"].DefaultView[0]["gender"].ToString()) continue;
                Gender.SelectedIndex = i;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string id_dep = "";

            if (Dep.Text == "")
            {
                MessageBox.Show("Отделение палаты является обязательным для заполнения", "Внимание");
                return;
            }

            for (int i = 0; i < Connect.ds.Tables["Deps"].DefaultView.Count; i++)
                if (Connect.ds.Tables["Deps"].DefaultView[i]["id"].ToString() == Dep.SelectedValue.ToString())
                    id_dep = Connect.ds.Tables["Deps"].DefaultView[i]["id"].ToString();

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (Status.Text == "")
                {
                    MessageBox.Show("Статус палаты является обязательным для заполнения", "Внимание");
                    return;
                }
                if (Type.Text == "")
                {
                    MessageBox.Show("Тип палаты является обязательным для заполнения", "Внимание");
                    return;
                }
                if (Gender.Text == "")
                {
                    MessageBox.Show("Пол является обязательным для заполнения", "Внимание");
                    return;
                }
                

                if (BedPlaceAndRoom.getCurrentPalRowNumber() != -1)
                {
                    sql = "update hospital_room set status='" + Status.Text + "', type='" +
                        Type.Text + "', gender='" + Gender.Text + "', department_id=" + id_dep + " where id=" + BedPlaceAndRoom.getCurrentPalRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    sql = "insert into hospital_room(department_id, status, type, gender) values(" + id_dep +
                        ", '" + Status.Text + "', '" + Type.Text + "', '" + Gender.Text + "')";
                    if (!Connect.Modification_Execute(sql)) return;
                }
            }

            this.Close();
        }
    }
}
