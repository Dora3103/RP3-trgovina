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

        public static string name;   // u njoj ćemo pamtiti username osobe koja će se ulogirati
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
        public Form1()
        {
            this.MinimumSize = new System.Drawing.Size(400, 230);
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")   // ako korisnik nije unio username
            {
                MessageBox.Show("Unesite username!");
                return;
            }

            else if (textBox2.Text == "")  // ako korisnik nije unio lozinku
            {
                MessageBox.Show("Unesite lozinku!");
                return;
            }

            else if (!textBox2.Text.Any(char.IsDigit)) // lozinka smije sadržavati samo brojeve
            {
                MessageBox.Show("lozinka mora sadržavati samo brojeve!");
                return;
            }

            try   // povezivanje s bazom koja sadrži login podatke o korisnicima
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("select ID, Username, Password from login", con);
                
                OleDbDataAdapter sda = new OleDbDataAdapter("select count(*) from login where Username='" + textBox1.Text + "' and Password=" + textBox2.Text, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")   // ako vrijedi znači da u bazi postoji točno jedna osoba koja zadovoljava tražene uvjete (naveden username i lozinku)
                {

                    name = textBox1.Text;
                    con.Close();  // obavezno zatvaramo vezu s bazom
                    this.Hide();
                    Worker m = new Worker();  // korisnik se uspješno ulogirao - ulazi na glavnu "stranicu"
                    m.Show();

                }

                else
                {
                    MessageBox.Show("Unesite ispravne podatke!");  // ako postupak nije bio uspješan korisnik je upisao pogrešne podatke - upozori ga
                    con.Close();
                }
            }

            catch(Exception ex)  // ako povezivanje s bazom nije uspjelo - javi error
            {
                MessageBox.Show("Error: " + ex);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();   // ako korisnik odluči izaći iz aplikacije
        }

    }
}
