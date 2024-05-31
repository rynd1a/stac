using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для SearchPac.xaml
    /// </summary>
    public partial class SearchPac : Window
    {
        public SearchPac()
        {
            InitializeComponent();
        }

        public static int id_pac = -1;

        private void ButtonDelDoc_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Table_Fill()
        {
            if (Fond.getCurrentGenderRowNumber() != "Общий")
            {
                Connect.Table_Fill("Pac", "select id as Номер, (fam || ' ' || name || ' ' || patr) as ФИО, birth_date as \"Дата рождения\"," +
               " gender as Пол, phone_number as Телефон, email as \"Электронная почта\", note as Примечание " +
               " from patient where gender = '" + Fond.getCurrentGenderRowNumber() + "' ");
            }
            else
            {
                Connect.Table_Fill("Pac", "select id as Номер, (fam || ' ' || name || ' ' || patr) as ФИО, birth_date as \"Дата рождения\"," +
                   " gender as Пол, phone_number as Телефон, email as \"Электронная почта\", note as Примечание " +
                   " from patient");
            }
            PacTable.ItemsSource = Connect.ds.Tables["Pac"].DefaultView;
            if ((PacTable.Columns[2] as DataGridTextColumn).Binding.StringFormat != "dd.MM.yyyy")
                (PacTable.Columns[2] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            PacTable.AutoGenerateColumns = true;
            PacTable.HeadersVisibility = DataGridHeadersVisibility.Column;
            PacTable.CanUserAddRows = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Table_Fill();
        }

        private void FIO_KeyUp(object sender, KeyEventArgs e)
        {
            Connect.ds.Tables["Pac"].DefaultView.RowFilter = "ФИО like'%" + FIO.Text + "%'";
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)PacTable.CurrentItem;
            id_pac = Convert.ToInt32(row["Номер"]);

            this.Close();
        }
    }
}
