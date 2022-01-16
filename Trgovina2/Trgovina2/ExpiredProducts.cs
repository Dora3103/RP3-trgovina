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
            InitializeComponent();
            showList(0);
        }


        private List<proizvod> GetExpiredProducts(int days)
        {
            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.allProducts();

            List<proizvod> prosao_rok = new List<proizvod>();

            DateTime granica = DateTime.Now.AddDays(days);
            foreach (proizvod p in proizvodi)
            {
                if (DateTime.Compare(p.exp, granica) < 0)
                {
                    prosao_rok.Add(p);
                }
            }
            return prosao_rok;
        }

        private void showList(int days)
        {
            List<proizvod> expired_products = GetExpiredProducts(days);

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
            foreach (proizvod p in expired_products)
            {
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
                prod.detail += (sender, e) =>
                {
                    ProductDetails prodDet = new ProductDetails(p);
                    prodDet.ShowDialog();
                };

                productTable.RowCount++;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "Prikaz proizvoda kojima rok trajanja ističe u sljedećih " + textBox1.Text + " dana ";
            while (productTable.Controls.Count > 0)
            {
                productTable.Controls[0].Dispose();
            }

            showList(int.Parse(textBox1.Text.ToString()));
        }
    }


}
