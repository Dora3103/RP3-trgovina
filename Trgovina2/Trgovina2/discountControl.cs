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
    public partial class discountControl : UserControl //kontrola popust
    {
        private int _id;
        private int _productId;
        public discountControl()
        {
            InitializeComponent();
        }

        public discountControl(string name)
        {
            InitializeComponent();
            if(name != "admin")
            {
                deleteButton.Enabled = false; //samo admin smije brisati ili mijenjati popust
                changeButton.Enabled = false;
                chooseComboBox.Enabled = false;
            }
        }

        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        public int productId //id proizvoda
        {
            set { _productId = value; }
            get { return _productId; }
        }

        public double percent //postotak popusta
        {
            get { return double.Parse(percentTextBox.Text); }
            set { percentTextBox.Text = value.ToString(); }
        }

        public DateTime from //datum početka popusta
        {
            get { return DateTime.Parse(fromTextBox.Text); }
            set { fromTextBox.Text = value.ToString("d.M.yyyy"); }
        }

        public DateTime to //datum završetka popusta
        {
            get { return DateTime.Parse(toTextBox.Text); }
            set { toTextBox.Text = value.ToString("d.M.yyyy"); }
        }

        public bool chgButton
        {
            set { changeButton.Visible = value; }
        }

        public bool optionCombo
        {
            set { chooseComboBox.Visible = value; }
        }

        public bool delButton
        {
            set { deleteButton.Visible = value; }
        }


        public int textSize
        {
            set
            {
                foreach (Control c in Controls)
                {
                    if (c is Label) c.Font = new Font(c.Font.Name, value, c.Font.Style);
                }
            }
        }

        public int labelMaxWidth
        {
            set
            {
                foreach (Control c in Controls)
                {
                    if (c is Label) c.MaximumSize = new Size(value, 0);
                }
            }
        }

        public bool labelAutoSize
        {
            set
            {
                foreach (Control c in Controls)
                {
                    if (c is Label) c.AutoSize = value;
                }
            }
        }

        public string option
        {
            get { return chooseComboBox.Text; }
        }

        public event EventHandler<discount> change;
        public event EventHandler<discount> delete;

        private void changeButton_Click(object sender, EventArgs e) //promijeni popust
        {
            
            string option = chooseComboBox.Text;
            if (option == "Postotak") //upiši novi postotak
            {
                percentTextBox.ReadOnly = false;
                percentTextBox.Text = "";
                percentTextBox.Focus();
            }
            else if (option == "Datum početka") //upiši novi datum početka
            {
                fromTextBox.ReadOnly = false;
                fromTextBox.Text = "";
                fromTextBox.Focus();
            }
            else if (option == "Datum završetka") //upiši novi datum završetka
            {
                toTextBox.ReadOnly = false;
                toTextBox.Text = "";
                toTextBox.Focus();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e) //obriši popust
        {
            if (delete != null)
            {
                delete(sender, new discount()
                {
                    id = _id,
                    productId = _productId,
                    percent = percent,
                    from = from,
                    to = to
                });
            }
        }

        private void percentTextBox_KeyUp(object sender, KeyEventArgs e) //potvrdi novi postotak
        {
            if (e.KeyCode == Keys.Enter)    //potvrda je pritisak na enter
            {
                TextBox tb = (TextBox)sender;
                if (change != null)
                {
                    change(sender, new discount()
                    {
                        id = _id,
                        productId = _productId,
                        percent = percent,
                        from = from,
                        to = to
                    });
                }
                tb.ReadOnly = true;
                chooseComboBox.Focus();
            }
        }

        private void fromTextBox_KeyUp(object sender, KeyEventArgs e) //potvrdi novi datum početka
        {
            if (e.KeyCode == Keys.Enter)    //potvrda je pritisak na enter
            {
                TextBox tb = (TextBox)sender;
                if (change != null)
                {
                    change(sender, new discount()
                    {
                        id = _id,
                        productId = _productId,
                        percent = percent,
                        from = from,
                        to = to
                    });
                }
                tb.ReadOnly = true;
                chooseComboBox.Focus();
            }
        }

        private void toTextBox_KeyUp(object sender, KeyEventArgs e) //potvrdi novi datum završetka
        {
            if (e.KeyCode == Keys.Enter)    //potvrda je pritisak na enter
            {
                TextBox tb = (TextBox)sender;
                if (change != null)
                {
                    change(sender, new discount()
                    {
                        id = _id,
                        productId = _productId,
                        percent = percent,
                        from = from,
                        to = to
                    });
                }
                tb.ReadOnly = true;
                chooseComboBox.Focus();
            }
        }
    }

    
}
