using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp4
{
    public partial class SeansEkle : Form
    {
        string path = "SstDB.db";
        string cs = @"URI=file:" + Application.StartupPath + "\\SstDB.db"; //database creat debug folder

        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public SeansEkle()
        {
            InitializeComponent();
            SQLiteConnection bag = new SQLiteConnection("Data source=C:\\Users\\kutla\\OneDrive\\Masaüstü\\VsApp\\SinemaSeansTakipApp\\SstDB;Version=3;");
            SQLiteConnection yeni = new SQLiteConnection(bag);
            yeni.Open(); // Bağlantıyı Açtık

            if (yeni.State == ConnectionState.Open)
            {
                MessageBox.Show("Veritabanına Bağlanıldı"); // Bağlantı Açılırsa Uyarı Versin
            }

            else
            {
                MessageBox.Show("Bağlantı Başarısız"); // Başarısız ise uyarı Versin
            }
            yeni.Close(); //Bağlantıyı Sonlandırdık
        }

        private void data_show()
        {
            var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM test";
            var cmd = new SQLiteCommand(stm, con);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dataGridView1.Rows.Insert(0, dr.GetString(0), dr.GetString(1), dr.GetString(2));
            }
        }
        private void Create_db()
        {
            if (!System.IO.File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + path))
                {
                    sqlite.Open();
                    string sql = "create table test(name varchar(20),id varchar(12))";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                Console.WriteLine("Database cannot create");
                return;
            }
        }

        private void SeansEkle_Load(object sender, EventArgs e)
        {
            Create_db();
            data_show();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);

            try
            {
                cmd.CommandText = "INSERT INTO FilmEkle (SalonId,SalonAd,Kapasite) VALUES(@SalonId,@SalonAd,@Kapasite)";

                string Ad = txtSeansAd.Text;
                string Saat = txtSaat.Text;
                string Sayi = txtSayi.Text;

                cmd.Parameters.AddWithValue("@SeansAd", Ad);
                cmd.Parameters.AddWithValue("@Saat", Saat);
                cmd.Parameters.AddWithValue("@Sayi", Sayi );


                dataGridView1.ColumnCount = 3;
                dataGridView1.Columns[0].Name = "Ad";
                dataGridView1.Columns[1].Name = "Saat";
                dataGridView1.Columns[2].Name = "Sayi";
                string[] row = new string[] { Ad, Saat, Sayi };
                dataGridView1.Rows.Add(row);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                Console.WriteLine("veri kaydedilmedi");
                return;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();

            var cmd = new SQLiteCommand(con);

            try
            {
                cmd.CommandText = "DELETE FROM test where name =@SalonAd";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@SeansAd", txtSeansAd.Text);

                cmd.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
                data_show();
            }
            catch (Exception)
            {
                Console.WriteLine("veri silinmedi");
                return;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            txtSeansAd.Text = dataGridView1.Rows[e.RowIndex].Cells["SeansAd"].FormattedValue.ToString();
            txtSaat.Text = dataGridView1.Rows[e.RowIndex].Cells["Saat"].FormattedValue.ToString();
        }
    }
}
