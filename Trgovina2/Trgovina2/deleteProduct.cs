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
    public partial class deleteProduct : UserControl
    {
        public int _id;
        public deleteProduct()
        {
            InitializeComponent();

        }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string name
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public string code
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        public int quant
        {
            get { return int.Parse(label3.Text); }
            set { label3.Text = value.ToString(); }
        }

        public DateTime date
        {
            get { return new DateTime(int.Parse(label4.Text.Split('.')[2]), int.Parse(label4.Text.Split('.')[1]), int.Parse(label4.Text.Split('.')[0])); }
            set { label4.Text = value.ToString("d.M.yyyy"); }
        }

        public event EventHandler<proizvod> izbrisi;

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if(izbrisi != null)
            {
                izbrisi(this, new proizvod
                {
                    name = name,
                    code = code,
                    quant = quant,
                    exp = date
                });
            }
        }
    }
}
