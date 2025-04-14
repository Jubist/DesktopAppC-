using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP2
{
    public partial class update : Form
    {
        public Article article { get; private set; }
        public update(Article articl)
        {
            InitializeComponent();
            article = articl;
            textBox1.Text = article.Code;
            textBox2.Text = article.Name;
            textBox3.Text = article.Description;
            textBox4.Text = article.Brand;
            textBox5.Text = article.Category;
            textBox6.Text = article.Price.ToString();
            pictureBox1.ImageLocation = article.Photo;
        }
        
        private void update_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Veuillez remplir les champs obligatoires (Code, Nom, Prix).");
                return;
            }
            if (!decimal.TryParse(textBox6.Text, out decimal price))
            {
                MessageBox.Show("Veuillez entrer un prix valide.");
                return;
            }
            // Update the article object
            article.Code = textBox1.Text;
            article.Name = textBox2.Text;
            article.Description = textBox3.Text;
            article.Brand = textBox4.Text;
            article.Category = textBox5.Text;
            article.Price = decimal.Parse(textBox6.Text);
            article.Photo = pictureBox1.ImageLocation;

            // Indicate success and close the form
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Fichiers image (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
