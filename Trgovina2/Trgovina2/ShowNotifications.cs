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
        // Pamtimo broj dana i kolicinu proizvoda za koje treba pokazati obavijest. 
        int num_days_to_check = 0;
        int low_quantity_threshold = 0;
        public ShowNotifications(int d, int k)
        {
            num_days_to_check = d;
            low_quantity_threshold = k;

            InitializeComponent();

            // Postavljamo vrijednosti broja dana i minimalnog broja proizvoda na zalihi, 
            // kako bi se ispravno prikazivale poruke vezane za postavke obavijesti. 
            textBox1.Text = num_days_to_check.ToString();
            textBox2.Text = low_quantity_threshold.ToString();
            
            // Prikazujemo pocetni sadrzaj obavijesti. 
            ShowInitialMessage();
        }

        public int GetNumDaysToCheck() {
            return num_days_to_check;
        }

        public int GetLowQuantityThreshold() {
            return low_quantity_threshold;
        }

        private List<proizvod> GetExpiredProducts(int days, List<proizvod> products) {
            List<proizvod> expired_products = new List<proizvod>();
            // Spremamo one proizvode kojima rok trajanja istice u zadanom broju dana.
            DateTime cutoff_date = DateTime.Now.AddDays(days);
            foreach (proizvod p in products) {
                if (DateTime.Compare(p.exp, cutoff_date) <= 0) {
                    expired_products.Add(p);
                }
            }
            return expired_products;
        }

        private List<proizvod> GetLowInStockProducts(int quant, List<proizvod> products)
        {
            List<proizvod> low_in_stock = new List<proizvod>();
            // Dohvacamo one proizvode kojima je trenutna kolicina manja od zadane vrijednosti quant. 
            foreach (proizvod p in products)
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
            dataBase db = new dataBase();
            List<proizvod> all_products = db.allProducts();

            // Dohvacamo proizvode kojima je prosao rok trajanja i kojih imamo malo na zalihi.  
            List<proizvod> expired_products = GetExpiredProducts(num_days_to_check, all_products);
            List<proizvod> low_in_stock = GetLowInStockProducts(low_quantity_threshold, all_products);

            // Prikazat cemo broj obavijesti u pocetnoj poruci. 
            int broj_obavijesti = 0;
            if (expired_products.Count > 0) broj_obavijesti++;
            if (low_in_stock.Count > 0) broj_obavijesti++;

            // Slijedi postavljanje poruka za obavijesti 

            label1.Text = "Broj obavijesti: " + broj_obavijesti;

            if (expired_products.Count == 0)
            {
                label2.Text = "Trenutno nema proizvoda koji su blizu isteka roka!";
            }
            else {
                label2.Text = "Obavijest: Postoje proizvodi kojima su blizu isteka roka";
            }

            if(low_in_stock.Count == 0)
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
            ExpiredProducts exp_p = new ExpiredProducts(num_days_to_check);
            exp_p.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LowStockProducts l = new LowStockProducts(low_quantity_threshold);
            l.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            num_days_to_check = int.Parse(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            low_quantity_threshold = int.Parse(textBox2.Text);
        }
    }
}
