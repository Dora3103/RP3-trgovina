using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trgovina2
{
    public partial class AddBill : Form
    {
        double pdv = 25;
        public AddBill()
        {
            InitializeComponent();
            fetchData();
            comboBox1.SelectedIndex = 0;
            textBox4.Text = "25";
        }

        public void fetchData()
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("" +
                    "SELECT Naziv, Kategorija, Kolicina, Kod, Cijena, Rok_trajanja, Datum_nabave " +
                    "FROM proizvodi WHERE Naziv LIKE '%" + textBoxName.Text + "%' " +
                    "AND Kod LIKE '%" + textBoxCode.Text + "%'", con);

                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fetchData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Molimo upišite kod i količinu proizvoda kojeg želite dodati na račun.");
            }
            else
            {
                foreach (BillControl control in flowLayoutPanel1.Controls)
                {
                    if (control.kod == textBox1.Text)
                    {
                        MessageBox.Show("Ovaj proizvod je već dodan u račun. Obrišite ga prvo pa ga dodajte ponovno.");
                        return;
                    }
                }

                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

                try
                {
                    con.Open();
                    OleDbDataAdapter sda = new OleDbDataAdapter("" +
                        "SELECT * " +
                        "FROM proizvodi WHERE Kod='" + textBox1.Text + "' " +
                        "AND Kolicina>=" + textBox2.Text, con);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        var kontrola = new BillControl()
                        {
                            kod = textBox1.Text,
                            kolicina = textBox2.Text,
                            naziv = dt.Rows[0]["Naziv"].ToString(),
                            //cijena = Convert.ToDouble(dt.Rows[0]["Cijena"])
                            cijena = (int)dt.Rows[0]["Cijena"],
                            pdv = textBox4.Text
                        };
                        kontrola.Margin = new Padding(5, 5, 5, 5);
                        flowLayoutPanel1.Controls.Add(kontrola);
                    }
                    else
                    {
                        MessageBox.Show("Ne postoji proizvod s tim kodom ili ga nema dovoljno. Molimo pokušajte ponovno.");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (((BillControl)flowLayoutPanel1.Controls[i]).kod == textBox3.Text)
                {
                    flowLayoutPanel1.Controls.RemoveAt(i);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

            try
            {
                con.Open();
                OleDbCommand cmd;
                foreach (BillControl item in flowLayoutPanel1.Controls)
                {
                    cmd = new OleDbCommand("" +
                    "UPDATE proizvodi " +
                    "SET Kolicina=Kolicina - " + item.kolicina + " " +
                    "WHERE Kod='" + item.kod + "'", con);

                    cmd.ExecuteNonQuery();
                }

                cmd = new OleDbCommand("" +
                    "DELETE FROM proizvodi WHERE Kolicina=0", con);

                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

            BillFinalControl control = new BillFinalControl();
            double total = makeBill(control);

            control.populateLabels(total.ToString("0.##"), comboBox1.Text.Equals("") ? "Gotovina" : comboBox1.Text);
            this.Hide();
            Form f = new Form();
            f.BackColor = SystemColors.Window;
            f.Text = "Bill";
            f.Height = 435;
            f.Width = 314;
            f.Controls.Add(control);
            f.ShowDialog();
        }

        private double makeBill(BillFinalControl control)
        {            
            string[] row = new string[5];
            double total = 0;

            foreach (BillControl item in flowLayoutPanel1.Controls)
            {
                row[0] = item.naziv;
                row[1] = item.kolicina;                

                double afterDiscount = item.cijena;

                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

                try
                {
                    con.Open();
                    var dateNow = DateTime.Now.Date;
                    OleDbDataAdapter sda = new OleDbDataAdapter("" +
                         "SELECT * " +
                         "FROM popust INNER JOIN proizvodi ON popust.proizvodId = proizvodi.ID " +
                         "WHERE proizvodi.Kod='" + item.kod + "' " +
                         "AND popust.datumOd<=Date() " +
                         "AND popust.datumDo>=Date()", con);                  

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        double percentage = Convert.ToDouble(dt.Rows[0]["postotakPopusta"]);
                        afterDiscount -= afterDiscount * percentage / 100;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }

                double productTotal = afterDiscount * Convert.ToDouble(item.kolicina) * (Convert.ToDouble(item.pdv) / 100 + 1);
                total+= productTotal;
                row[2] = afterDiscount.ToString("0.##");
                row[3] = pdv.ToString() + "%";
                row[4] = productTotal.ToString("0.##");
                control.populateTable(row);
            }

            return total;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            pdv = double.Parse(textBox4.Text);
        }
    }
}
