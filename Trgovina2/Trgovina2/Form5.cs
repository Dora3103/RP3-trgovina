using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trgovina2
{
    public partial class Removeproduct : Form
    {
        public Removeproduct()
        {
            InitializeComponent();
            addToTable();
        }

        public void addToTable()
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.serachForExpired();
            /*for(int i = dpTable.Controls.Count - 1; i > 3; --i){
                dpTable.Controls.RemoveAt(i);
            }*/
            deleteProduct header = new deleteProduct();
            header.delButton = false;
            header.textSize = 12;
            RowStyle temp = dpTable.RowStyles[0];
            dpTable.RowCount++;
            dpTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            dpTable.Controls.Add(header, 0, dpTable.RowCount - 1);
            dpTable.SetColumnSpan(header, dpTable.ColumnCount);
            foreach (proizvod p in proizvodi)
            {
                deleteProduct dp = new deleteProduct();
                dp.id = p.id;
                dp.name = p.name;
                dp.code = p.code;
                dp.quant = p.quant;
                dp.date = p.exp;
                dp.Width = dpTable.Width;

                temp = dpTable.RowStyles[0];
                dpTable.RowCount++;
                dpTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                dpTable.Controls.Add(dp, 0, dpTable.RowCount - 1);
                dpTable.SetColumnSpan(dp, dpTable.ColumnCount);
                dp.izbrisi += (sender, e) =>
                {
                    delete(new proizvod()
                    {
                        id = dp.id,
                        name = dp.name,
                        code = dp.code,
                        quant = dp.quant,
                        exp = dp.date
                    });
                    dpTable.Controls.Remove(dp);
                };



            }
        }

        private void delete(proizvod p)
        {
            dataBase db = new dataBase();
            db.deleteProduct(p);
            int i = db.checkQuantity(p.name);
            if (i == -1)
                MessageBox.Show("Trenutno je manje od 10 komada proizvoda " + p.name + "!");
            if(i == -2)
                MessageBox.Show("Više nema proizvoda " + p.name + "!");


        }
    }
}
