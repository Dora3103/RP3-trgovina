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
    public partial class Removeproduct : Form //obriši proizvod
    {
        public Removeproduct()
        {
            InitializeComponent();
            addToTable(); //prikazujemo proizvode kojima je istekao rok trajanja jer ih treba maknuti iz baze
        }

        public void addToTable()
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.serachForExpired(); //dohvaćamo proizvode kojima je istekao rok trajanja
            deleteProduct header = new deleteProduct(); //zaglavlje tablice
            header.delButton = false;
            header.textSize = 12;
            RowStyle temp = dpTable.RowStyles[0];
            dpTable.RowCount++;
            dpTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            dpTable.Controls.Add(header, 0, dpTable.RowCount - 1); //dodaj zaglavlje
            dpTable.SetColumnSpan(header, dpTable.ColumnCount);
            foreach (proizvod p in proizvodi) //dodajemo proizvode u tablicu
            {
                deleteProduct dp = new deleteProduct(); //stvori novu kontrolu
                dp.id = p.id;
                dp.name = p.name;
                dp.code = p.code;
                dp.quant = p.quant;
                dp.date = p.exp;
                dp.Width = dpTable.Width;

                temp = dpTable.RowStyles[0]; //dodaj kontrolu u tablicu
                dpTable.RowCount++;
                dpTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                dpTable.Controls.Add(dp, 0, dpTable.RowCount - 1);
                dpTable.SetColumnSpan(dp, dpTable.ColumnCount);
                dp.izbrisi += (sender, e) => //dodaj događaj brisanja proizvoda
                {
                    delete(new proizvod()
                    {
                        id = dp.id,
                        name = dp.name,
                        code = dp.code,
                        quant = dp.quant,
                        exp = dp.date
                    });
                    dpTable.Controls.Remove(dp); //makni kontrolu s izbrisanim proizvodom iz tablice
                };
            }
        }

        private void delete(proizvod p) //izbriši proizvod
        {
            dataBase db = new dataBase();
            db.deleteProduct(p.id); //izbriši proizvod iz baze
            int i = db.checkQuantity(p.name); //provjeri koliko komada proizvoda je još ostalo
            if (i == -1)
                MessageBox.Show("Trenutno je manje od 10 komada proizvoda " + p.name + "!");
            if(i == -2)
                MessageBox.Show("Više nema proizvoda " + p.name + " za prodaju!");


        }
    }
}
