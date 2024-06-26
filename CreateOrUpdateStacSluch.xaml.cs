﻿using System;
using System.Windows;

namespace stac
{
    public partial class CreateOrUpdateStacSluch : Window
    {
        public CreateOrUpdateStacSluch()
        {
            InitializeComponent();
        }

        private static int id_pac = -1;

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchPac searchPac = new SearchPac();
            searchPac.ShowDialog();

            id_pac = SearchPac.id_pac;
            if (id_pac == -1) return;
            Connect.Table_Fill("PacSearch", "select id, (fam || ' ' || name || ' ' || patr) as name from patient where id=" + id_pac);
            Pac.Text = Connect.ds.Tables["PacSearch"].Rows[0]["name"].ToString();

            return;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Fond.action = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Medic", "select id, (fam || ' ' || name || ' ' || patr) as name from medic");
            Vrach.ItemsSource = Connect.ds.Tables["Medic"].DefaultView;
            
            if (Fond.action == 2) return;
            ButtonSearch.Visibility = Visibility.Hidden;
            ButtonSave.Visibility = Visibility.Hidden;
            Rez.Visibility = Visibility.Visible;
            CloseS.Visibility = Visibility.Visible;
            LabelCloseS.Visibility = Visibility.Visible;
            LabelRez.Visibility = Visibility.Visible;
            Connect.Table_Fill("UpdSluch", "select s.id, patient_id, (p.fam || ' ' || p.name || ' ' || p.patr) as Пациент, " +
               "medic_id, place_id, diagnosis, s.status, date_create, date_close, result from stac_sluch s " +
               "join patient p on s.patient_id=p.id where s.id=" + Fond.getCurrentRowNumber() + " order by s.id");

            Pac.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["Пациент"].ToString();
            Diag.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["diagnosis"].ToString();
            Status.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["status"].ToString();
            OpenS.Text = Convert.ToDateTime(Connect.ds.Tables["UpdSluch"].Rows[0]["date_create"]).ToString();
            if (Connect.ds.Tables["UpdSluch"].Rows[0]["date_close"].ToString() == "") CloseS.Text = "";
            else CloseS.Text = Convert.ToDateTime(Connect.ds.Tables["UpdSluch"].Rows[0]["date_close"]).ToString();

            for (int i = 0; i < Connect.ds.Tables["Medic"].DefaultView.Count; i++)
            {
                if (
                    Connect.ds.Tables["Medic"].DefaultView[i]["id"].ToString()
                    != Connect.ds.Tables["UpdSluch"].Rows[0]["medic_id"].ToString()
                    ) continue;
               
                Vrach.SelectedIndex = i;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                sql = "update stac_sluch set diagnosis='" + Diag.Text + "', status='" +
                    Status.Text + "', date_close='" + CloseS.Text + "', result='" +
                    Rez.Text + "' where id=" + Fond.getCurrentRowNumber();
                if (!Connect.Modification_Execute(sql)) return;

                sql = "update bed_place set status='Свободна' where id = (select bed_place_id from bed_place_hospital_room where id=" + Connect.ds.Tables["UpdSluch"].Rows[0]["place_id"].ToString() +")";
                if (!Connect.Modification_Execute(sql)) return;

            }

            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;
            string id_medic = "";

            if (Vrach.SelectedValue == null)
            {
                MessageBox.Show("Лечащий врач является обязательным для заполнения", "Внимание");
                return;
            }

            for (int i = 0; i < Connect.ds.Tables["Medic"].DefaultView.Count; i++)
                if (Connect.ds.Tables["Medic"].DefaultView[i]["id"].ToString() == Vrach.SelectedValue.ToString())
                    id_medic = Connect.ds.Tables["Medic"].DefaultView[i]["id"].ToString();

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Прикрепить пациента?", "", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (id_pac == -1)
                {
                    MessageBox.Show("Пациент является обязательным для заполнения", "Внимание");
                    return;
                }

                if (OpenS.Text == "")
                {
                    MessageBox.Show("Дата прикрепления является обязательной для заполнения", "Внимание");
                    return;
                }

                sql = "insert into stac_sluch(patient_id, medic_id, place_id, diagnosis, status, date_create) " +
                    " values(" + id_pac + ", " + id_medic + ", " + Fond.getCurrentPlace() + ", '" + Diag.Text + "', '" 
                    + Status.Text + "', '" + OpenS.Text + "')";
                if (!Connect.Modification_Execute(sql)) return;

                sql = "update bed_place set status='Занята' where id = (select bed_place_id from bed_place_hospital_room where id=" + Fond.getCurrentPlace() + ")";
                if (!Connect.Modification_Execute(sql)) return;
            }

            this.Close();
        }
    }
}
