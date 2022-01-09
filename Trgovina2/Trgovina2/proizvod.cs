using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina2
{
    public class proizvod
    {
        public int id;
        public string name;
        public string code;
        public double price;
        public string cat;
        public DateTime exp;
        public DateTime date;
        public int quant;

        public proizvod() { }
        public proizvod(int i, string n, string c, double p, DateTime e, DateTime d, int q, string ct)
        {
            id = i;
            name = n;
            code = c;
            price = p;
            exp = e;
            date = d;
            quant = q;
            cat = ct;
        }


    }
}
