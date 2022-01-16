using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Globalization;


namespace Trgovina2
{
    public partial class ShowNotifications : Form
    {
        public ShowNotifications()
        {
            InitializeComponent();
            ShowInitialMessage();
        }

        private List<proizvod> GetExpiredProducts(int days) {
            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.allProducts();

            List<proizvod> prosao_rok = new List<proizvod>();

            DateTime granica = DateTime.Now.AddDays(days);
            foreach (proizvod p in proizvodi) {
                if (DateTime.Compare(p.exp, granica) < 0) {
                    prosao_rok.Add(p);
                }
            }
            return prosao_rok;
        }

        private void ShowInitialMessage()
        {
            List<proizvod> istekao_rok = GetExpiredProducts(0);
            if (istekao_rok.Count == 0)
            {
                label1.Text = "Trenutno nema obavijesti! Nema proizvoda isteklog roka";
            }
            else {
                label1.Text = "Obavijest: Postoje proizvodi kojima je istekao rok";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExpiredProducts exp_p = new ExpiredProducts();
            exp_p.ShowDialog();
        }
    }
}
