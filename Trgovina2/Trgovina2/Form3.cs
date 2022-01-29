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
    public partial class Newproduct : Form //dodavanje novog proizvoda
    {
        public Newproduct()
        {
            this.MinimumSize = new System.Drawing.Size(400, 430);
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
