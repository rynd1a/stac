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
    /// Логика взаимодействия для Departs.xaml
    /// </summary>
    public partial class Departs : Page
    {
        public Departs()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Depart", "select id as Номер, name as Наименование from department");
            DepartsTable.ItemsSource = Connect.ds.Tables["Depart"].DefaultView;
            DepartsTable.AutoGenerateColumns = true;
            DepartsTable.HeadersVisibility = DataGridHeadersVisibility.Column;
            DepartsTable.CanUserAddRows = false;
            DepartsTable.Columns[0].IsReadOnly = true;
        }

        private void DepartsTable_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string result;
                string sql;
                DataRowView row = (DataRowView)DepartsTable.SelectedItems[0];
                int id = DepartsTable.SelectedIndex;
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
                        NavigationService.Navigate(new Departs());
                    }
                    else
                    {
                        sql = "insert into department(name) values('" + row["Наименование"] + "')";
                        if (!Connect.Modification_Execute(sql))
                            return;
                        NavigationService.Navigate(new Departs());
                    }
                }
            }
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int index = DepartsTable.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            int id = Convert.ToInt32(Connect.ds.Tables["Depart"].Rows[index]["Номер"]);
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                string sql = "delete from department where id = " + id;
                if (!Connect.Modification_Execute(sql))
                    return;
                Connect.ds.Tables["Depart"].Rows.RemoveAt(index);
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            Connect.ds.Tables["Depart"].Rows.Add(new object[] { });
        }

    }
}
