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
    public partial class addProduct : UserControl
    {
        public addProduct()
        {
            InitializeComponent();
        }

        public string name
        {
            get { return nameTextBox.Text; }
        }

        public string code
        {
            get { return codeTextBox.Text; }
        }

        public double price
        {
            get { return double.Parse(priceTextBox.Text); }
        }

        public string cat
        {
            get { return catBox.Text; }
        }

        public int quant
        {
            get { return int.Parse(quantTextBox.Text); }
        }

        public DateTime exp
        {
            get { return expDateTimePicker.Value; }
        }

        public DateTime date
        {
            get { return dateDateTimePicker.Value; }
              
        }

        public event EventHandler<proizvod> dodaj;

        private void button1_Click(object sender, EventArgs e)
        {
            if ( dodaj != null)
            {
                dodaj(this, new proizvod
                {
                    
                    name = name,
                    code = code,
                    price = price,
                    cat = cat,
                    quant = quant,
                    date = date,
                    exp = exp,

                });
            }
        }

    }
}
