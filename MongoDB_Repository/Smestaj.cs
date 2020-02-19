using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using NisvilleFindAccommodation;

namespace MongoDB_Repository
{
    public partial class Smestaj : Form
    {
        public Smestaj()
        {
            InitializeComponent();
        }
        
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                button2.Enabled = false;
                button3.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Accommodation>("listasmestaja");

            string t = listBox1.SelectedItem.ToString();
            string lokacija = textBox1.Text;
            string kvadratura = textBox2.Text;
            string brKreveta = textBox3.Text;
            string dostupnost = textBox4.Text;
            string internet = textBox5.Text;
            //---------------------------------------------------------------------------

            var collection2 = db.GetCollection<Agency>("agencije");

            var agencija = collection2.Find(Query.EQ("name", t));
            
            Accommodation smestaj = new Accommodation { location = lokacija, size = float.Parse(kvadratura), beds = Int32.Parse(brKreveta), available = dostupnost, internet = internet};
            foreach (Agency a in collection2.Find(Query.EQ("name", t)))
            {
                smestaj.agencija = new MongoDBRef("agencije", a.Id);
            }

            collection.Insert(smestaj);

            //---------------------------------------------------------------------------------------------------------------------
            //odavde na dole predstavlja problem
            //foreach (Accommodation a in collection.Find(Query.And(
            //                Query.EQ("size", smestaj.size),
            //                Query.EQ("location", smestaj.location)
            //                )))
            //{
            //    var query = Query.EQ("name", t);
            //    var update = MongoDB.Driver.Builders.Update.Set("Accommodations", BsonValue.Create(new List<MongoDBRef> { new MongoDBRef("listasmestaja", a.Id) }));
            //    collection2.Update(query, update);
            //}

            //----------------------------------------------------------------------------------------------------------------------

            MessageBox.Show("Smeštaj je dodat u agenciju.");
            //kada smo dodavali agencije mi smo dodavali kao da oni nemaju smsetaj,
            //posto smo sada dodali smestaj mi smo stavili referencu na agenciju, ali treba i da udjemo u agenciju i da update tu agenciju da ima taj i taj smesta
            //jel to ok?

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;

            button1.Enabled = false;
            
            listBox1.Items.Clear();

            foreach (Agency a in collection2.FindAll())
            {
                listBox1.Items.Add(a.name);
            }

            var collection1 = db.GetCollection<Accommodation>("listasmestaja");

            listBox3.Items.Clear();
            foreach (Accommodation a in collection1.FindAll())
            {
                listBox3.Items.Add(a.location + "," + a.size + "," + a.beds + "," + a.available + "," + a.internet);
            }

        }

        private void Smestaj_Load(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Agency>("agencije");
            
            foreach (Agency a in collection.FindAll())
            {
                listBox1.Items.Add(a.name);
            }

            var collection1 = db.GetCollection<Accommodation>("listasmestaja");

            foreach (Accommodation a in collection1.FindAll())
            {
                listBox3.Items.Add(a.location + "," + a.size + "," + a.beds + "," + a.available + ","+ a.internet);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Accommodation>("listasmestaja");

            string smestaj = textBox6.Text;

            var query = Query.EQ("location", smestaj);

            collection.Remove(query);

            var collection1 = db.GetCollection<Accommodation>("listasmestaja");

            listBox3.Items.Clear();
            foreach (Accommodation a in collection1.FindAll())
            {
                listBox3.Items.Add(a.location + "," + a.size + "," + a.beds + "," + a.available + "," + a.internet);
            }

            textBox6.Text = string.Empty;

            MessageBox.Show("Smeštaj je uspešno obrisan.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string t = listBox3.SelectedItem.ToString();

            string[] tPodeljen = t.Split(',');

            textBox1.Text = tPodeljen[0].ToString();
            textBox2.Text = tPodeljen[1].ToString();
            textBox3.Text = tPodeljen[2].ToString();
            textBox4.Text = tPodeljen[3].ToString();
            textBox5.Text = tPodeljen[4].ToString();

            textBox1.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            textBox2.Visible = false;

            button1.Enabled = false;            
            button2.Enabled = false;
            button4.Enabled = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Accommodation>("listasmestaja");

            string t = listBox3.SelectedItem.ToString();

            string[] tPodeljen = t.Split(',');

            string lokacija = tPodeljen[0].ToString();
            float kvadratura = float.Parse(tPodeljen[1].ToString());

            int brKreveta = Int32.Parse(textBox3.Text);
            string dostupnost = textBox4.Text;
            string internet = textBox5.Text;

            var query = Query.And(
                            Query.EQ("size", kvadratura),
                            Query.EQ("location", lokacija)
                            );
            var update = MongoDB.Driver.Builders.Update.Set("beds", brKreveta).Set("available", dostupnost).Set("internet", internet);

            collection.Update(query, update);


            textBox1.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            textBox2.Visible = true;
            button2.Enabled = true;
            button4.Enabled = false;
            button3.Enabled = false;

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;

            var collection1 = db.GetCollection<Accommodation>("listasmestaja");

            listBox3.Items.Clear();
            foreach (Accommodation a in collection1.FindAll())
            {
                listBox3.Items.Add(a.location + "," + a.size + "," + a.beds + "," + a.available + "," + a.internet);
            }

            MessageBox.Show("Promenjeno je.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
    }
}
