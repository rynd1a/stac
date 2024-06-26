﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;

namespace stac
{
    public partial class CreateorUpdatePac : Window
    {
        public CreateorUpdatePac()
        {
            InitializeComponent();
        }

        public static int id_adr = -1;
        public static int id_doc = -1;


        public static int getCurrentAdrRowNumber()
        {
            return id_adr;
        }

        public static int getCurrentDocRowNumber()
        {
            return id_doc;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Nam.Text = "";
            Fam.Text = "";
            Patr.Text = "";
            Phone.Text = "";
            Email.Text = "";
            Note.Text = "";

            Pacients.id_pac = -1;
            id_adr = -1;    
            id_doc = -1;    
        }

        private void Table_Fill()
        {
            Connect.Table_Fill("UpdAdr", "select id, adr from address where patient_id=" + Pacients.getCurrentRowNumber());
            Connect.Table_Fill("UpdDoc", "select id, (type || ' ' || ser || ' ' || num) as doc from document where patient_id=" + Pacients.getCurrentRowNumber());

            AdrTable.ItemsSource = Connect.ds.Tables["UpdAdr"].DefaultView;
            AdrTable.AutoGenerateColumns = true;
            AdrTable.HeadersVisibility = DataGridHeadersVisibility.None;
            AdrTable.CanUserAddRows = false;
            AdrTable.Columns[0].Visibility = Visibility.Hidden;

            DocTable.ItemsSource = Connect.ds.Tables["UpdDoc"].DefaultView;
            DocTable.AutoGenerateColumns = true;
            DocTable.HeadersVisibility = DataGridHeadersVisibility.None;
            DocTable.CanUserAddRows = false;
            DocTable.Columns[0].Visibility = Visibility.Hidden;
        }

        private void AdrRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)AdrTable.CurrentItem;
            id_adr = Convert.ToInt32(row["id"]);

            PacAdr pacAdr = new PacAdr();
            pacAdr.ShowDialog();
            AdrTable.SelectedIndex = -1;
            Table_Fill();
        }

        private void DocRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)DocTable.CurrentItem;
            id_doc = Convert.ToInt32(row["id"]);

            PacDoc pacDoc = new PacDoc();
            pacDoc.ShowDialog();
            DocTable.SelectedIndex = -1;
            Table_Fill();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {            
            if (Pacients.getCurrentRowNumber() == -1) return;
            Table_Fill();
            Connect.Table_Fill("UpdPac", "select * from patient where id=" + Pacients.getCurrentRowNumber());

            Nam.Text = Connect.ds.Tables["UpdPac"].Rows[0]["name"].ToString();
            Fam.Text = Connect.ds.Tables["UpdPac"].Rows[0]["fam"].ToString();
            Patr.Text = Connect.ds.Tables["UpdPac"].Rows[0]["patr"].ToString();
            Phone.Text = Connect.ds.Tables["UpdPac"].Rows[0]["phone_number"].ToString();
            Email.Text = Connect.ds.Tables["UpdPac"].Rows[0]["email"].ToString();
            Note.Text = Connect.ds.Tables["UpdPac"].Rows[0]["note"].ToString();
            Birth.Text = Convert.ToDateTime(Connect.ds.Tables["UpdPac"].Rows[0]["birth_date"]).ToString();

            for (int i = 0; i < Gender.Items.Count; i++)
            {
                if (Gender.Items[i].ToString() == Connect.ds.Tables["UpdPac"].DefaultView[0]["gender"].ToString())
                {
                    continue;
                }
                Gender.SelectedIndex = i;
            }
        }

        private void ButtonAddAdr_Click(object sender, RoutedEventArgs e)
        {
            if (Pacients.getCurrentRowNumber() != -1)
            {
                if (Connect.ds.Tables["UpdAdr"].Rows.Count == 2)
                {
                    MessageBox.Show("Пациент не может иметь более двух типов адресов.", "Ошибка");
                    return;
                }
            }

            PacAdr pacAdr = new PacAdr();
            pacAdr.ShowDialog();
            AdrTable.SelectedIndex = -1;
            Table_Fill();
        }

        private void ButtonAddDoc_Click(object sender, RoutedEventArgs e)
        {
            if (Pacients.getCurrentRowNumber() != -1)
            {
                if (Connect.ds.Tables["UpdDoc"].Rows.Count == 3)
                {
                    MessageBox.Show("Пациент не может иметь более трех типов документов.", "Ошибка");
                    return;
                }
            }

            PacDoc pacDoc = new PacDoc();
            pacDoc.ShowDialog();
            DocTable.SelectedIndex = -1;
            Table_Fill();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (Nam.Text == "" || Nam.Text == "Имя")
                {
                    MessageBox.Show("Имя пациента является обязательным для заполнения", "Ошибка");
                    return;
                }
                if (Fam.Text == "" || Fam.Text == "Фамилия")
                {
                    MessageBox.Show("Фамилия пациента является обязательной для заполнения", "Ошибка");
                    return;
                }

                if (Pacients.getCurrentRowNumber() != -1)
                {
                    sql = "update patient set fam='" + Fam.Text + "', name='" +
                        Nam.Text + "', patr='" + Patr.Text + "', birth_date='" +
                        Birth.Text + "', gender='"+ Gender.Text + "', phone_number='"+
                        Phone.Text + "', email='" + Email.Text + "', note='" + Note.Text + 
                        "' where id=" + Pacients.getCurrentRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    sql = "insert into patient(fam, name, patr, birth_date, gender, phone_number, email, note) values('" + Fam.Text +
                        "', '" + Nam.Text + "', '" + Patr.Text + "', '" + Birth.Text + "', '" + Gender.Text + 
                        "', '" + Phone.Text + "', '" + Email.Text + "', '" + Note.Text + "')";
                    if (!Connect.Modification_Execute(sql)) return;
                }
            }

            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
