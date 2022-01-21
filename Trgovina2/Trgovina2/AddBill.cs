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
    public partial class AddBill : Form
    {
        double pdv = 25;
        public AddBill()
        {
            InitializeComponent();
            fetchData();
            comboBox1.SelectedIndex = 0;
            textBox4.Text = "25";
        }

        // funkcija kojom se dohvaćaju proizvodi iz baze ovisno o filteru kojeg korisnik unese te
        // se zatim s njima puni tablica
        public void fetchData()
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

            try
            {
                con.Open();
                OleDbDataAdapter sda = new OleDbDataAdapter("" +
                    "SELECT Naziv, Kategorija, Kolicina, Kod, Cijena, Rok_trajanja, Datum_nabave " +
                    "FROM proizvodi WHERE Naziv LIKE '%" + textBoxName.Text + "%' " +
                    "AND Kod LIKE '%" + textBoxCode.Text + "%'", con);

                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fetchData();
        }

        // klikom na gumb Dodaj dodaje se proizvod određene količine i pdv-a na račun
        private void button2_Click(object sender, EventArgs e)
        {
            // zabranjujemo dodavanje ako nije naveden kod i količina
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Molimo upišite kod i količinu proizvoda kojeg želite dodati na račun.");
            }
            else
            {
                // provjeravamo je li proizvod s odabranim kodom već dodan na račun i ako je obavijestimo korisnika o tome
                foreach (BillControl control in flowLayoutPanel1.Controls)
                {
                    if (control.kod == textBox1.Text)
                    {
                        MessageBox.Show("Ovaj proizvod je već dodan u račun. Obrišite ga prvo pa ga dodajte ponovno.");
                        return;
                    }
                }

                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

                try
                {
                    // provjeramo je li proizvod postoji u bazi i ima li ga dovoljno
                    con.Open();
                    OleDbDataAdapter sda = new OleDbDataAdapter("" +
                        "SELECT * " +
                        "FROM proizvodi WHERE Kod='" + textBox1.Text + "' " +
                        "AND Kolicina>=" + textBox2.Text, con);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // ako proizvoda ima dovoljno u bazi onda kreiramo BillControl-u (kontrolu koja sluši samo za prikaz proizvoda
                    // na računu) i dodajemo tu kontrolu na panel
                    if (dt.Rows.Count == 1)
                    {
                        var kontrola = new BillControl()
                        {
                            kod = textBox1.Text,
                            kolicina = textBox2.Text,
                            naziv = dt.Rows[0]["Naziv"].ToString(),                            
                            cijena = (int)dt.Rows[0]["Cijena"],
                            pdv = textBox4.Text
                        };
                        kontrola.Margin = new Padding(5, 5, 5, 5);
                        flowLayoutPanel1.Controls.Add(kontrola);
                    }
                    // ako ga nema uopce ili nema dovoljno ne dodajemo ga na račun nego ispisujemo poruku korisniku
                    else
                    {
                        MessageBox.Show("Ne postoji proizvod s tim kodom ili ga nema dovoljno. Molimo pokušajte ponovno.");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
            }
        }

        // funkcija koja se poziva kad stisnemo na gumb Ukloni za uklanjanje proizvoda s računa
        // te miče proizvod iz panela (iz računa), a u koliko ne stavimo nikak kod ili upišemo krivi kod
        // ne događa se ništa
        private void button3_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (((BillControl)flowLayoutPanel1.Controls[i]).kod == textBox3.Text)
                {
                    flowLayoutPanel1.Controls.RemoveAt(i);
                }
            }
        }

        // funkcija koja se izvrši kad se klikne na gumb Završi, odnosno kad želimo kreirati račun s izabranim proizvodima
        private void button4_Click(object sender, EventArgs e)
        {
            // provjeravamo jesu li dodani proizvodi na račun i ako nisu obavještavamo korisnika
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("Račun je prazan. Molimo dodajte prvo proizvode.");
                return;
            }

            // otvaramo konekciju s bazom
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

            try
            {
                con.Open();
                OleDbCommand cmd;
                // svaki dodani proizvod mičemo iz baze onoliko puta koliko ga ima na računu
                foreach (BillControl item in flowLayoutPanel1.Controls)
                {
                    cmd = new OleDbCommand("" +
                    "UPDATE proizvodi " +
                    "SET Kolicina=Kolicina - " + item.kolicina + " " +
                    "WHERE Kod='" + item.kod + "'", con);

                    cmd.ExecuteNonQuery();
                }

                // na kraju obrišemo proizvode kojih više nema u bazi, odnosno imaju količinu jednaku 0
                cmd = new OleDbCommand("" +
                    "DELETE FROM proizvodi WHERE Kolicina=0", con);

                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

            // kreiramo kontrolu za prikazivanje napravljenog računa
            BillFinalControl control = new BillFinalControl();
            double total = makeBill(control);

            // pozivamo metodu iz kontrole za postavljanje labela
            control.populateLabels(total.ToString("0.##"), comboBox1.Text.Equals("") ? "Gotovina" : comboBox1.Text);
            this.Hide();
            // kreiramo običnu formu koja će prikazivati kontrolu odnosno račun 
            // te joj dodajemo visinu, širinu, boju, naziv i kontrolu
            Form f = new Form();
            f.BackColor = SystemColors.Window;
            f.Text = "Bill";
            f.Height = 435;
            f.Width = 314;
            f.Controls.Add(control);
            f.ShowDialog();
        }

        // pomoćna funkcija za određivanje ukupne cijene računa
        private double makeBill(BillFinalControl control)
        {      
            // array stringova kojim držimo podatke o dotičnom proizvodu
            string[] row = new string[5];
            // varijabla u kojoj držimo ukupnu cijenu računa, inicijalizirana na 0
            double total = 0;

            // računamo cijenu za svaki proizvod ovisno o tome je li on na sniženju, je li za njega postavljen
            // određen pdv (ili defaultni 25%) te ovisno o odabranoj količini
            foreach (BillControl item in flowLayoutPanel1.Controls)
            {
                row[0] = item.naziv;
                row[1] = item.kolicina;                

                // varijabla u kojoj držimo cijenu za taj proizvod
                double afterDiscount = item.cijena;

                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\login.accdb");

                try
                {
                    con.Open();
                    // provjeravamo ima li u bazi sniženja za taj proizvod i ono trenutno vrijedi
                    OleDbDataAdapter sda = new OleDbDataAdapter("" +
                         "SELECT * " +
                         "FROM popust INNER JOIN proizvodi ON popust.proizvodId = proizvodi.ID " +
                         "WHERE proizvodi.Kod='" + item.kod + "' " +
                         "AND popust.datumOd<=Date() " +
                         "AND popust.datumDo>=Date()", con);                  

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // ako je proizvod trenutno na sniženju onda mu izračunamo cijenu
                    if (dt.Rows.Count == 1)
                    {
                        double percentage = Convert.ToDouble(dt.Rows[0]["postotakPopusta"]);
                        afterDiscount -= afterDiscount * percentage / 100;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }

                // izračunamo konačnu cijenu proizvoda ovisno o količini i pdv-u
                double productTotal = afterDiscount * Convert.ToDouble(item.kolicina) * (Convert.ToDouble(item.pdv) / 100 + 1);
                // dodajemo tu cijenu ukupnoj cijeni računa
                total+= productTotal;
                row[2] = afterDiscount.ToString("0.##");
                row[3] = pdv.ToString() + "%";
                row[4] = productTotal.ToString("0.##");

                // pozivamo funkciju iz kontrole kojom punimo tablicu s proizvodom koji ide na račun
                control.populateTable(row);
            }

            return total;
        }

        // funkcija koja se poziva svaki put kad se promjeni textbox za pdv
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // spremamo ono što je korisnik unio u varijablu pdv
            pdv = double.Parse(textBox4.Text);
        }
    }
}
