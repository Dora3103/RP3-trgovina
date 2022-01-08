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
                OleDbDataAdapter sda = new OleDbDataAdapter("select Naziv, Kolicina, Kod, Rok_trajanja from proizvodi where Rok_trajanja >" + new DateTime(2002,1,1).ToString("dd-MM-yy"), con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach(DataRow row in dt.Rows)
                {
                    ret.Add(new proizvod()
                    {
                        name = row[0].ToString(),
                        quant = int.Parse(row[1].ToString()),
                        code = row[2].ToString(),
                        //exp = (DateTime)row[3]

                    });
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
    }
}
