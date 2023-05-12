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

namespace WindowsFormsApp4
{
    public partial class FilmEkle : Form
    {
        //path of data base
        string path = "SstDB.db";
        string cs = @"URI=file:" + Application.StartupPath + "\\SstDB.db";

        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public FilmEkle()
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
                Console.WriteLine("veriseti oluşturulamadı");
                return;
            }
        }

        private void FilmEkle_Load(object sender, EventArgs e)
        {
            Create_db();
            data_show();
        }

        private void btnFilmEkle_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);

            try
            {
                cmd.CommandText = "INSERT INTO FilmEkle (FilmId,FilmAd,FilmTarihi,SeansSaat) VALUES(@FilmId,@FilmAd,@FilmTarih,@SeansSaat)";

                string Id = txtId.Text;
                string Ad = txtAd.Text;
                string Tarih = dateTimePicker1.Text;
                string SeansSaat = txtSeansSaat.Text;


                cmd.Parameters.AddWithValue("@FilmId", Id);
                cmd.Parameters.AddWithValue("@FilmAd", Ad);
                cmd.Parameters.AddWithValue("@FilmTarih", Tarih);
                cmd.Parameters.AddWithValue("@SeansSaat", SeansSaat);

                dataGridView1.ColumnCount = 4;
                dataGridView1.Columns[0].Name = "Id";
                dataGridView1.Columns[1].Name = "Ad";
                dataGridView1.Columns[2].Name = "Tarih";
                dataGridView1.Columns[3].Name = "SeansSaat";
                string[] row = new string[] { Id, Ad, Tarih, SeansSaat };
                dataGridView1.Rows.Add(row);

                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                Console.WriteLine("veri eklenmedi");
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
                cmd.CommandText = "DELETE FROM test where name =@FilmAd";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@FilmAd", txtAd.Text);

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
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dataGridView1.CurrentRow.Selected = true;
                txtAd.Text = dataGridView1.Rows[e.RowIndex].Cells["FilmAd"].FormattedValue.ToString();
                txtSeansSaat.Text = dataGridView1.Rows[e.RowIndex].Cells["SeansSaat"].FormattedValue.ToString();
            }
        }
    }
}
