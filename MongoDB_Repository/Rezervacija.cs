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
    public partial class Rezervacija : Form
    {
        public Rezervacija()
        {
            InitializeComponent();
        }

        private void Rezervacija_Load(object sender, EventArgs e)
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

            foreach (Reservation a in collection3.FindAll())
            {            
                foreach (Participant p in collection2.FindAll())
                {
                    listBox3.Items.Add(p.name + "," + p.lastName + "," + p.cardNumber + "," + a.Id);
                }                
            }


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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Reservation>("rezervacije");

            string smestaj = textBox2.Text;

            var query = Query.EQ("_id", new BsonObjectId(smestaj));

            collection.Remove(query);

            MessageBox.Show("Rezervacija je uspešno obrisana.");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
