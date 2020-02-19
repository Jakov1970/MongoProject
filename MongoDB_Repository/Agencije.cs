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
    public partial class Agencije : Form
    {
        public Agencije()
        {
            InitializeComponent();
        }

        private void Agencije_Load(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Agency>("agencije");

            foreach (Agency a in collection.FindAll())
            {
                listBox1.Items.Add(a.name+","+a.city+","+a.telephone);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {            
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");
            
            var collection = db.GetCollection<Agency>("agencije");

            string ime = textBox1.Text;
            string grad = textBox2.Text;
            string telefon = textBox3.Text;

            Agency agencija1 = new Agency { name = ime, city = grad, telephone = telefon };

            collection.Insert(agencija1);

            var collection2 = db.GetCollection<Agency>("agencije");

            listBox1.Items.Clear();

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;

            foreach (Agency a in collection2.FindAll())
            {
                listBox1.Items.Add(a.name + "," + a.city + "," + a.telephone);
            }

            MessageBox.Show("Dodato");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Agency>("agencije");

            string agencijaIme = textBox4.Text;

            var query = Query.EQ("name", agencijaIme);

            collection.Remove(query);

            textBox4.Text = string.Empty;

            button2.Enabled = false;

            var collection2 = db.GetCollection<Agency>("agencije");

            listBox1.Items.Clear();

            foreach (Agency a in collection2.FindAll())
            {
                listBox1.Items.Add(a.name + "," + a.city + "," + a.telephone);
            }

            MessageBox.Show("Uspesno obrisano.");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
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
            textBox3.Text = tPodeljen[2].ToString();

            textBox1.Visible = false;
            label1.Visible = false;

            button1.Enabled = false;
            button5.Enabled = true;
            button2.Enabled = false;
            button4.Enabled = false;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("smestaj");

            var collection = db.GetCollection<Agency>("agencije");

            string ime = textBox1.Text;
            string grad = textBox2.Text;
            string telefon = textBox3.Text;

            var query = Query.EQ("name", ime);
            var update = MongoDB.Driver.Builders.Update.Set("city", grad).Set("telephone",telefon);
            
            collection.Update(query, update);

            var collection2 = db.GetCollection<Agency>("agencije");

            listBox1.Items.Clear();

            foreach (Agency a in collection2.FindAll())
            {
                listBox1.Items.Add(a.name + "," + a.city + "," + a.telephone);
            }

            button1.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;

            textBox1.Visible = true;
            label1.Visible = true;

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;

            MessageBox.Show("Agencija je izmenjena.");

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
