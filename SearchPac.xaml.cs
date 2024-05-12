﻿using System;
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
            Connect.Table_Fill("Pac", "select id as Номер, (fam || ' ' || name || ' ' || patr) as ФИО, birth_date as \"Дата рождения\"," +
               " gender as Пол, phone_number as Телефон, email as \"Электронная почта\", note as Примечание " +
               " from patient");
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Table_Fill();
        }

        private void FIO_KeyUp(object sender, KeyEventArgs e)
        {
            Connect.ds.Tables["Pac"].DefaultView.RowFilter = "ФИО like'%" + FIO.Text + "%'";
        }

        private void FIO_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FIO.Text == "ФИО")
                FIO.Text = "";
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)PacTable.CurrentItem;
            id_pac = Convert.ToInt32(row["Номер"]);

            this.Close();
        }
    }
}
