using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace AccessDegisiklikAlgilama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public OleDbConnection con;
        DateTime SonErisim;

        private void Form1_Load(object sender, EventArgs e)
        {
            SonErisim = DateTime.Now;
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=Kisiler.accdb");
            KisileriGetir();
            timer1.Start();
        }

        private void KisileriGetir()
        {
            listView1.Items.Clear();
            con.Open();
            OleDbCommand komut = new OleDbCommand("SELECT * FROM Kisiler", con);
            OleDbDataReader okuyucu = komut.ExecuteReader();
            while (okuyucu.Read())
            {
                string id = okuyucu["ID"].ToString();
                string adi = okuyucu["Kisi_Adi"].ToString();
                string soyadi = okuyucu["Kisi_Soyadi"].ToString();
                string telefon = okuyucu["Kisi_Telefon"].ToString();
                int count = listView1.Items.Count;
                listView1.Items.Add(id);
                listView1.Items[count].SubItems.Add(adi);
                listView1.Items[count].SubItems.Add(soyadi);
                listView1.Items[count].SubItems.Add(telefon);
            }
            con.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            con.Open();
            string adi = textBox1.Text.Trim();
            string soyadi = textBox2.Text.Trim();
            string telefon = textBox3.Text.Trim();
            OleDbCommand komut = new OleDbCommand("INSERT INTO Kisiler (Kisi_Adi, Kisi_Soyadi, Kisi_Telefon) VALUES(@adi,@soyadi,@telefon)", con);
            komut.Parameters.AddWithValue("@adi", adi);
            komut.Parameters.AddWithValue("@soyadi", soyadi);
            komut.Parameters.AddWithValue("@telefon", telefon);
            komut.ExecuteNonQuery();
            con.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            FileInfo info = new FileInfo("Kisiler.accdb");
            DateTime erisim = info.LastWriteTime;
            int compare = DateTime.Compare(SonErisim, erisim);
            if (compare < 0)
            {
                KisileriGetir();
                SonErisim = erisim;
            }
        }
    }
}
