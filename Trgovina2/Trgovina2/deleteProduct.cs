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
    public partial class deleteProduct : UserControl //kontrola za brisanje proizvoda
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

        public string name //naziv proizvoda
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public string code //kod proizvoda
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        public int quant //količina proizvoda
        {
            get { return int.Parse(label3.Text); }
            set { label3.Text = value.ToString(); }
        }

        public DateTime date //datum isteka roka trajanja proizvoda
        {
            get { return DateTime.Parse(label4.Text); }
            set { label4.Text = value.ToString("d.M.yyyy"); }
        }


        public bool delButton
        {
            set { deleteButton.Visible = value; }
        }

        public int textSize
        {
            set 
            { 
                foreach(Control c in Controls)
                {
                    if(c is Label) c.Font = new Font(c.Font.Name, value, c.Font.Style);
                } 
            }
        }

        public event EventHandler<proizvod> izbrisi;

        private void deleteButton_Click(object sender, EventArgs e) //brisanje proizvoda
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
    }
}
