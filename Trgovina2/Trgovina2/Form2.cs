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
    public partial class Worker : Form
    {

        int expiration_days = 0;
        int min_amount = 0;
        public Worker()
        {
            this.MinimumSize = new System.Drawing.Size(480, 360);
            InitializeComponent();
            label2.Text = Form1.name;  // upisujemo username korisnika
            CheckForNotif(); // osvjezi obavijesti
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            if (Form1.name == "admin")  // samo šef smije dodavati nove radnike u bazu - username je jedinstven za svakog radnika (samo šef ima username admin)
            {
                Register r = new Register();  // otvori formu u kojoj će to moći napraviti
                r.ShowDialog();
            }
            else
            {
                MessageBox.Show("Samo šef smije dodavati nove radnike!"); // javi upozorenje ako netko od radnika to pokuša 
            }
            CheckForNotif(); // osvjezi obavijesti
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Form1.name == "admin")  // samo šef smije dodavati nove proizvode
            {
                Newproduct p = new Newproduct(); // otvori formu u kojoj će to moći napraviti
                p.ShowDialog();
            }
            else
            {
                MessageBox.Show("Samo šef smije dodavati nove proizvode!"); // javi upozorenje ako netko od radnika to pokuša
            }
            CheckForNotif(); // osvjezi obavijesti
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Form1.name == "admin") // samo šef smije micati proizvode iz baze
            {
                Removeproduct p = new Removeproduct(); // otvori formu u kojoj će to moći napraviti
                p.ShowDialog();
            }
            else
            {
                MessageBox.Show("Samo šef smije uklanjati proizvode!"); // javi upozorenje ako netko od radnika to pokuša
            }
            CheckForNotif(); // osvjezi obavijesti
        }

        private void button4_Click(object sender, EventArgs e) // odlogiravanje radnika
        {
            this.Close();
            Form1 f = new Form1();
            f.Show(); // ponovno otvori login formu kako bi se drugi radnik mogao ulogirati
            CheckForNotif(); // osvjezi obavijesti
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //this.Hide();
            ListOfProducts f = new ListOfProducts();
            f.Show();
            CheckForNotif(); // osvjezi obavijesti
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddBill p = new AddBill();
            p.ShowDialog();
            CheckForNotif(); // osvjezi obavijesti
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowNotifications n = new ShowNotifications(expiration_days, min_amount);
            n.ShowDialog();
            // Pamtimo zadnje konfiguracijske vrijednosti za prikaz obavijesti, tj. broj dana
            // unutar kojih provjeravamo istek roka trajanja te minimalan broj proizvoda 
            // koje zelimo imati na zalihi. 
            expiration_days = n.GetNumDaysToCheck();
            min_amount = n.GetLowQuantityThreshold();
            CheckForNotif(); // osvjezi obavijesti
        }

        private void CheckForNotif() {
            bool notification = false;

            DateTime cutoff_date = DateTime.Now.AddDays(expiration_days);

            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                // provjeravamo postoje li u bazi proizvodi za koje bi trebalo prikazati notifikaciju
                OleDbDataAdapter sda = new OleDbDataAdapter("select count(*) from proizvodi where Rok_trajanja <= " 
                    + cutoff_date.ToOADate() + "or Kolicina <= " + min_amount.ToString(), con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() != "0")
                {
                    notification = true;
                }
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Ako postoji obavijest, prikazujemo znak lampice. 
            // Ako ne postoji obavijest, skrivamo taj znak. 
            if (notification)
            {
                notif.Text = "💡";
                notif.Show();
            }
            else {
                notif.Hide();
            }
        }
    }
}
