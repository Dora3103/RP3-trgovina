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
    public partial class LowStockProducts : Form
    {
        // Parametar quant predstavlja zeljenu minimalnu kolicinu proizvoda. 
        // Kad kolicina proizvoda koju imamo dostupnu padne ispod te kolicine, 
        // stvaramo obavijest. 
        public LowStockProducts(int quant)
        {
            this.MinimumSize = new System.Drawing.Size(800, 400);
            InitializeComponent();

            textBox1.Text = quant.ToString();
            showList(quant);
        }


        // Prikazuje tablicu koja sadrzi sve one proizvode kojih na zalihi ima manje od zadane kolicine. 
        private void showList(int quant)
        {
            dataBase db = new dataBase();
            List<proizvod> low_in_stock = db.productsLowQuantity(quant);

            productTable.Controls.Clear();
            productTable.RowCount = 1;

            // Inicijalizacija tablice kojom cemo prikazati proizvode s niskim zalihama,
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
            foreach (proizvod p in low_in_stock)
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
                    ProductDetails prodDet = new ProductDetails(p, Form1.name);
                    prodDet.ShowDialog();
                };

                productTable.RowCount++;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Praznimo tablicu kad se promijenio argument pretrage. 
            // Tablicu cemo ponovno popuniti s novim rezultatima.
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
