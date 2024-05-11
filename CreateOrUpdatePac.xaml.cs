using System.Security.Cryptography;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для CreateorUpdatePac.xaml
    /// </summary>
    public partial class CreateorUpdatePac : Window
    {
        public CreateorUpdatePac()
        {
            InitializeComponent();
        }

        public static int id_adr = -1;
        public static int id_doc = -1;

        private void Fam_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Fam.Text == "Фамилия")
                Fam.Text = "";
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "Имя")
                Name.Text = "";
        }

        private void Patr_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Patr.Text == "Отчество")
                Patr.Text = "";
        }

        private void Phone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Phone.Text == "Номер телефона")
                Phone.Text = "";
        }

        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "Электронная почта")
                Email.Text = "";
        }

        public static int getCurrentAdrRowNumber()
        {
            return id_adr;
        }

        public static int getCurrentDocRowNumber()
        {
            return id_doc;
        }

        private void Note_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Note.Text == "Примечание")
                Note.Text = "";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Name.Text = "Имя";
            Fam.Text = "Фамилия";
            Patr.Text = "Отчество";
            Phone.Text = "Номер телефона";
            Email.Text = "Электронная почта";
            Note.Text = "Примечание";

            Pacients.id_pac = -1;
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
            AdrTable.Columns[0].IsReadOnly = true;
            AdrTable.Columns[1].IsReadOnly = true;

            DocTable.ItemsSource = Connect.ds.Tables["UpdDoc"].DefaultView;
            DocTable.AutoGenerateColumns = true;
            DocTable.HeadersVisibility = DataGridHeadersVisibility.None;
            DocTable.CanUserAddRows = false;
            DocTable.Columns[0].Visibility = Visibility.Hidden;
            DocTable.Columns[1].IsReadOnly = true;
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

            Name.Text = Connect.ds.Tables["UpdPac"].Rows[0]["name"].ToString().Replace(" ", "");
            Fam.Text = Connect.ds.Tables["UpdPac"].Rows[0]["fam"].ToString().Replace(" ", "");
            Patr.Text = Connect.ds.Tables["UpdPac"].Rows[0]["patr"].ToString().Replace(" ", "");
            Phone.Text = Connect.ds.Tables["UpdPac"].Rows[0]["phone_number"].ToString().Replace(" ", "");
            Email.Text = Connect.ds.Tables["UpdPac"].Rows[0]["email"].ToString().Replace(" ", "");
            Note.Text = Connect.ds.Tables["UpdPac"].Rows[0]["note"].ToString().Replace(" ", "");
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
            if (Connect.ds.Tables["UpdAdr"].Rows.Count == 2)
            {
                MessageBox.Show("Пациент не может иметь более двух типов адресов.", "Ошибка");
                return;
            }

            PacAdr pacAdr = new PacAdr();
            pacAdr.ShowDialog();
            AdrTable.SelectedIndex = -1;
            Table_Fill();
        }

        private void ButtonAddDoc_Click(object sender, RoutedEventArgs e)
        {
            if (Connect.ds.Tables["UpdDoc"].Rows.Count == 3)
            {
                MessageBox.Show("Пациент не может иметь более трех типов документов.", "Ошибка");
                return;
            }

            PacDoc pacDoc = new PacDoc();
            pacDoc.ShowDialog();
            DocTable.SelectedIndex = -1;
            Table_Fill();
        }
    }
}
