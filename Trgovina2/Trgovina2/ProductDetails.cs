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
    public partial class ProductDetails : Form //detalji jednog proizvoda
    {
        int _id;
        dataBase db;
        string _name;
        public ProductDetails()
        {
            this.MinimumSize = new System.Drawing.Size(650, 630);
            InitializeComponent();
        }

        public ProductDetails(proizvod p, string name) // ispiši detalje o proizvodu (naziv, kod, cijena, kategorija, količina, datum nabave, rok trajanja)
        {
            InitializeComponent();
            _id = p.id;
            _name = name;
            nameTextBox.Text = p.name;
            codeTextBox.Text = p.code;
            quantLabel.Text = p.quant.ToString();
            catLabel.Text = p.cat;
            priceTextBox.Text = p.price.ToString();
            expLabel.Text = p.exp.ToString("d.M.yyyy");
            dateLabel.Text = p.date.ToString("d.M.yyyy");
            if (_name != "admin") // samo admin smije mijenjati naziv, kod i cijenu  proizvoda, te dodavati i brisati popuste
            {
                changeCodeButton.Enabled = false;
                changeNameButton.Enabled = false;
                changePriceButton.Enabled = false;
                addDiscButton.Enabled = false;
                delButton.Enabled = false;
            }
            db = new dataBase();
            showDiscounts();

        }

        private void showDiscounts() //stvaramo tablicu s popustima za proizvod čiji se detalji prikazuju
        {
            List<discount> discountList = db.getDiscountsByProuctId(_id); //dohvaćamo sve popuste za zadani proizvod
            discountControl header = new discountControl(); //zaglavlje tablice
            header.Width = discountTable.Width;
            header.chgButton = false; //mičemo gumbe s kontrole (jer je to zaglavlje)
            header.optionCombo = false;
            header.delButton = false;
            header.textSize = 10;
            RowStyle temp = discountTable.RowStyles[0];

            discountTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            discountTable.Controls.Add(header, 0, discountTable.RowCount - 1); //dodaj zaglavlje u tablicu
            discountTable.SetColumnSpan(header, discountTable.ColumnCount);
            discountTable.RowCount++;
            foreach (discount d in discountList) //dodajemo popuste u tablicu
            {
                discountControl disc = new discountControl(_name); //stvaramo novu kontrolu za popust
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
                    if (option == "Postotak")
                    {
                        db.changeDiscountPercent(e.id, e.percent);
                    }

                    else if (option == "Datum početka")
                    {
                        db.changeDiscountFrom(e.id, e.from);
                    }

                    else if (option == "Datum završetka")
                    {
                        db.changeDiscountTo(e.id, e.to);
                    }
                };

                disc.delete += (sender, e) => //dodajemo događaj brisanja popusta
                {
                    db.deleteDiscount(disc.id);
                    discountTable.Controls.Remove(disc);
                };

                discountTable.RowCount++;
            }

        }

        private void addDiscButton_Click(object sender, EventArgs e) //dodajemo novi popust
        {

            addDiscount add = new addDiscount(_id); //stvaramo dijalog za dodavanje popusta
            DialogResult res = add.ShowDialog();
            Console.WriteLine(res);
            if (res != DialogResult.Yes) return;
            discount d = db.getNewestDiscount(_id); //dohvaćamo novi popust
            discountControl disc = new discountControl(); //stvaramo kontrolu za novi popust

            disc.id = d.id;
            disc.productId = d.productId;
            disc.percent = d.percent;
            disc.from = d.from;
            disc.to = d.to;

            RowStyle temp = discountTable.RowStyles[0];

            discountTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            discountTable.Controls.Add(disc, 0, discountTable.RowCount - 1); //dodaj novu kontrolu u tablicu
            discountTable.SetColumnSpan(disc, discountTable.ColumnCount);
            disc.change += (sender1, e1) =>
            {
                string option = disc.option;
                if (option == "Postotak")
                {
                    db.changeDiscountPercent(e1.id, e1.percent);
                }

                else if (option == "Datum početka")
                {
                    db.changeDiscountFrom(e1.id, e1.from);
                }

                else if (option == "Datum završetka")
                {
                    db.changeDiscountTo(e1.id, e1.to);
                }
            };

            disc.delete += (sender1, e1) => //dodajemo događaj za brisanje popusta
            {
                db.deleteDiscount(disc.id);
                discountTable.Controls.Remove(disc);
            };

            discountTable.RowCount++;
        }

        private void changePriceButton_Click(object sender, EventArgs e) //promijeni cijenu proizvoda
        {
            priceTextBox.ReadOnly = false;
            priceTextBox.Text = "";
            priceTextBox.Focus();
        }

        private void changeCodeButton_Click(object sender, EventArgs e) //promijeni kod proizvoda
        {
            codeTextBox.ReadOnly = false;
            codeTextBox.Text = "";
            codeTextBox.Focus();
        }

        private void changeNameButton_Click(object sender, EventArgs e) //promijeni naziv proizvoda
        {
            nameTextBox.ReadOnly = false;
            nameTextBox.Text = "";
            nameTextBox.Focus();
        }

        private void nameTextBox_KeyUp(object sender, KeyEventArgs e) //potvrdi novi naziv prozvoda
        {
            if (e.KeyCode == Keys.Enter)    //potvrda je pritisak na enter
            {
                TextBox tb = (TextBox)sender;
                db.changeName(_id, tb.Text);
                tb.ReadOnly = true;
                label2.Focus();
                
                
            }
        }

        private void codeTextBox_KeyUp(object sender, KeyEventArgs e) //potvrdi novi kod prozvoda
        {
            if (e.KeyCode == Keys.Enter) //potvrda je pritisak na enter
            {
                TextBox tb = (TextBox)sender;
                db.changeCode(_id, tb.Text);
                tb.ReadOnly = true;
                label2.Focus();
            }
        }

        private void priceTextBox_KeyUp(object sender, KeyEventArgs e) //potvrdi novu cijenu prozvoda
        {
            if (e.KeyCode == Keys.Enter) //potvrda je pritisak na enter
            {
                TextBox tb = (TextBox)sender;
                db.changePrice(_id,int.Parse(tb.Text));
                tb.ReadOnly = true;
                label2.Focus();
            }
        }

        private void delButton_Click(object sender, EventArgs e) //makni proizvod iz baze
        {
            db.deleteProduct(_id);
            this.Close();
        }
    }
}
