using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            CheckForNotif();
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
            CheckForNotif();
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
            CheckForNotif();
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
            CheckForNotif();
        }

        private void button4_Click(object sender, EventArgs e) // odlogiravanje radnika
        {
            this.Close();
            Form1 f = new Form1();
            f.Show(); // ponovno otvori login formu kako bi se drugi radnik mogao ulogirati
            CheckForNotif();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //this.Hide();
            ListOfProducts f = new ListOfProducts();
            f.Show();
            CheckForNotif();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddBill p = new AddBill();
            p.ShowDialog();
            CheckForNotif();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowNotifications n = new ShowNotifications(expiration_days, min_amount);
            n.ShowDialog();

            expiration_days = n.GetBrojDana();
            min_amount = n.GetKolicina();
            CheckForNotif();
        }

        private void CheckForNotif() {
            bool notification = false;

            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.allProducts();

            DateTime granica = DateTime.Now.AddDays(expiration_days);
            foreach (proizvod p in proizvodi)
            {
                if (DateTime.Compare(p.exp, granica) < 0)
                {
                    notification = true;
                }
                if (p.quant <= min_amount) {
                    notification = true;
                }
            }

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
