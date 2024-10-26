using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SQLite;
using System.Runtime.Remoting.Contexts;

namespace Zapsinaya_knizka_new
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static SQLiteConnection podkluchit;
        static SQLiteCommand komanda;

        List<Stranica> stranici = new List<Stranica>();

        
        
        int nomer_stranici;
        int data;
        private void Form1_Load(object sender, EventArgs e)
        {
            button_nazad.Hide();
            dataGridView1.Hide();
            this.chart1.Hide();
            button_main.Hide();
            label4.Hide();
            comboBox1.Hide();
            nomer_stranici = 0;
            SQLiteConnection connection = new SQLiteConnection("Integrated Security = SSPI; Data Source = stranica.db");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT  Nomer, Text  FROM Stranica";
            using (var rd1 = command.ExecuteReader())
            {

                while (rd1.Read())
                {
                    stranici.Add(new Stranica(rd1.GetInt32(0), rd1.GetString(1)));


                }
            }
            connection.Close();

            label3.Text = ("Страниц всего:" + " " + stranici.Count.ToString());
            textBox1.Text = stranici[nomer_stranici].Text_stranici;
            label2.Text = $"Номер страницы: {nomer_stranici+1}";

            if (nomer_stranici == 0 && stranici.Count == 1)
            {
                obnovlenie();
                podkluchit = new SQLiteConnection("Data Source=" + "stranica.db" + ";Version=3; FailIfMissing=False");
                podkluchit.Open();
                komanda = new SQLiteCommand(podkluchit);
                komanda.CommandText = $"INSERT INTO Stranica (Nomer, Text) VALUES ({stranici.Count + 1} , \"Привет я новая страница {stranici.Count + 1}\")";
                komanda.ExecuteNonQuery();
                label3.Text = "Страниц всего:" + " " + (stranici.Count + 1).ToString();
            }

        }
        public void obnovlenie()
        {
            stranici.Clear();
            SQLiteConnection connection = new SQLiteConnection("Integrated Security = SSPI; Data Source = stranica.db");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT  Nomer, Text  FROM Stranica";
            using (var rd1 = command.ExecuteReader())
            {

                while (rd1.Read())
                {
                    stranici.Add(new Stranica(rd1.GetInt32(0), rd1.GetString(1)));


                }
            }
            connection.Close();
        }

        public void udalenie(string text)
        {
            obnovlenie();
            podkluchit = new SQLiteConnection("Data Source=" + "stranica.db" + ";Version=3; FailIfMissing=False");
            podkluchit.Open();
            komanda = new SQLiteCommand(podkluchit);
            komanda.CommandText = $"DELETE FROM Stranica WHERE Text = \"{text}\"";
            komanda.ExecuteNonQuery();
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            obnovlenie();
            stranici[nomer_stranici].Text_stranici = textBox1.Text;
            string temp = stranici[nomer_stranici].Text_stranici;
            podkluchit = new SQLiteConnection("Data Source=" + "stranica.db" + ";Version=3; FailIfMissing=False");
            podkluchit.Open();
            komanda = new SQLiteCommand(podkluchit);
            komanda.CommandText = $"UPDATE Stranica SET Text=\"{temp}\" WHERE Nomer={nomer_stranici + 1}";
            komanda.ExecuteNonQuery();
        }

        private void button_vpered_Click(object sender, EventArgs e)
        {
            
            obnovlenie();
            button_nazad.Show();
            nomer_stranici++;
            textBox1.Text = stranici[nomer_stranici].Text_stranici;
            if (nomer_stranici == stranici.Count-1)
            {
                button_vpered.Hide();
            }
            label2.Text = $"Номер страницы: {nomer_stranici +1}";
        }

        private void button_nazad_Click(object sender, EventArgs e)
        {
            obnovlenie();
            nomer_stranici--;
            textBox1.Text = stranici[nomer_stranici].Text_stranici;
            if (nomer_stranici < 1)
            {
                button_nazad.Hide();
            }
            label2.Text = $"Номер страницы: {nomer_stranici + 1}";
            if (nomer_stranici < stranici.Count() - 1)
            {
                button_vpered.Show();
            }
            label2.Text = $"Номер страницы: {nomer_stranici + 1}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            obnovlenie();
            podkluchit = new SQLiteConnection("Data Source=" + "stranica.db" + ";Version=3; FailIfMissing=False");
            podkluchit.Open();
            komanda = new SQLiteCommand(podkluchit);
            komanda.CommandText = $"INSERT INTO Stranica (Nomer, Text) VALUES ({stranici.Count+1} , \"Привет я новая страница {stranici.Count+1}\")";
            komanda.ExecuteNonQuery();
            label3.Text = "Страниц всего:" + " " +(stranici.Count+1 ).ToString();
            if (nomer_stranici == stranici.Count() - 1)
            {
                button_vpered.Show();
            }
            

        }

        private void button_data_Click(object sender, EventArgs e)
        {
            label1.Hide();                          
            label2.Hide();
            label3.Hide();
            button1.Hide();
            button_nazad.Hide();
            button_vpered.Hide();
            textBox1.Hide();
            button_data.Hide();
            button2.Hide();

            this.dataGridView1.Show();              
            this.chart1.Show();
            button_main.Show();
            label4.Show();
            comboBox1.Show();

            comboBox1.Items.Clear();
            comboBox1.Items.Add(" Объем текста меньше 100 ");
            comboBox1.Items.Add("Объем текста больше 100");
            comboBox1.Items.Add("По Объему от меньшего к большему");
            comboBox1.Items.Add("По Объему от большего к меньшему");



            

            obnovlenie();

            for(int i = 0; i < stranici.Count; i++)
            {
                data = stranici[i].Text_stranici.ToArray().Length;
                this.chart1.Series[0].Points.AddXY(stranici[i].Nomer_stranici, data);
                
            }
            this.dataGridView1.DataSource= stranici;
        }

        private void button_main_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Hide();              
            this.chart1.Hide();
            button_main.Hide();
            label4.Hide();
            comboBox1.Hide();
            button_nazad.Hide();

            label1.Show();                        
            label2.Show();
            label3.Show();
            button1.Show();
            button_nazad.Show();
            button_vpered.Show();
            textBox1.Show();
            button_data.Show();
            button2.Show();

            obnovlenie();
            nomer_stranici = 0;
            label3.Text = ("Страниц всего:" + " " + stranici.Count.ToString());
            textBox1.Text = stranici[nomer_stranici].Text_stranici;
            label2.Text = $"Номер страницы: {nomer_stranici + 1}";


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                IEnumerable<Stranica> proverka = stranici.Where(n => n.Text_stranici.ToArray().Length < 100);
                this.chart1.Series[0].Points.Clear();
                for (int i = 0; i <proverka.ToList().Count; i++)
                {
                    data = proverka.ToList()[i].Text_stranici.ToArray().Length;
                    this.chart1.Series[0].Points.AddXY(proverka.ToList()[i].Nomer_stranici, data);
                }
                
                this.dataGridView1.DataSource = proverka.ToArray();
            }
            if (comboBox1.SelectedIndex == 1)
            {
                IEnumerable<Stranica> proverka = stranici.Where(n => n.Text_stranici.ToArray().Length > 100);
                this.chart1.Series[0].Points.Clear();
                for (int i = 0; i < proverka.ToList().Count; i++)
                {
                    data = proverka.ToList()[i].Text_stranici.ToArray().Length;
                    this.chart1.Series[0].Points.AddXY(proverka.ToList()[i].Nomer_stranici, data);

                }
                this.dataGridView1.DataSource = proverka.ToArray();
            }
            if (comboBox1.SelectedIndex == 2)
            {
                IEnumerable<Stranica> proverka = stranici.OrderBy(n => n.Text_stranici.ToArray().Length);
                this.chart1.Series[0].Points.Clear();
                for (int i = 0; i < proverka.ToList().Count; i++)
                {
                    data = proverka.ToList()[i].Text_stranici.ToArray().Length;
                    this.chart1.Series[0].Points.AddXY(proverka.ToList()[i].Nomer_stranici, data);

                }
                this.dataGridView1.DataSource = proverka.ToArray();
            }
            if (comboBox1.SelectedIndex == 3)
            {
                IEnumerable<Stranica> proverka = stranici.OrderBy(n => n.Text_stranici.ToArray().Length).Reverse();
                this.chart1.Series[0].Points.Clear();
                for (int i = 0; i < proverka.ToList().Count; i++)
                {
                    data = proverka.ToList()[i].Text_stranici.ToArray().Length;
                    this.chart1.Series[0].Points.AddXY(proverka.ToList()[i].Nomer_stranici, data);

                }
                this.dataGridView1.DataSource = proverka.ToArray();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            obnovlenie();
            string text = stranici[stranici.Count-1].Text_stranici.ToString();
            udalenie(text); 
            obnovlenie(); 
            label3.Text = ("Страниц всего:" + " " + stranici.Count.ToString());
            obnovlenie();
            nomer_stranici = 0;
            label3.Text = ("Страниц всего:" + " " + stranici.Count.ToString());
            textBox1.Text = stranici[nomer_stranici].Text_stranici;
            label2.Text = $"Номер страницы: {nomer_stranici + 1}";
            button_nazad.Hide();
            button_vpered.Show();
        }
    }
}
