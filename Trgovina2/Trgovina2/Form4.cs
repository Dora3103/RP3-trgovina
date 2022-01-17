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
    public partial class Register : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
        public Register()
        {
            this.MinimumSize = new System.Drawing.Size(440, 220);
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Unesite username!");
                return;
            }

            else if (textBox2.Text == "")
            {

                MessageBox.Show("Unesite lozinku!");
                return;
            }

            else if(textBox2.Text.Length < 4)
            {
                MessageBox.Show("Lozinka mora biti duljine barem 4!");
                return;
            }

            else if (!textBox2.Text.Any(char.IsDigit))
            {
                MessageBox.Show("lozinka mora sadržavati samo brojeve!");
                return;
            }

            try
            {
                con.Open();

                OleDbDataAdapter sda = new OleDbDataAdapter("select count(*) from login where Username='" + textBox1.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Taj username već postoji!");
                    return;
                }

                OleDbDataAdapter sd = new OleDbDataAdapter("select count(*) from login where Password=" + textBox2.Text, con);
                DataTable dta = new DataTable();
                sd.Fill(dta);
                if (dta.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Ta lozinka je zauzeta!");
                    return;

                }

                else
                {
                    OleDbCommand comm = new OleDbCommand();
                    comm.Connection = con;
                    comm.CommandText = "insert into login ([Username], [Password]) values('" + textBox1.Text + "'," + textBox2.Text + ")";
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Uspješno dodano!");
                    con.Close();
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
    }
}
