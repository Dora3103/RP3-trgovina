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
        public List<proizvod> serachForExpired()
        {
            List<proizvod> ret = new List<proizvod>();
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");
            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("select ID, Naziv, Kolicina, Kod, Rok_trajanja from proizvodi where Rok_trajanja >" + new DateTime(2002,1,1).ToString("dd-MM-yy"), con);
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
                        //exp = (DateTime)row[3]

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
                Console.WriteLine("broj obrisanih redova " + deleted.ToString());
                con.Close();
                Console.WriteLine("tu sam");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
