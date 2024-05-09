using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
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
    /// Логика взаимодействия для Medics.xaml
    /// </summary>
    public partial class Medics : Page
    {
        public Medics()
        {
            InitializeComponent();
        }

        public static int id_vrach = -1;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Medic", "select m.id as Номер, fam as Фамилия, m.name as Имя, patr as Отчество, d.name as Отделение, department_id from medic m join department d on m.department_id=d.id order by m.id");
            FieldForm_Fill();
        }

        private void FieldForm_Fill()
        {
            MedicsTable.ItemsSource = Connect.ds.Tables["Medic"].DefaultView;
            MedicsTable.AutoGenerateColumns = true;
            MedicsTable.HeadersVisibility = DataGridHeadersVisibility.Column;
            MedicsTable.CanUserAddRows = false;
            MedicsTable.Columns[0].IsReadOnly = true;
            MedicsTable.Columns[1].IsReadOnly = true;
            MedicsTable.Columns[2].IsReadOnly = true;
            MedicsTable.Columns[3].IsReadOnly = true;
            MedicsTable.Columns[4].IsReadOnly = true;
            MedicsTable.Columns[5].Visibility = Visibility.Hidden;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)MedicsTable.SelectedItems[0];
            id_vrach = Convert.ToInt32(row["Номер"]);

            NewMedic newMedic = new NewMedic();
            newMedic.ShowDialog();
            MedicsTable.SelectedIndex = -1;
            return;
        }

        public static int IdVrach() { return id_vrach; }

        private void MedicsTable_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string result;
                string sql;
                DataRowView row = (DataRowView)MedicsTable.SelectedItems[0];
                int id = MedicsTable.SelectedIndex;
                MessageBoxButton buttons = MessageBoxButton.YesNo;

                if (id == -1) MessageBox.Show("Выберите строку!");

                result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
                if (result == "No") return;
                else if (result == "Yes")
                {
                    if (row["Номер"].ToString() != "")
                    {
                        sql = "update department set name='" +
                        row["Наименование"] +
                        "' where id = " + row["Номер"];

                        if (!Connect.Modification_Execute(sql))
                            return;
                        Connect.ds.Tables["Medic"].Rows[id].ItemArray = new object[] { row["Номер"], row["Наименование"] };
                    }
                    else
                    {
                        sql = "insert into department(name) values('" + row["Наименование"] + "')";
                        if (!Connect.Modification_Execute(sql))
                            return;
                        Connect.ds.Tables["Medic"].Rows[id].ItemArray = new object[] { row["Номер"], row["Наименование"] };
                    }
                }
            }
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int id = MedicsTable.SelectedIndex;
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
                sql = "delete from medic where id = " + id;
                if (!Connect.Modification_Execute(sql))
                    return;
                Connect.ds.Tables["Medic"].Rows.RemoveAt(id);
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            NewMedic newMedic = new NewMedic();
            newMedic.ShowDialog();
            return;
        }

        private void MedicsTable_GotFocus(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Medics());
        }
    }
}
