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
   
    public partial class Form1 : Form
    {

        public static string quantity;
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
        public Form1()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
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

            else if (!textBox2.Text.Any(char.IsDigit))
            {
                MessageBox.Show("lozinka mora sadržavati samo brojeve!");
                return;
            }

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("select ID, Username, Password from login", con);
                
                OleDbDataAdapter sda = new OleDbDataAdapter("select count(*) from login where Username='" + textBox1.Text + "' and Password=" + textBox2.Text, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {

                    quantity = textBox1.Text;
                    con.Close();
                    this.Hide();
                    Worker m = new Worker();
                    m.Show();

                }

                else
                {
                    MessageBox.Show("Unesite ispravne podatke!");
                    con.Close();
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
