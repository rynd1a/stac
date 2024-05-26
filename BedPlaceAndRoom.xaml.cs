using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace stac
{
    public partial class BedPlaceAndRoom : Page
    {
        public BedPlaceAndRoom()
        {
            InitializeComponent();
        }

        private static int id_dep = -1;
        public static int id_pal = -1;
        private static int status = -1;
        public static int id_bed = -1;

        private void ButtonNewRoom_Click(object sender, RoutedEventArgs e)
        {
            id_pal = -1;
            CreateOrUpdateRoom createOrUpdateRoom = new CreateOrUpdateRoom();
            createOrUpdateRoom.ShowDialog();
            Fill_HospitalRoom(id_dep);
        }

        private void ButtonNewPlace_Click(object sender, RoutedEventArgs e)
        {
            id_bed = -1;
            CreateOrUpdateBedPlace createOrUpdateBedPlace = new CreateOrUpdateBedPlace();
            createOrUpdateBedPlace.ShowDialog();
            Fill_BedPlace(id_pal);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Dep", "select * from department");
            DepPal.ItemsSource = Connect.ds.Tables["Dep"].DefaultView;
            ButtonBack.Visibility = Visibility.Hidden;
            ButtonNewPlace.Visibility = Visibility.Hidden;
            ButtonDelPlace.Visibility = Visibility.Hidden;
            ButtonDelRoom.Visibility = Visibility.Hidden;
            ButtonNewRoom.Visibility = Visibility.Hidden;
            ButtonUpdRoom.Visibility = Visibility.Hidden;
        }

        private void Fill_HospitalRoom(int id_dep)
        {
            Connect.Table_Fill("Pal", "select id as Номер, department_id, status as Статус, type as Тип," +
                " gender as Пол from hospital_room where department_id=" + id_dep + " order by id");
            Tabl.ItemsSource = Connect.ds.Tables["Pal"].DefaultView;
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
            ButtonNewPlace.Visibility = Visibility.Hidden;
            ButtonDelPlace.Visibility = Visibility.Hidden;
            ButtonDelRoom.Visibility = Visibility.Visible;
            ButtonNewRoom.Visibility = Visibility.Visible; 
            ButtonUpdRoom.Visibility = Visibility.Hidden;
        }

        private void Fill_BedPlace(int id_pal)
        {
            Connect.Table_Fill("Bed", "select bed_place.id as Номер, type as Профиль, status as Статус from bed_place " +
                "join bed_place_hospital_room on bed_place.id=bed_place_hospital_room.bed_place_id where hospital_room_id =" + id_pal + " order by bed_place.id");
            Tabl.ItemsSource = Connect.ds.Tables["Bed"].DefaultView;
            Tabl.AutoGenerateColumns = true;
            Tabl.HeadersVisibility = DataGridHeadersVisibility.Column;
            Tabl.CanUserAddRows = false;
            Tabl.Columns[0].IsReadOnly = true;
            Tabl.Columns[1].IsReadOnly = true;
            Tabl.Columns[2].IsReadOnly = true;
            status = 2;
            ButtonBack.Visibility = Visibility.Visible;
            ButtonNewPlace.Visibility = Visibility.Visible;
            ButtonDelPlace.Visibility = Visibility.Visible;
            ButtonDelRoom.Visibility = Visibility.Hidden;
            ButtonNewRoom.Visibility = Visibility.Hidden;
            ButtonUpdRoom.Visibility = Visibility.Hidden;
        }

        private void Dep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < Connect.ds.Tables["Dep"].DefaultView.Count; i++)
                if (Connect.ds.Tables["Dep"].Rows[i]["id"].ToString() == DepPal.SelectedValue.ToString())
                    id_dep = Convert.ToInt32(Connect.ds.Tables["Dep"].DefaultView[i]["id"]);

            Fill_HospitalRoom(id_dep);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (status == -1) return;
            if (status == 1)
            {
                DataRowView row = (DataRowView)Tabl.CurrentItem;
                id_pal = Convert.ToInt32(row["Номер"]);

                Tabl.SelectedIndex = -1;
                Fill_BedPlace(id_pal);
                return;
            }
            if (status == 2)
            {
                DataRowView row = (DataRowView)Tabl.CurrentItem;
                id_bed = Convert.ToInt32(row["Номер"]);

                CreateOrUpdateBedPlace createOrUpdateBedPlace = new CreateOrUpdateBedPlace();
                createOrUpdateBedPlace.ShowDialog();
                Tabl.SelectedIndex = -1;
                Fill_BedPlace(id_pal);
                return;
            }
            
        }

        public static int getCurrentBedRowNumber()
        {
            return id_bed;
        }

        public static int getCurrentPalRowNumber()
        {
            return id_pal;
        }

       

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Fill_HospitalRoom(id_dep);
            id_pal = -1;
        }

        private void ButtonDelPlace_Click(object sender, RoutedEventArgs e)
        {
            int index = Tabl.SelectedIndex;
           
            if (index == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            id_bed = Convert.ToInt32(Connect.ds.Tables["Bed"].Rows[index]["Номер"]);
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                string sql = "delete from bed_place_hospital_room where bed_place_id =  " + id_bed + ";" +
                    "delete from bed_place where id = " + id_bed;
                if (!Connect.Modification_Execute(sql)) return;
                Connect.ds.Tables["Bed"].Rows.RemoveAt(index);
            }
        }

        private void Tabl_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (status != 1) return;
            ButtonUpdRoom.Visibility = Visibility.Visible;
        }

        private void ButtonUpdRoom_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)Tabl.SelectedItem;
            id_pal = Convert.ToInt32(row["Номер"]);

            CreateOrUpdateRoom createOrUpdateRoom = new CreateOrUpdateRoom();
            createOrUpdateRoom.ShowDialog();
            Fill_HospitalRoom(id_dep);
        }

        private void ButtonDelRoom_Click(object sender, RoutedEventArgs e)
        {
            int index = Tabl.SelectedIndex;
            string sql;
            if (index == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            id_pal = Convert.ToInt32(Connect.ds.Tables["Pal"].Rows[index]["Номер"]);

            Connect.Table_Fill("PalBed", "select * from bed_place_hospital_room where hospital_room_id =" + id_pal);

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (Connect.ds.Tables["PalBed"].DefaultView.Count != 0)
                {
                    MessageBox.Show("К палате привязаны койки. Удаление невозможно.", "Внимание");
                    return;
                }

                 sql = "delete from hospital_room where id = " + id_pal;
                if (!Connect.Modification_Execute(sql)) return;
                Connect.ds.Tables["Pal"].Rows.RemoveAt(index);
            }
        }
    }
}
