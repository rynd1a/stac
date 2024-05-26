using System;
using System.Windows;

namespace stac
{
    public partial class PacAdr : Window
    {
        public PacAdr()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (CreateorUpdatePac.getCurrentAdrRowNumber() == -1) return;

            Connect.Table_Fill("UpdAdr", "select id, type, adr from address where id= " + CreateorUpdatePac.getCurrentAdrRowNumber());
            Adr.Text = Connect.ds.Tables["UpdAdr"].Rows[0]["adr"].ToString();

            for (int i = 0; i < Type.Items.Count; i++)
            {
                if (Type.Items[i].ToString() != Connect.ds.Tables["UpdAdr"].DefaultView[0]["type"].ToString())
                {
                    continue;
                }
                Type.SelectedIndex = i;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string  result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (Adr.Text == "")
                {
                    MessageBox.Show("Адрес является обязательным для заполнения", "Внимание");
                    return;
                }

                if (Type.Text == "")
                {
                    MessageBox.Show("Тип адреса является обязательным для заполнения", "Внимание");
                    return;
                }

                if (CreateorUpdatePac.getCurrentAdrRowNumber() != -1)
                {
                    sql = "update address set type='" + Type.Text + "', adr='" +
                        Adr.Text + "' where id=" + CreateorUpdatePac.getCurrentAdrRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    Connect.Table_Fill("TypeAdr", "select id from address where type='" + Type.Text + "' and patient_id = " + Pacients.getCurrentRowNumber());

                    if (Connect.ds.Tables["TypeAdr"].Rows.Count > 0)
                    {
                        MessageBox.Show("Пациент не может иметь два адреса одного типа.", "Ошибка");
                        return;
                    }

                    sql = "insert into address(patient_id, type, adr) values(" + Pacients.getCurrentRowNumber() +
                        ", '" + Type.Text + "', '" + Adr.Text + "')";
                    if (!Connect.Modification_Execute(sql)) return;
                    
                }
            }
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            CreateorUpdatePac.id_adr = -1;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Adr.Text = "";
            Type.SelectedIndex = -1;

            CreateorUpdatePac.id_adr = -1;
        }
    }
}
