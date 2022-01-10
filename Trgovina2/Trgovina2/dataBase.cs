﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina2
{
    internal class dataBase
    {
        public List<proizvod> serachForExpired()
        {
            List<proizvod> ret = new List<proizvod>();
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                //OleDbDataAdapter sda = new OleDbDataAdapter("select ID, Naziv, Kolicina, Kod, Rok_trajanja from proizvodi where Rok_trajanja >" + DateTime.Today.ToString("dd-MM-yy"), con);
                OleDbDataAdapter sda = new OleDbDataAdapter("select ID, Naziv, Kolicina, Kod, Rok_trajanja from proizvodi where Rok_trajanja < Date() order by Rok_trajanja, Naziv", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach(DataRow row in dt.Rows)
                {
                    ret.Add(new proizvod()
                    {
                        id = int.Parse(row[0].ToString()),
                        name = row[1].ToString(),
                        quant = int.Parse(row[2].ToString()),
                        code = row[3].ToString(),
                        exp = DateTime.Parse(row[4].ToString())

                    }) ;
                }
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }

            return ret;
        }

        public void deleteProduct(proizvod p)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("delete from proizvodi where ID = " + p.id.ToString(), con);
                int deleted = cmd.ExecuteNonQuery();
                //Console.WriteLine("broj obrisanih redova " + deleted.ToString());
                con.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public bool addProduct(proizvod p)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            //if (checkIfExists(p) == 1) return upgradeQuantity(p);
            //if (checkIfExists(p) == 0) return false;
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("insert into proizvodi ([Naziv],[Kategorija],[Kolicina],[Kod],[Cijena],[Rok_trajanja],[Datum_nabave]) values('" +
                   p.name + "','" + p.cat + "'," + p.quant.ToString() + ",'" + p.code + "'," + p.price + "," + p.exp.ToString("#d/M/yyyy#") + "," + p.date.ToString("#d/M/yyyy#")+")", con);
                //Console.WriteLine(p.name);
                 //OleDbCommand cmd = new OleDbCommand("insert into proizvodi ([Naziv]) values('" +
                   // p.name + "')", con);
                int inserted = cmd.ExecuteNonQuery();
                Console.WriteLine("ubaceno redaka " + inserted);
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public int checkIfExists(proizvod p)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                /*  OleDbDataAdapter sda = new OleDbDataAdapter("select count(*) from proizvodi where Naziv = '" + p.name + "' and Kod = '" + p.code + "' and Rok_trajanja = " 
                      + p.exp.ToString("dd-MMM-yy") + " and Datum_nabave = " + p.date.ToString("dd-MMM-yy"), con);*/
                Console.WriteLine(p.exp.ToString("dd-MMM-yy"));
                OleDbDataAdapter sda = new OleDbDataAdapter("select count(*) from proizvodi where Naziv = '" + p.name + "' and Kod = '" + p.code + "' and Rok_trajanja = "+p.exp.ToString("dd-MM-yy"), con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    Console.WriteLine("postoji");
                    con.Close();
                    return 1;
                }
                else
                {
                    con.Close();
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }

            
        }

        public bool upgradeQuantity(proizvod p)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select Kolicina from proizvodi where Naziv = '" + p.name + "' and Kod = '" + p.code + "' and Rok_trajanja = "
                    + p.exp.ToString("dd-MM-yy") + "Datum_nabave = " + p.date.ToString("dd-MM-yy"), con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                Console.WriteLine(dt.Rows[0][0]);
                if(dt.Rows.Count > 1)
                {
                    con.Close();
                    return false;
                }
                OleDbCommand cmd = new OleDbCommand("update proizvodi set Kolicina = " + p.quant + dt.Rows[0][0] + "where Naziv =  '" + p.name + "' and Kod = '" + p.code + "' and Rok_trajanja = "
                    + p.exp.ToString("dd-MM-yy") + "Datum_nabave = " + p.date.ToString("dd-MM-yy"), con);
                int updated = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public int checkQuantity(string productName)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select Kolicina from proizvodi where Naziv = '" + productName + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int quant = 0;
                foreach(DataRow r in dt.Rows)
                {
                    quant += int.Parse(r[0].ToString());
                }
                if (quant > 10) return 1;
                else if (quant == 0) return -2;
                return -1;
               
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }

        }
    }
}
