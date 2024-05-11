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
    /// Логика взаимодействия для Pacients.xaml
    /// </summary>
    public partial class Pacients : Page
    {
        public Pacients()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateorUpdatePac createorUpdatePac = new CreateorUpdatePac();
            createorUpdatePac.ShowDialog();
            return;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Pac", "select id as Номер, fam || ' ' || name || ' ' || patr as ФИО, birth_date as \"Дата рождения\"," +
                " (case when gender = 0 then 'Женский' when gender = 1 then 'Мужской' else 'Неизвестно' end) as Пол, phone_number" +
                " as Телефон, email as \"Электронная почта\", note as Примечание " +
                " from patient order by id");
            PacTable.ItemsSource = Connect.ds.Tables["Pac"].DefaultView;
            PacTable.AutoGenerateColumns = true;
            (PacTable.Columns[2] as DataGridTextColumn).Binding.StringFormat = "dd.mm.yyyy";
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

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int id = PacTable.SelectedIndex;
            if (id == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }

            string result;
            string sql = "";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                sql = "delete from patient where id = " + id;
                if (!Connect.Modification_Execute(sql))
                    return;
                Connect.ds.Tables["Pac"].Rows.RemoveAt(id);
            }
        }

    }
}
