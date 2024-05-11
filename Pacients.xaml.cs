using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для Pacients.xaml
    /// </summary>
    public partial class Pacients : Page
    {
        public Pacients()
        {
            InitializeComponent();
        }

        public static int id_pac = -1;

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateorUpdatePac createorUpdatePac = new CreateorUpdatePac();
            createorUpdatePac.ShowDialog();
            return;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FillTable();
        }

        public static int getCurrentRowNumber()
        {
            return id_pac;
        }

        private void FillTable()
        {
            Connect.Table_Fill("Pac", "select id as Номер, (fam || ' ' || name || ' ' || patr) as ФИО, birth_date as \"Дата рождения\"," +
               " gender as Пол, phone_number as Телефон, email as \"Электронная почта\", note as Примечание " +
               " from patient order by id");
            PacTable.ItemsSource = Connect.ds.Tables["Pac"].DefaultView;
             if ((PacTable.Columns[2] as DataGridTextColumn).Binding.StringFormat != "dd.MM.yyyy")
                (PacTable.Columns[2] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            PacTable.AutoGenerateColumns = true;
            PacTable.HeadersVisibility = DataGridHeadersVisibility.Column;
            PacTable.CanUserAddRows = false;
            PacTable.Columns[0].IsReadOnly = true;
            PacTable.Columns[1].IsReadOnly = true;
            PacTable.Columns[2].IsReadOnly = true;
            PacTable.Columns[3].IsReadOnly = true;
            PacTable.Columns[4].IsReadOnly = true;
            PacTable.Columns[5].IsReadOnly = true;
            PacTable.Columns[6].IsReadOnly = true;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)PacTable.CurrentItem;
            id_pac = Convert.ToInt32(row["Номер"]);

            CreateorUpdatePac createorUpdatePac = new CreateorUpdatePac();
            createorUpdatePac.ShowDialog();
            PacTable.SelectedIndex = -1;
            FillTable();
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int id = PacTable.SelectedIndex;
            if (id == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }

            string result;
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                string sql = "delete from patient where id = " + id;
                if (!Connect.Modification_Execute(sql))
                    return;
                Connect.ds.Tables["Pac"].Rows.RemoveAt(id);
            }
        }

    }
}
