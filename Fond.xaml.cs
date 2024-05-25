using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для Fond.xaml
    /// </summary>
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

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (status == -1) return;
            if (status == 1)
            {
                DataRowView row = (DataRowView)Tabl.CurrentItem;
                id_fondPal = Convert.ToInt32(row["Номер"]);

                Tabl.SelectedIndex = -1;
                Fill_BedPlace(id_fondPal);
                return;
            }
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
            Connect.Table_Fill("FondBed", "select s.id as Случай, bp.id as Номер, bp.status as Статус, " +
                "(fam || substring(name, 1, 1) || '. ' || substring(patr, 1, 1)) as Пациент, s.diagnosis as Диагноз " +
                "from bed_place bp join bed_place_hospital_room bphr on bp.id=bphr.bed_place_id left " +
                "join stac_sluch s on bphr.id=s.place_id left join patient p on s.patient_id=p.id " +
                "where hospital_room_id = " + id_fondPal + " order by bp.id");
            Tabl.ItemsSource = Connect.ds.Tables["FondBed"].DefaultView;
            Tabl.AutoGenerateColumns = true;
            Tabl.HeadersVisibility = DataGridHeadersVisibility.Column;
            Tabl.CanUserAddRows = false;
            Tabl.Columns[0].Visibility = Visibility.Hidden;
            Tabl.Columns[0].IsReadOnly = true;
            Tabl.Columns[1].IsReadOnly = true;
            Tabl.Columns[2].IsReadOnly = true;
            Tabl.Columns[3].IsReadOnly = true;
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
        }

        private void Tabl_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ButtonAction.Visibility = Visibility.Hidden;
            if (status != 2) return;
            string pac;
            string stat;

            DataRowView row = (DataRowView)Tabl.CurrentItem;
            pac = (row["Пациент"]).ToString();
            stat = (row["Статус"]).ToString();

            if (pac != "") 
            { 
                ButtonAction.Content = "Выписать"; 
                action = 1;
                id_sluch = Convert.ToInt32(row["Случай"]);
            }
            else if (stat == "Свободна") 
            { 
                ButtonAction.Content = "Прикрепить"; 
                action = 2;
            }
            else if (stat != "Свободна") 
            { 
                action = 0; 
                return; 
            }
            ButtonAction.Visibility = Visibility.Visible;
        }

        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            CreateOrUpdateStacSluch createOrUpdateStacSluch = new CreateOrUpdateStacSluch();
            createOrUpdateStacSluch.ShowDialog();
        }
    }
}
