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
        public AddBill()
        {
            InitializeComponent();
            fetchData();
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
                            cijena = (int)dt.Rows[0]["Cijena"]
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

            makeBill();
        }

        private void makeBill()
        {
            BillFinalControl control = new BillFinalControl();
            string[] row = new string[4];

            foreach (BillControl item in flowLayoutPanel1.Controls)
            {
                row[0] = item.naziv;
                row[1] = item.kolicina;
                row[2] = item.cijena.ToString();

                double afterDiscount = item.cijena;

                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

                try
                {
                    con.Open();

                    OleDbDataAdapter sda = new OleDbDataAdapter("" +
                        "SELECT postotakPopusta " +
                        "FROM popust INNER JOIN proizvodi ON popust.proizvodId = proizvodi.id " +
                        "WHERE proizvodi.Kod='" + item.kod + "' " +
                        "AND popust.datumOd<=CURRENT_DATE " +
                        "AND popust.datumDo>=CURRENT_DATE", con);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        afterDiscount = (double)dt.Rows[0]["postotakPopusta"];
                    }

                    row[3] = afterDiscount.ToString();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }

                row[3] = afterDiscount.ToString("0.##");
                control.populateTable(row);
            }
        }
    }
}
