using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            Table_Fill();
        }

        private void Table_Fill()
        {
            Connect.Table_Fill("Medics", "select m.id as Номер, fam as Фамилия, m.name as Имя, patr as Отчество, d.name as Отделение from medic m join department d on m.department_id=d.id order by m.id");
            MedicsTable.ItemsSource = Connect.ds.Tables["Medics"].DefaultView;
            MedicsTable.AutoGenerateColumns = true;
            MedicsTable.HeadersVisibility = DataGridHeadersVisibility.Column;
            MedicsTable.CanUserAddRows = false;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)MedicsTable.CurrentItem;
            id_vrach = Convert.ToInt32(row["Номер"]);

            NewMedic newMedic = new NewMedic();
            newMedic.ShowDialog();
            MedicsTable.SelectedIndex = -1;
            Table_Fill();
        }

        public static int getCurrentRowNumber()
        {
            return id_vrach;
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            NewMedic newMedic = new NewMedic();
            newMedic.ShowDialog();
            id_vrach = -1;
            Table_Fill();
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int index = MedicsTable.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            id_vrach = Convert.ToInt32(Connect.ds.Tables["Medics"].Rows[index]["Номер"]);
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                string sql = "delete from medic where id = " + id_vrach;
                if (!Connect.Modification_Execute(sql)) return;
                Connect.ds.Tables["Medics"].Rows.RemoveAt(index);
            }
        }
    }
}
