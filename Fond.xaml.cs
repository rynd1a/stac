using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace stac
{
    public partial class Fond : Page
    {
        public Fond()
        {
            InitializeComponent();
        }

        private static int id_dep = -1;
        public static int id_fondPal = -1;
        public static int id_fondBed = -1;
        private static int status = -1;
        public static int action = 0; // 1 - Выписать, 2 - Прикрепить
        public static int id_sluch = -1;
        public static int id_place = -1;
        public static string gender = "";

        DataRowView row;


        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (status == -1) return;
            if (status == 1)
            {
                DataRowView row = (DataRowView)Tabl.CurrentItem;
                id_fondPal = Convert.ToInt32(row["Номер"]);
                gender = row["Пол"].ToString();

                Tabl.SelectedIndex = -1;
                Fill_BedPlace(id_fondPal);
                return;
            }
            
        }
        public static string getCurrentGenderRowNumber()
        {
            return gender;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (id_fondPal != -1) return;
            Connect.Table_Fill("DepPal", "select id, name from department");
            Connect.Table_Fill("DepVrach", "select department_id from medic where id= " + Login.id_userVrach);
            id_dep = Convert.ToInt32(Connect.ds.Tables["DepVrach"].Rows[0]["department_id"]);
            Fill_HospitalRoom(id_dep);
            DepPal.ItemsSource = Connect.ds.Tables["DepPal"].DefaultView;

            for (int i = 0; i < Connect.ds.Tables["DepPal"].DefaultView.Count; i++)
            {
                if (
                    Connect.ds.Tables["DepPal"].DefaultView[i]["id"].ToString()
                    != id_dep.ToString()
                    )
                {
                    continue;
                }

                DepPal.SelectedIndex = i;
            }
        }

        public static int getCurrentRowNumber()
        {
            return id_sluch;
        }

        public static int getCurrentPlace()
        {
            return id_place;
        }

        private void Fill_HospitalRoom(int id_dep)
        {
            if (Login.id_userVrach == "")
                Connect.Table_Fill("FondPal", "select id as Номер, department_id, status as Статус, type as Тип," +
                " gender as Пол from hospital_room order by id");
            else
                Connect.Table_Fill("FondPal", "select id as Номер, department_id, status as Статус, type as Тип," +
                " gender as Пол from hospital_room where department_id=" + id_dep + " order by id");
            Tabl.ItemsSource = Connect.ds.Tables["FondPal"].DefaultView;
            Tabl.AutoGenerateColumns = true;
            Tabl.HeadersVisibility = DataGridHeadersVisibility.Column;
            Tabl.CanUserAddRows = false;
            Tabl.Columns[1].Visibility = Visibility.Hidden;
            Tabl.Columns[0].IsReadOnly = true;
            Tabl.Columns[1].IsReadOnly = true;
            Tabl.Columns[2].IsReadOnly = true;
            Tabl.Columns[3].IsReadOnly = true;
            Tabl.Columns[4].IsReadOnly = true;
            status = 1;
            ButtonBack.Visibility = Visibility.Hidden;
            ButtonAction.Visibility = Visibility.Hidden;
        }

        private void Fill_BedPlace(int id_fondPal)
        {
            Connect.Table_Fill("FondBed", "with tbl as (select s.id, (p.fam || substring(p.name, 1, 1) || '. ' || substring(p.patr, 1, 1)) as pac, s.diagnosis, place_id " +
                " from stac_sluch s " +
                " join patient p on s.patient_id=p.id where date_close is null) select bp.id as Номер, bp.status as Статус, bphr.id as Место, tbl.id as Случай, " +
                " pac as Пациент, tbl.diagnosis as Диагноз from bed_place bp join bed_place_hospital_room bphr on bp.id=bphr.bed_place_id " +
                "left join tbl on bphr.id=tbl.place_id where hospital_room_id = " + id_fondPal + " order by bp.id");
            Tabl.ItemsSource = Connect.ds.Tables["FondBed"].DefaultView;
            Tabl.AutoGenerateColumns = true;
            Tabl.HeadersVisibility = DataGridHeadersVisibility.Column;
            Tabl.CanUserAddRows = false;
            Tabl.Columns[2].Visibility = Visibility.Hidden;
            Tabl.Columns[3].Visibility = Visibility.Hidden;
            Tabl.Columns[0].IsReadOnly = true;
            Tabl.Columns[1].IsReadOnly = true;
            Tabl.Columns[4].IsReadOnly = true;
            Tabl.Columns[5].IsReadOnly = true;
            status = 2;
            ButtonBack.Visibility = Visibility.Visible;
            ButtonAction.Visibility = Visibility.Hidden;
        }

        private void Dep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < Connect.ds.Tables["DepPal"].DefaultView.Count; i++)
                if (Connect.ds.Tables["DepPal"].Rows[i]["id"].ToString() == DepPal.SelectedValue.ToString())
                    id_dep = Convert.ToInt32(Connect.ds.Tables["DepPal"].DefaultView[i]["id"]);

            Fill_HospitalRoom(id_dep);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            status = 1;
            Fill_HospitalRoom(id_dep);
            id_fondPal = -1;
            action = 0;
        }

   
        
        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            CreateOrUpdateStacSluch createOrUpdateStacSluch = new CreateOrUpdateStacSluch();
            if (action == 2)
            {
                id_place = Convert.ToInt32(row["Место"]);
                createOrUpdateStacSluch.ShowDialog();
                Fill_BedPlace(id_fondPal);
                return;
            }
            // row = (DataRowView)Tabl.CurrentItem;
            id_sluch = Convert.ToInt32(row["Случай"]);
            createOrUpdateStacSluch.ShowDialog();
            Fill_BedPlace(id_fondPal);
            row = null;
        }

        

        private void Tabl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Tabl_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (status != 2) return;
            if (id_sluch != -1)
            {
                id_sluch = -1;
            }

            string pac;
            string stat;
            row = (DataRowView)Tabl.CurrentItem;
            if (row == null) return;
            pac = (row["Случай"]).ToString();
            stat = (row["Статус"]).ToString();

            if (pac != "")
            {
                ButtonAction.Content = "Выписать";
                action = 1;
            }
            else if (stat == "Свободна")
            {
                ButtonAction.Content = "Прикрепить";
                action = 2;
            }
            else if (stat != "Свободна")
            {
                ButtonAction.Visibility = Visibility.Hidden;
                action = 0;
                return;
            }
            ButtonAction.Visibility = Visibility.Visible;
        }
    }
}
