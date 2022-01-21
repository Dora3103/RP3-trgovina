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

        // kontrola koja služi za prikazivanje računu nakon što ga je korisnik kreirao
        public BillFinalControl()
        {
            InitializeComponent();
            // postavljamo ime blagajnika/blagajnice
            label2.Text = Form1.name;
            // postavljamo današnji datum i vrijeme
            labelDate.Text = DateTime.Now.ToString();
        }

        // funkcija koju pozivamo u AddBill formi i kojom punimo tablicu za prikazivanje proizvoda iz ovog računa
        public void populateTable(string[] row)
        {
            dataGridView1.Rows.Add(row);
        }

        // funkcija kojom postavljamo labele iz forme AddBill
        public void populateLabels(string ammount, string type)
        {
            labelTotal.Text = ammount;
            labelType.Text = type;
        }

    }
}
