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
        int broj_dana_za_provjeru = 0;
        int kolicina_proizvoda_za_provjeru = 0;
        public ShowNotifications(int d, int k)
        {
            broj_dana_za_provjeru = d;
            kolicina_proizvoda_za_provjeru = k;

            InitializeComponent();
            textBox1.Text = broj_dana_za_provjeru.ToString();
            textBox2.Text = kolicina_proizvoda_za_provjeru.ToString();
            
            ShowInitialMessage();
        }
        public int GetBrojDana() {
            return broj_dana_za_provjeru;
        }

        public int GetKolicina() {
            return kolicina_proizvoda_za_provjeru;
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

        private List<proizvod> GetLowInStockProducts(int quant)
        {
            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.allProducts();

            List<proizvod> low_in_stock = new List<proizvod>();

            foreach (proizvod p in proizvodi)
            {
                if (p.quant <= quant)
                {
                    low_in_stock.Add(p);
                }
            }
            return low_in_stock;
        }

        private void ShowInitialMessage()
        {
            List<proizvod> istekao_rok = GetExpiredProducts(broj_dana_za_provjeru);
            List<proizvod> niske_zalihe = GetLowInStockProducts(kolicina_proizvoda_za_provjeru);

            int broj_obavijesti = 0;
            if (istekao_rok.Count > 0) broj_obavijesti++;
            if (niske_zalihe.Count > 0) broj_obavijesti++;

            label1.Text = "Broj obavijesti: " + broj_obavijesti;

            if (istekao_rok.Count == 0)
            {
                label2.Text = "Trenutno nema proizvoda koji su blizu isteka roka!";
            }
            else {
                label2.Text = "Obavijest: Postoje proizvodi kojima su blizu isteka roka";
            }

            if(niske_zalihe.Count == 0)
            {
                label3.Text = "Trenutno nema proizvoda s niskim zalihama!";
            }
            else
            {
                label3.Text = "Obavijest: Postoje proizvodi s niskim zalihama!";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExpiredProducts exp_p = new ExpiredProducts();
            exp_p.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LowStockProducts l = new LowStockProducts();
            l.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            broj_dana_za_provjeru = int.Parse(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            kolicina_proizvoda_za_provjeru = int.Parse(textBox2.Text);
        }
    }
}
