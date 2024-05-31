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
            Table_Fill();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Table_Fill();
        }

        public static int getCurrentRowNumber()
        {
            return id_pac;
        }

        private void Table_Fill()
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
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)PacTable.CurrentItem;
            id_pac = Convert.ToInt32(row["Номер"]);

            CreateorUpdatePac createorUpdatePac = new CreateorUpdatePac();
            createorUpdatePac.ShowDialog();
            PacTable.SelectedIndex = -1;
            Table_Fill();
            NavigationService.Navigate(new Pacients());
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int index = PacTable.SelectedIndex;

            if (index == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            id_pac = Convert.ToInt32(Connect.ds.Tables["Pac"].Rows[index]["Номер"]);
            Connect.Table_Fill("PacSluch", "select count(id) from stac_sluch where patient_id=" + id_pac);

            if (Connect.ds.Tables["PacSluch"].Rows.Count > 0)
            {
                MessageBox.Show("У пациента есть госпитализация. Удаление невозможно", "Ошибка");
                return;
            }

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                string sql = "delete from document where patient_id = " + id_pac + "; delete from address where patient_id = " 
                    + id_pac + "; delete from patient where id = " + id_pac;
                if (!Connect.Modification_Execute(sql))
                    return;
            }
            NavigationService.Navigate(new Pacients());
        }

    }
}
