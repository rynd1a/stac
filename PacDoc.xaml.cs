using System;
using System.Windows;

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для PacDoc.xaml
    /// </summary>
    public partial class PacDoc : Window
    {
        public PacDoc()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (CreateorUpdatePac.getCurrentDocRowNumber() == -1) return;

            Connect.Table_Fill("UpdDoc", "select id, type, ser, num, issued, date_issued from document where id= " + CreateorUpdatePac.getCurrentDocRowNumber());
            Ser.Text = Connect.ds.Tables["UpdDoc"].Rows[0]["ser"].ToString();
            Num.Text = Connect.ds.Tables["UpdDoc"].Rows[0]["num"].ToString();
            Issued.Text = Connect.ds.Tables["UpdDoc"].Rows[0]["issued"].ToString();
            Dat.Text = Convert.ToDateTime(Connect.ds.Tables["UpdDoc"].Rows[0]["date_issued"]).ToString();

            for (int i = 0; i < Type.Items.Count; i++)
            {
                if (Type.Items[i].ToString() == Connect.ds.Tables["UpdDoc"].DefaultView[0]["type"].ToString())
                {
                    continue;
                }
                Type.SelectedIndex = i;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Ser.Text = "Серия";
            Num.Text = "Номер";
            Issued.Text = "Кем выдан";
            Dat.Text = "";
            Type.SelectedIndex = -1;

            CreateorUpdatePac.id_doc = -1;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                try { 
                    Convert.ToInt32(Num.Text); 
                } catch (FormatException) {
                    MessageBox.Show("Неверный формат номера документа", "Ошибка");
                    return;
                }

                if (CreateorUpdatePac.getCurrentDocRowNumber() != -1)
                {
                    sql = "update document set type='" + Type.Text + "', ser='" +
                        Ser.Text + "', num=" + Num.Text + ", issued='" + Issued.Text +
                        "', date_issued='" +  Dat.Text + "' where id=" + CreateorUpdatePac.getCurrentDocRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    Connect.Table_Fill("TypeDoc", "select id from document where type='" + Type.Text + "' and patient_id = " + Pacients.getCurrentRowNumber());

                    if (Connect.ds.Tables["TypeDoc"].Rows.Count > 0)
                    {
                        MessageBox.Show("Пациент не может иметь два документа одного типа.", "Ошибка");
                        return;
                    }

                    sql = "insert into document(patient_id, type, ser, num, issued, date_issued) values(" + Pacients.getCurrentRowNumber() +
                        ", '" + Type.Text + "', '" + Ser.Text + "', " + Num.Text + ", '" + Issued.Text + 
                        "', '" + Dat.Text + "')";
                    if (!Connect.Modification_Execute(sql)) return;

                }
            }
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            CreateorUpdatePac.id_doc = -1;
            this.Close();
        }

        private void Ser_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Ser.Text == "Серия")
                Ser.Text = "";
        }

        private void Num_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Num.Text == "Номер")
                Num.Text = "";
        }

        private void Issued_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Issued.Text == "Кем выдан")
                Issued.Text = "";
        }
    }
}
