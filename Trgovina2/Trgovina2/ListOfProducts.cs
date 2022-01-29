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
    public partial class ListOfProducts : Form //lista svih proizvoda
    {
        public ListOfProducts()
        {
            InitializeComponent();
            showList();
        }

        private void showList() //stvaramo listu svih proizvoda
        {
            dataBase db = new dataBase();
            List<proizvod> proizvodi = db.allProducts(); //dohvaćamo sve proizvoda

            productControl header = new productControl(); //zaglavllje tablice
            header.Width = productTable.Width;
            header.detButton = false;
            header.textSize = 12;
            RowStyle temp = productTable.RowStyles[0];
            ColumnStyle temp2 = productTable.ColumnStyles[0];
            
            productTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            productTable.Controls.Add(header, 0, productTable.RowCount - 1); //dodaj zaglavlje u tablicu
            productTable.SetColumnSpan(header, productTable.ColumnCount);
            productTable.RowCount++;
            foreach (proizvod p in proizvodi) //dodajemo proizvode u tablicu
            {
                productControl prod = new productControl();
                prod.id = p.id;
                prod.name = p.name;
                prod.code = p.code;
                prod.exp = p.exp;
                prod.price = p.price;
                prod.Width = productTable.Width;
                //prod.labelMaxWidth = (int)Math.Ceiling(temp2.Width);
                //prod.labelAutoSize = true;

                temp = productTable.RowStyles[0]; //dodaj kontrolu u tablicu
                productTable.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                productTable.Controls.Add(prod, 0, productTable.RowCount - 1);
                productTable.SetColumnSpan(prod, productTable.ColumnCount);
                prod.detail += (sender, e) => //događaj prikazivanja detalja o proizvodu
                {
                     ProductDetails prodDet = new ProductDetails(p);
                     prodDet.ShowDialog(); //otvori dialog s detaljima o proizvodu
                };

                productTable.RowCount++;
            }
        }
    }
}
