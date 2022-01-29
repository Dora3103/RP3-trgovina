using System;
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
        public List<proizvod> serachForExpired() //dohvati sve proizvode kojima je istekao rok trajanja (u odnosu na današnji datum)
        {
            List<proizvod> ret = new List<proizvod>();
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();

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
            }

            return ret;
        }

        public void deleteProduct(int id) //izbriši proizvod s ID-em id
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("delete from proizvodi where ID = " + id, con);
                int deleted = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public bool addProduct(proizvod p) //dodaj novi proizvod
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            int check = checkIfExists(p);
            if (check == 1) return upgradeQuantity(p);
            if (check == 0) return false;
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("insert into proizvodi ([Naziv],[Kategorija],[Kolicina],[Kod],[Cijena],[Rok_trajanja],[Datum_nabave]) values('" +
                   p.name + "','" + p.cat + "'," + p.quant.ToString() + ",'" + p.code + "'," + p.price + "," + p.exp.ToString("#d/M/yyyy#") + "," + p.date.ToString("#d/M/yyyy#")+")", con);
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

        public int checkIfExists(proizvod p) //provjeri postoji li već proizvod sa zadanim svojstvima
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select count(*) from proizvodi where Naziv = '" + p.name + "' and Kod = '" + p.code + "' and Rok_trajanja = "+p.exp.ToString("#d/M/yyyy#"), con);
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

        public bool upgradeQuantity(proizvod p) //promijeni količinu proizvoda (povećaj)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select Kolicina from proizvodi where Naziv = '" + p.name + "' and Kod = '" + p.code + "' and Rok_trajanja = "
                    + p.exp.ToString("#d/M/yyyy#") + "Datum_nabave = " + p.date.ToString("#d/M/yyyy#"), con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                Console.WriteLine(dt.Rows[0]["Kolicina"]);
                if(dt.Rows.Count > 1)
                {
                    con.Close();
                    return false;
                }
                int newQuant = p.quant + int.Parse(dt.Rows[0]["Kolicina"].ToString());

                OleDbCommand cmd = new OleDbCommand("update proizvodi set Kolicina = " + newQuant + "where Naziv =  '" + p.name + "' and Kod = '" + p.code + "' and Rok_trajanja = "
                    + p.exp.ToString("#d/M/yyyy#") + "Datum_nabave = " + p.date.ToString("#d/M/yyyy#"), con);
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

        public int checkQuantity(string productName) //provjeri koliko komada proizvoda s imenom productName je ostalo
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

        public List<proizvod> allProducts() //vrati listu svih proizvoda u bazi
        {
            List<proizvod> ret = new List<proizvod>();
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select * from proizvodi order by Naziv, Rok_trajanja", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    ret.Add(new proizvod()
                    {
                        id = int.Parse(r["ID"].ToString()),
                        name = r["Naziv"].ToString(),
                        cat = r["Kategorija"].ToString(),
                        price = double.Parse(r["Cijena"].ToString()),
                        quant = int.Parse(r["Kolicina"].ToString()),
                        code = r["Kod"].ToString(),
                        exp = DateTime.Parse(r["Rok_trajanja"].ToString()),
                        date = DateTime.Parse(r["Datum_nabave"].ToString())
                    });
                }
                con.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ret;
            }
            return ret;
        }

        public List<discount> getDiscountsByProuctId(int id) //dohvati popuste za proizvod s ID-em id
        {
            List<discount> ret = new List<discount>();
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select * from popust where proizvodId = " +id, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    ret.Add(new discount()
                    {
                        id = int.Parse(r["ID"].ToString()),
                        productId = int.Parse(r["proizvodId"].ToString()),
                        percent = int.Parse(r["postotakPopusta"].ToString()),
                        from = DateTime.Parse(r["datumOd"].ToString()),
                        to = DateTime.Parse(r["datumDo"].ToString())
                    });
                }
                con.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ret;
            }


            return ret;
        }

        public bool addDiscount(discount d) //dodaj novi popust
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("insert into popust ([proizvodId],[postotakPopusta],[datumOd],[datumDo]) values(" +
                   d.productId + "," + d.percent + ","  + d.from.ToString("#d/M/yyyy#") + "," + d.to.ToString("#d/M/yyyy#") + ")", con);
                int inserted = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public bool changeName(int id, string name) //promijeni ime proizvoda s ID-em id u name
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {

                OleDbCommand cmd = new OleDbCommand("update proizvodi set Naziv = '" + name + "' where ID =  " + id, con);
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

        public bool changePrice(int id, int price) //promijeni cijenu proizvoda s ID-em id u price
        {

            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {

                OleDbCommand cmd = new OleDbCommand("update proizvodi set Cijena = " + price + " where ID =  " + id, con);
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

        public bool changeCode(int id, string code) //promijeni kod proizvoda s ID-em id u code
        {

            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {

                OleDbCommand cmd = new OleDbCommand("update proizvodi set Kod = '" + code + "' where ID =  " + id, con);
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

        public bool deleteDiscount(int id) //obriši popust s ID-em id
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("delete from popust where ID = " + id, con);
                int deleted = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public discount getNewestDiscount(int productId) //pronađi zadnji dodani popust za zadani proizvod
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            discount ret = new discount();
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select top 1 * from popust where proizvodId = " + productId + " order by ID desc", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    ret.id = int.Parse(r["ID"].ToString());
                    ret.productId = int.Parse(r["proizvodId"].ToString());
                    ret.percent = int.Parse(r["postotakPopusta"].ToString());
                    ret.from = DateTime.Parse(r["datumOd"].ToString());
                    ret.to = DateTime.Parse(r["datumDo"].ToString());
                }
                con.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ret;
            }
            return ret;
        }
    }
}
