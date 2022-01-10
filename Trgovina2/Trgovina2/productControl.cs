﻿using System;
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
    public partial class productControl : UserControl
    {
        public int _id;
        public productControl()
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
            get { return int.Parse(label5.Text); }
            set { label5.Text = value.ToString(); }
        }

        public DateTime exp
        {
            get { return DateTime.Parse(label6.Text); }
            set { label6.Text = value.ToString("d.M.yyyy"); }
        }

        public DateTime date
        {
            get { return DateTime.Parse(label7.Text); }
            set { label7.Text = value.ToString("d.M.yyyy"); }
        }

        public string cat
        {
            get { return label3.Text; }
            set { label3.Text = value; }
        }

        public double price
        {
            get { return double.Parse(label4.Text); }
            set { label4.Text = value.ToString(); }
        }

        /*public void removeButton()
        {
            Controls.Remove(deleteButton);
        }*/


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
    }
}