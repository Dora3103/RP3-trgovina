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
    public partial class ProductDetails : Form
    {
        int _id;
        dataBase db;
        public ProductDetails()
        {
            InitializeComponent();
        }

        public ProductDetails(proizvod p)
        {
            InitializeComponent();
            _id = p.id;
            nameTextBox.Text = p.name;
            codeTextBox.Text = p.code;
            quantLabel.Text = p.quant.ToString();
            catLabel.Text = p.cat;
            priceTextBox.Text = p.price.ToString();
            expLabel.Text = p.exp.ToString("d.M.yyyy");
            dateLabel.Text = p.date.ToString("d.M.yyyy");
            db = new dataBase();
            showDiscounts();

        }

        private void showDiscounts()
        {
            List<discount> discountList = db.getDiscountsByProuctId(_id);
            discountControl header = new discountControl();
            header.Width = discountTable.Width;
            header.chgButton = false;
            header.optionCombo = false;
            header.delButton = false;
            header.textSize = 10;
            RowStyle temp = discountTable.RowStyles[0];
            ColumnStyle temp2 = discountTable.ColumnStyles[0];

            discountTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            discountTable.Controls.Add(header, 0, discountTable.RowCount - 1);
            discountTable.SetColumnSpan(header, discountTable.ColumnCount);
            discountTable.RowCount++;
            foreach (discount d in discountList)
            {
                discountControl disc = new discountControl();
                disc.id = d.id;
                disc.productId = d.productId;
                disc.percent = d.percent;
                disc.from = d.from;
                disc.to = d.to;
                //prod.labelMaxWidth = (int)Math.Ceiling(temp2.Width);
                //prod.labelAutoSize = true;

                temp = discountTable.RowStyles[0];

                discountTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                discountTable.Controls.Add(disc, 0, discountTable.RowCount - 1);
                discountTable.SetColumnSpan(disc, discountTable.ColumnCount);
                disc.change += (sender, e) =>
                {
                    string option = disc.option;
                    //db.changeDiscountPercent(e.id, e.percent)
                };

                disc.delete += (sender, e) =>
                {
                    db.deleteDiscount(disc.id);
                    discountTable.Controls.Remove(disc);
                };

                discountTable.RowCount++;
            }

        }

        private void addDiscButton_Click(object sender, EventArgs e)
        {

            addDiscount add = new addDiscount(_id);
            add.Show();

            
        }

        private void changePriceButton_Click(object sender, EventArgs e)
        {
            priceTextBox.ReadOnly = false;
            priceTextBox.Text = "";
            priceTextBox.Focus();
        }

        private void changeCodeButton_Click(object sender, EventArgs e)
        {
            codeTextBox.ReadOnly = false;
            codeTextBox.Text = "";
            codeTextBox.Focus();
        }

        private void changeNameButton_Click(object sender, EventArgs e)
        {
            nameTextBox.ReadOnly = false;
            nameTextBox.Text = "";
            nameTextBox.Focus();
        }

        private void nameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)    
            {
                Console.WriteLine("tu sam");
                TextBox tb = (TextBox)sender;
                db.changeName(_id, tb.Text);
                tb.ReadOnly = true;
                label2.Focus();
                
                
            }
        }

        private void codeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                db.changeCode(_id, tb.Text);
                tb.ReadOnly = true;
                label2.Focus();
            }
        }

        private void priceTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                db.changePrice(_id,int.Parse(tb.Text));
                tb.ReadOnly = true;
                label2.Focus();
            }
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            db.deleteProduct(_id);
        }
    }
}
