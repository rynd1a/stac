using System;
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
    /// Логика взаимодействия для StacSluch.xaml
    /// </summary>
    public partial class StacSluch : Page
    {
        public StacSluch()
        {
            InitializeComponent();
        }

        public static int id_sluch = -1;

        public static int getCurrentRowNumber()
        {
            return id_sluch;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            id_sluch = -1;
            CreateOrUpdateStacSluch createOrUpdateStacSluch = new CreateOrUpdateStacSluch();
            createOrUpdateStacSluch.ShowDialog();
            Table_Fill();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)Sluch.CurrentItem;
            id_sluch = Convert.ToInt32(row["Номер"]);

            CreateOrUpdateStacSluch createOrUpdateStacSluch = new CreateOrUpdateStacSluch();
            createOrUpdateStacSluch.ShowDialog();
            Sluch.SelectedIndex = -1;
            Table_Fill();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Table_Fill();
        }

        private void Table_Fill()
        {
            Connect.Table_Fill("StacSluch", "select s.id as Номер, patient_id, (p.fam || ' ' || substring(p.name, 1, 1) || '.' || substring(p.patr, 1, 1)) as Пациент," +
                " medic_id, (m.fam || ' ' || substring(m.name, 1, 1) || '.' || substring(m.patr, 1, 1)) as Врач, place_id, dep.name as Отеделение, " +
                "diagnosis as Диагноз, s.status as Статус, date_create as \"Дата открытия\", \r\ndate_close as \"Дата закрытия\", result as Результат " +
                "from stac_sluch s join patient p on s.patient_id=p.id join medic m on s.medic_id=m.id join bed_place_hospital_room bp on s.place_id=bp.id " +
                "join hospital_room hr on bp.hospital_room_id=hr.id join department dep on hr.department_id=dep.id");
            Sluch.ItemsSource = Connect.ds.Tables["StacSluch"].DefaultView;
            Sluch.AutoGenerateColumns = true;
            Sluch.HeadersVisibility = DataGridHeadersVisibility.Column;
            Sluch.CanUserAddRows = false;
            Sluch.Columns[0].IsReadOnly = true;
            Sluch.Columns[1].IsReadOnly = true;
            Sluch.Columns[2].IsReadOnly = true;
            Sluch.Columns[3].IsReadOnly = true;
            Sluch.Columns[4].IsReadOnly = true;
            if ((Sluch.Columns[9] as DataGridTextColumn).Binding.StringFormat != "dd.MM.yyyy")
                (Sluch.Columns[9] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            if ((Sluch.Columns[10] as DataGridTextColumn).Binding.StringFormat != "dd.MM.yyyy")
                (Sluch.Columns[10] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            Sluch.Columns[1].Visibility = Visibility.Hidden;
            Sluch.Columns[3].Visibility = Visibility.Hidden;
            Sluch.Columns[5].Visibility = Visibility.Hidden;
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int index = Sluch.SelectedIndex;

            if (index == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            id_sluch = Convert.ToInt32(Connect.ds.Tables["StacSluch"].Rows[index]["Номер"]);
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (Connect.ds.Tables["StacSluch"].Rows[index]["Дата закрытия"].ToString() != "")
                {
                    MessageBox.Show("Случай закрыт. Удаление невозможно!", "Ошибка");
                    return;
                }
                string sql = "delete from stac_sluch where id=" + id_sluch;
                if (!Connect.Modification_Execute(sql)) return;
            }
            NavigationService.Navigate(new StacSluch());
        }
    }
}
