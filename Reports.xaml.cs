using System.Data;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace stac
{
    public partial class Reports : Page
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void ButtonPrint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.Application app = new Excel.Application();
            app.Visible = true;
            Excel.Workbook book = app.Workbooks.Add();
            Excel.Worksheet lst = book.Worksheets[1];
            lst.Name = "Листок учета коечного фонда";
            int i = 0, j = 0;
            for (i = 0; i < Tabl.Columns.Count; i++)
            {
                lst.Cells[1, i + 1] = Tabl.Columns[i].Header;
            }

            for (i = 0; i < Tabl.Items.Count; i++)
            {
                DataRowView row = (DataRowView)Tabl.Items[i];
                for (j = 0; j < Tabl.Columns.Count; j++)
                {
                    lst.Cells[i + 2, j + 1] = row[j];
                }
            }

            lst.Range[lst.Cells[1, 1], lst.Cells[1, j + 1]].HorizontalAlignment = 3;
            lst.Cells.Columns.EntireColumn.AutoFit();
            lst.Range[lst.Cells[1, 1], lst.Cells[i + 1, j]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            lst.Rows[1].Insert();
            lst.Range[lst.Cells[1, 1], lst.Cells[1, j]].Merge();
            lst.Cells[1, 1] = "Листок учета коечного фонда с " + First.Text + " по " + Second.Text;
            lst.Cells[1, 1].HorizontalAlignment = 3;
        }


        private void DatePicker_CalendarClosed(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Second.Text == "") return;
            if (First.Text == "")
            {
                MessageBox.Show("Выберите дату начала формирования.", "Внимание");
                return;
            }

            Connect.Table_Fill("Report", "with tbl as (select s.id, s.diagnosis, place_id, date_create, date_close " +
                "from stac_sluch s) select d.name as Отделение, hr.gender as Пол, count(bp.id) as Использовано, " +
                "count(tbl.date_close) as Выписано,  count_open(department_id, hr.gender) as Открыто, " +
                "count_recovery(department_id, hr.gender) as \"Выписано - выздоровление\", count_transfer(department_id, hr.gender) as \"Выписано - перевод\", " +
                "count_noRes(department_id, hr.gender) as \"Выписано - без перемен\", count_free(department_id, hr.gender) as Свободно " +
                "from bed_place bp  join bed_place_hospital_room bphr on bp.id=bphr.bed_place_id  join hospital_room hr on bphr.hospital_room_id=hr.id " +
                "join department d on hr.department_id=d.id left join tbl on bphr.id=tbl.place_id " +
                "where tbl.date_create between '" + First.Text + "' and '" + Second.Text + "' " +
                "group by d.name, hr.gender, Открыто, \"Выписано - выздоровление\", \"Выписано - перевод\", \"Выписано - без перемен\", Свободно");

            Tabl.ItemsSource = Connect.ds.Tables["Report"].DefaultView;
            Tabl.AutoGenerateColumns = true;
            Tabl.HeadersVisibility = DataGridHeadersVisibility.Column;
            Tabl.CanUserAddRows = false;
        }
    }
}
