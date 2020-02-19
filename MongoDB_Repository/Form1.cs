using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB_Repository;

namespace NisvilleFindAccommodation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       

        private void button9_Click(object sender, EventArgs e)
        {
            Agencije a = new Agencije();
            a.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Smestaj s = new Smestaj();
            s.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Participanti p = new Participanti();
            p.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Accommodation>("listasmestaja");

            foreach (Accommodation a in collection.Find(Query.EQ("available", "da")))
            {
                listBox1.Items.Add(a.location + "," + a.size + "," + a.beds + "," + a.Id);
            }


            var collection2 = db.GetCollection<Participant>("participanti");

            foreach (Participant p in collection2.FindAll())
            {
                listBox2.Items.Add(p.name + "," + p.lastName + "," + p.cardNumber + "," + p.city + "," + p.Id);
            }


            var collection3 = db.GetCollection<Reservation>("rezervacije");


            //odavde pocinje
            foreach (Reservation a in collection3.FindAll())
            {
                var query1 = Query.EQ("_id", new BsonObjectId(a.participant.Id.ToString()));

                var query2 = Query.EQ("_id", new BsonObjectId(a.accommodation.Id.ToString()));



                foreach (Accommodation ac in collection.Find(query2))
                {
              //      var query3 = Query.EQ("_id", new BsonObjectId(ac.agencija.Id.ToString()));
                //    foreach (Agency agencija in collection.Find(query2))
                  //  {
                        foreach (Participant p in collection2.Find(query1))
                        {
                            listBox3.Items.Add(p.name + "," + p.lastName + "," + ac.location + "," + ac.beds + "," + a.Id);
                        }
                    //}
                }
            }



            //         List<string> listaZaIspis = new List<string>();

            // listBox3.Items.Add(p.name + "," + p.lastName + "," + p.cardNumber + "," + a.Id);
            //foreach (Reservation a in collection3.FindAll())
            //{
            //    var query1 = Query.EQ("_id", a.participant.Id);

            //    var query2 = Query.EQ("_id", a.accommodation.Id);

            //    foreach (Participant p in collection2.Find(query1))
            //    {
            //        listaZaIspis.Add(p.name);
            //        listaZaIspis.Add(p.lastName);
            //    }
            //    foreach (Accommodation ac in collection.Find(query2))
            //    {
            //        listaZaIspis.Add(ac.location);
            //    }
            //}
            //for (int i= 0; i < listaZaIspis.Count; i++)
            //{
            //    MessageBox.Show(listaZaIspis[i]);
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Accommodation>("listasmestaja");

            int kreveti = Int32.Parse(textBox1.Text);

            var query = Query.And(
                            Query.LTE("beds", kreveti),
                            Query.EQ("available", "da")
                            );

            listBox1.Items.Clear();
            foreach (Accommodation a in collection.Find(query))
            {
                listBox1.Items.Add(a.location + "," + a.size + "," + a.beds + "," + a.Id);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Reservation>("rezervacije");

            string soba = listBox1.SelectedItem.ToString();
            string participant = listBox2.SelectedItem.ToString();

            string[] tPodeljen = soba.Split(',');
            string[] tPodeljen1 = participant.Split(',');

            string sobaID = tPodeljen[3].ToString();
            string participantID = tPodeljen1[4].ToString();

            //MessageBox.Show(sobaID + "    i    " + participantBrKarte);

            var collection2 = db.GetCollection<Accommodation>("listasmestaja");



            Reservation rezervacija = new Reservation { accommodation = new MongoDBRef("listasmestaja", sobaID), participant = new MongoDBRef("participanti", participantID) };

            collection.Insert(rezervacija);

            var query = Query.EQ("_id", new BsonObjectId(sobaID));
            var update = MongoDB.Driver.Builders.Update.Set("available", "ne");

            collection2.Update(query, update);

            MessageBox.Show("Dodata rezervacija.");


            var collection3 = db.GetCollection<Reservation>("rezervacije");
            var collection4 = db.GetCollection<Participant>("participanti");
            var collection5 = db.GetCollection<Accommodation>("listasmestaja");
            //odavde pocinje
            listBox3.Items.Clear();
            foreach (Reservation a in collection3.FindAll())
            {
                var query1 = Query.EQ("_id", new BsonObjectId(a.participant.Id.ToString()));

                var query2 = Query.EQ("_id", new BsonObjectId(a.accommodation.Id.ToString()));

                foreach (Accommodation ac in collection5.Find(query2))
                {
                    foreach (Participant p in collection4.Find(query1))
                    {
                        listBox3.Items.Add(p.name + "," + p.lastName + "," + ac.location + "," + ac.beds + "," + a.Id);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Reservation>("rezervacije");

            string rezervacija = listBox3.SelectedItem.ToString();
            string[] tPodeljen = rezervacija.Split(',');
            string rezervacijaID = tPodeljen[4].ToString();

            var query = Query.EQ("_id", new BsonObjectId(rezervacijaID));

            collection.Remove(query);
            //-------------------------odavde----------------------------


            var collection2 = db.GetCollection<Accommodation>("listasmestaja");
            var query5 = Query.EQ("_id", new BsonObjectId(rezervacijaID));
            var update = MongoDB.Driver.Builders.Update.Set("available", "da");

            collection2.Update(query5, update);

            //-------------------------do ovde----------------------------
            MessageBox.Show("Rezervacija je uspešno obrisana.");

            var collection3 = db.GetCollection<Reservation>("rezervacije");
            var collection4 = db.GetCollection<Participant>("participanti");
            var collection5 = db.GetCollection<Accommodation>("listasmestaja");
            //odavde pocinje
            listBox3.Items.Clear();
            foreach (Reservation a in collection3.FindAll())
            {
                var query1 = Query.EQ("_id", new BsonObjectId(a.participant.Id.ToString()));

                var query2 = Query.EQ("_id", new BsonObjectId(a.accommodation.Id.ToString()));

                foreach (Accommodation ac in collection5.Find(query2))
                {
                    foreach (Participant p in collection4.Find(query1))
                    {
                        listBox3.Items.Add(p.name + "," + p.lastName + "," + ac.location + "," + ac.beds + "," + a.Id);
                    }
                }
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
