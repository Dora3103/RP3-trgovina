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
    public partial class ExpiredProducts : Form
    {
        public ExpiredProducts()
        {
            this.MinimumSize = new System.Drawing.Size(800, 400);
            InitializeComponent();
            showList(0);
        }


        private List<proizvod> GetExpiredProducts(int days)
        {
            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.allProducts();

            // Ova lista ce sadrzavati sve proizvode kojima je prosao rok.
            List<proizvod> expired_products = new List<proizvod>();

            // Vrijednost koju cemo koristiti za usporedbu. 
            DateTime cutoff_date = DateTime.Now.AddDays(days);
            foreach (proizvod p in proizvodi)
            {
                // Provjera je li datum isteka roka trajanja <= granica. 
                if (DateTime.Compare(p.exp, cutoff_date) <= 0)
                {
                    expired_products.Add(p);
                }
            }
            return expired_products;
        }

        private void showList(int days)
        {
            List<proizvod> expired_products = GetExpiredProducts(days);

            // Inicijaliziramo tablicu u kojoj cemo prikazati proizvode isteklog roka. 
            productControl header = new productControl();
            header.Width = productTable.Width;
            header.detButton = false;
            header.textSize = 12;
            RowStyle temp = productTable.RowStyles[0];
            ColumnStyle temp2 = productTable.ColumnStyles[0];

            productTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            productTable.Controls.Add(header, 0, productTable.RowCount - 1);
            productTable.SetColumnSpan(header, productTable.ColumnCount);
            productTable.RowCount++;

            // Dodajemo proizvod po proizvod u tablicu. 
            foreach (proizvod p in expired_products)
            {
                // Inicijaliziramo productControl za proizvod p kojeg trenutno obradjujemo. 
                productControl prod = new productControl();
                prod.id = p.id;
                prod.name = p.name;
                prod.code = p.code;
                prod.exp = p.exp;
                prod.price = p.price;
                prod.Width = productTable.Width;

                temp = productTable.RowStyles[0];

                productTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                productTable.Controls.Add(prod, 0, productTable.RowCount - 1);
                productTable.SetColumnSpan(prod, productTable.ColumnCount);
                // Omogucavamo prikaz vise detalja za proizvod koristenjem ProductDetails objekta. 
                prod.detail += (sender, e) =>
                {
                    ProductDetails prodDet = new ProductDetails(p,Form1.name);
                    prodDet.ShowDialog();
                };

                productTable.RowCount++;
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Praznimo tablicu. 
            while (productTable.Controls.Count > 0)
            {
                productTable.Controls[0].Dispose();
            }

            // Ako u textboxu postoji broj, generiramo odgovarajuc sadrzaj tablice.
            if (int.TryParse(textBox1.Text, out _))
            {
                showList(int.Parse(textBox1.Text.ToString()));
            }
        }
    }


}
