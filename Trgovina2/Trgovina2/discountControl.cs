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
    public partial class discountControl : UserControl
    {
        private int _id;
        private int _productId;
        public discountControl()
        {
            InitializeComponent();
        }

        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        public int productId
        {
            set { _productId = value; }
            get { return _productId; }
        }

        public double percent
        {
            get { return double.Parse(percentTextBox.Text); }
            set { percentTextBox.Text = value.ToString(); }
        }

        public DateTime from
        {
            get { return DateTime.Parse(fromTextBox.Text); }
            set { fromTextBox.Text = value.ToString("d.M.yyyy"); }
        }

        public DateTime to
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

        private void changeButton_Click(object sender, EventArgs e)
        {
            if(change != null)
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
        }

        private void deleteButton_Click(object sender, EventArgs e)
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
    }

    
}
