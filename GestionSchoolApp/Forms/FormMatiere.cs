using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionSystemeEtudiant;

namespace GestionSchoolApp.Forms
{
    public partial class FormMatiere : Form
    {

        private AppDBContext _context = new AppDBContext();
        private ErrorProvider errorProvider = new ErrorProvider();
        public FormMatiere()
        {
            InitializeComponent();
            refresh();
        }


        private bool ValiderChamps()
        {
            bool estValide = true;

            // Vérifier si le champ NomMatiere est vide
            if (string.IsNullOrWhiteSpace(txtnom.Text))
            {
                errorProvider.SetError(txtnom, "Le nom de la matière est requis !");
                estValide = false;
            }
            else
            {
                errorProvider.SetError(txtnom, ""); // Effacer l'erreur si valide
            }


            return estValide;
        }
        
        public void refresh()
        {

            dataGridView1.DataSource = null;
            using (var db = new AppDBContext())
            {
                dataGridView1.DataSource = db.Matieres.ToList();
            }
        }

        private void FormMatiere_Load(object sender, EventArgs e)
        {
            refresh();
            
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (!ValiderChamps()) return;
            try
            {
                // Créer une nouvelle instance de la matière
                var matiere = new Matiere
                {
                    nomMatiere = txtnom.Text
                };

                // Vérifier que le nom de la matière n'est pas vide
                if (string.IsNullOrWhiteSpace(matiere.nomMatiere))
                {
                    MessageBox.Show("Le nom de la matière ne peut pas être vide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Ajouter la matière à la base de données
                _context.Matieres.Add(matiere);
                _context.SaveChanges();
                refresh();

                // Message de succès
                MessageBox.Show("Matière ajoutée avec succès !");
            }
            catch (DbUpdateException dbEx)
            {
                // Exception spécifique liée à la base de données
                MessageBox.Show("Erreur lors de l'ajout de la matière dans la base de données. Détails : " + dbEx.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Exception générale
                MessageBox.Show("Une erreur s'est produite. Détails : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
