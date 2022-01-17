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
    public partial class BillFinalControl : UserControl
    {
        public BillFinalControl()
        {
            InitializeComponent();
            label2.Text = Form1.quantity;
            labelDate.Text = DateTime.Now.ToString();
        }

        public void populateTable(string[] row)
        {
            dataGridView1.Rows.Add(row);
        }

        public void populateLabels(string ammount, string type)
        {
            labelTotal.Text = ammount;
            labelType.Text = type;
        }

    }
}
