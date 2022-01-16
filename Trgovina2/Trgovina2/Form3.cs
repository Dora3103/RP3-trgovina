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
    public partial class Newproduct : Form
    {
        public Newproduct()
        {
            InitializeComponent();
        }

        private void addProduct1_dodaj(object sender, proizvod e)
        {
            dataBase db = new dataBase();
            if (db.addProduct(e))
            {
                MessageBox.Show("Proizvod dodan!");
                Close();
            }
                
            else
                MessageBox.Show("Dogodila se greška!");
        }
    }
}
