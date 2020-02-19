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
    public partial class Participanti : Form
    {
        public Participanti()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Participant>("participanti");

            string ime = textBox1.Text;
            string prezime = textBox2.Text;
            string brKarte = textBox4.Text;
            string grad = textBox5.Text;
            string telefon = textBox6.Text;

            Participant participant = new Participant { name = ime, lastName = prezime, cardNumber = brKarte, city = grad, telephone = telefon };

            collection.Insert(participant);

            var collection2 = db.GetCollection<Participant>("participanti");

            listBox1.Items.Clear();

            foreach (Participant p in collection2.FindAll())
            {
                listBox1.Items.Add(p.name + "," + p.lastName + "," + p.cardNumber + "," + p.city + "," + p.telephone);
            }

            MessageBox.Show("Dodat je novi participant.");

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Participant>("participanti");

            string karta = textBox3.Text;

            var query = Query.EQ("cardNumber", karta);

            collection.Remove(query);

            var collection2 = db.GetCollection<Participant>("participanti");

            listBox1.Items.Clear();

            foreach (Participant p in collection2.FindAll())
            {
                listBox1.Items.Add(p.name + "," + p.lastName + "," + p.cardNumber + "," + p.city + "," + p.telephone);
            }

            button2.Enabled = false;

            MessageBox.Show("Participant je uspešno obrisan.");
        }

        private void Participanti_Load(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Participant>("participanti");

            foreach (Participant p in collection.FindAll())
            {
                listBox1.Items.Add(p.name + "," + p.lastName + "," + p.cardNumber + "," + p.city + "," + p.telephone );
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                button4.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string t = listBox1.SelectedItem.ToString();

            string[] tPodeljen = t.Split(',');

            textBox1.Text = tPodeljen[0].ToString();
            textBox2.Text = tPodeljen[1].ToString();
            textBox4.Text = tPodeljen[2].ToString();
            textBox5.Text = tPodeljen[3].ToString();
            textBox6.Text = tPodeljen[4].ToString();

            button1.Enabled = false;
            button5.Enabled = true;
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox5.Visible = false;

            label1.Visible = false;
            label2.Visible = false;
            label5.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox5.Visible = true;

            label1.Visible = true;
            label2.Visible = true;
            label5.Visible = true;

            string t = listBox1.SelectedItem.ToString();

            string[] tPodeljen = t.Split(',');

            string staraKarta= tPodeljen[2].ToString();

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Participant>("participanti");

            string ime = textBox1.Text;
            string prezime = textBox2.Text;
            string grad = textBox5.Text;

            string novaKarta = textBox4.Text;
            string telefon = textBox6.Text;

            var query = Query.EQ("cardNumber", staraKarta);
            var update = MongoDB.Driver.Builders.Update.Set("cardNumber", novaKarta).Set("telephone", telefon);
            
            collection.Update(query, update);
            
            button5.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = false;
            button4.Enabled = false;

            textBox1.Visible = true;
            label1.Visible = true;

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;

            var collection3 = db.GetCollection<Participant>("participanti");

            listBox1.Items.Clear();

            foreach (Participant p in collection3.FindAll())
            {
                listBox1.Items.Add(p.name + "," + p.lastName + "," + p.cardNumber + "," + p.city + "," + p.telephone);
            }



            MessageBox.Show("Participant je izmenjen.");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
    }
}
