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
    public partial class addDiscount : Form
    {
        int productId;
        public addDiscount(int id)
        {
            productId = id;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            dataBase db = new dataBase();
            discount d = new discount()
            {
                productId = productId,
                percent = (double)numericUpDown1.Value,
                from = fromDateTimePicker.Value,
                to = toDateTimePicker.Value
            };
            db.addDiscount(d);
            this.Close();

        }
    }
}
