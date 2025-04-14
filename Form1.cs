using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjetBDD;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TP2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
        }
        private void LoadData()
        {
            ArticlesManager manager = new ArticlesManager();
            List<Article> articles = manager.Read(); // Lire les articles depuis la base de données
            dataGridView1.DataSource = articles; // Lier les données au DataGridView
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int currentRowIndex = dataGridView1.CurrentCell.RowIndex;

                if (currentRowIndex > 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[currentRowIndex - 1].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[currentRowIndex - 1].Cells[0];
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArticlesManager manager = new ArticlesManager();
            ajout ajoutForm = new ajout();
            if (ajoutForm.ShowDialog() == DialogResult.OK) 
            {
                try
                {
                    // Add the article to the database
                    manager.Add(ajoutForm.article);

                    // Reload the data to show the newly added article
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout de l'article : {ex.Message}");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();
            ArticlesManager manager = new ArticlesManager();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Veuillez entrer un mot-clé pour la recherche.");
                return;
            }

            try
            {
                // Call the Search method and update the DataGridView
                List<Article> filteredArticles = manager.Search(keyword);
                dataGridView1.DataSource = filteredArticles;

                if (filteredArticles.Count == 0)
                {
                    MessageBox.Show("Aucun article trouvé correspondant à votre recherche.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int currentRowIndex = dataGridView1.CurrentCell.RowIndex;

                if (currentRowIndex < dataGridView1.Rows.Count - 1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[currentRowIndex + 1].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[currentRowIndex + 1].Cells[0];
                }
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Vérifiez qu'une ligne est sélectionnée
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                // Mettez à jour les labels avec les données des colonnes correspondantes
                label1.Text = row.Cells["Code"].Value.ToString();
                label2.Text = row.Cells["Name"].Value.ToString();
                label3.Text = row.Cells["Description"].Value.ToString();
                label4.Text = row.Cells["Brand"].Value.ToString();
                label5.Text = row.Cells["Category"].Value.ToString();
                label6.Text = row.Cells["Price"].Value.ToString();  

                // Affichez l'image dans le PictureBox
                string photoPath = row.Cells["Photo"].Value.ToString();  // Chemin ou URL de la photo
                if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath))
                {
                    pictureBox1.Image = Image.FromFile(photoPath); // Charge l'image depuis le fichier
                }
                else
                {
                    pictureBox1.Image = null; // Si le chemin est vide ou invalide, ne rien afficher
                }
            }
            else
            {
                // Réinitialisez les labels et le PictureBox si aucune ligne n'est sélectionnée
                label2.Text = "";
                label3.Text = "";
                label4.Text = "";
                label5.Text = "";
                label6.Text = "";
                pictureBox1.Image = null;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                Article selectedArticle = new Article
                {
                    Id = (int)selectedRow.Cells["Id"].Value,
                    Code = selectedRow.Cells["Code"].Value.ToString(),
                    Name = selectedRow.Cells["Name"].Value.ToString(),
                    Description = selectedRow.Cells["Description"].Value.ToString(),
                    Brand = selectedRow.Cells["Brand"].Value.ToString(),
                    Category = selectedRow.Cells["Category"].Value.ToString(),
                    Price = (decimal)selectedRow.Cells["Price"].Value,
                    Photo = selectedRow.Cells["Photo"].Value.ToString()
                };
                
                update modifForm = new update(selectedArticle);
                ArticlesManager manager = new ArticlesManager();
                if (modifForm.ShowDialog() == DialogResult.OK)
                {
                    manager.Update(modifForm.article); // Update the article in the database
                    LoadData(); // Refresh the DataGridView
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un article à modifier.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un article à supprimer.");
                return;
            }

            // Get the selected row and retrieve the article ID
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int articleId = (int)selectedRow.Cells["Id"].Value;

            // Confirm the deletion
            var result = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cet article ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            ArticlesManager manager = new ArticlesManager();
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Call the manager's Delete method
                    manager.Delete(articleId);

                    // Reload the data to reflect changes
                    LoadData();
                    MessageBox.Show("L'article a été supprimé avec succès.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression : {ex.Message}");
                }
            }
        }
    }
}
