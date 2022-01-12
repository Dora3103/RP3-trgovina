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
        public Worker()
        {
            InitializeComponent();
            label2.Text = Form1.quantity;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            if (Form1.quantity == "admin")
            {
                Register r = new Register();
                r.ShowDialog();
            }
            else
            {
                MessageBox.Show("Samo šef smije dodavati nove radnike!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Form1.quantity == "admin")
            {
                Newproduct p = new Newproduct();
                p.ShowDialog();
            }
            else
            {
                MessageBox.Show("Samo šef smije dodavati nove proizvode!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Form1.quantity == "admin")
            {
                Removeproduct p = new Removeproduct();
                p.ShowDialog();
            }
            else
            {
                MessageBox.Show("Samo šef smije uklanjati proizvode!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            ListOfProducts f = new ListOfProducts();
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddBill p = new AddBill();
            p.ShowDialog();
        }
    }
}
