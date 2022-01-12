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
    public partial class BillControl : UserControl
    {
        int _cijena;

        public BillControl()
        {
            InitializeComponent();
        }

        public string kod
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string naziv
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public string kolicina
        {
            get { return textBox3.Text; }
            set { textBox3.Text = value; }
        }

        public int cijena
        {
            get => _cijena;
            set { _cijena = value; }
        }
    }
}
