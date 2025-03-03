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
    public partial class FormCours : Form
    {
        private AppDBContext _context = new AppDBContext();
        private Cours _cours;
        private ErrorProvider errorProviderCours = new ErrorProvider(); // Instance de l'ErrorProvider
        public FormCours()
        {
            InitializeComponent();
            refresh();
        }


        private bool ValiderChamps()
        {
            bool estValide = true;

            // Vérifier si le champ NomCours est vide
            if (string.IsNullOrWhiteSpace(txtnomcours.Text))
            {
                errorProviderCours.SetError(txtnomcours, "Le nom du cours est requis !");
                estValide = false;
            }
            else
            {
                errorProviderCours.SetError(txtnomcours, ""); // Effacer l'erreur si valide
            }

            // Vérifier si le champ Description est vide
            if (string.IsNullOrWhiteSpace(txtdescription.Text))
            {
                errorProviderCours.SetError(txtdescription, "La description est requise !");
                estValide = false;
            }
            else
            {
                errorProviderCours.SetError(txtdescription, "");
            }

            // Vérifier si le cours existe déjà
            if (_context.Cours.Any(c => c.nomCours.ToLower() == txtnomcours.Text.ToLower()))
            {
                errorProviderCours.SetError(txtnomcours, "Ce cours existe déjà !");
                estValide = false;
            }

            // Vérifier si au moins une matière est sélectionnée dans la CheckedListBox
            if (checkedListBoxMatieres.CheckedItems.Count == 0)
            {
                errorProviderCours.SetError(checkedListBoxMatieres, "Veuillez sélectionner au moins une matière !");
                estValide = false;
            }
            else
            {
                errorProviderCours.SetError(checkedListBoxMatieres, ""); // Effacer l'erreur si valide
            }

            return estValide;
        }

        public void refresh()
        {

            dataGridView1.DataSource = null;
            using (var db = new AppDBContext())
            {
                dataGridView1.DataSource = db.Cours.ToList();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Si la validation échoue, on arrête l'exécution
            if (!ValiderChamps()) return;

            try
            {
                // Si un cours existe déjà, on le modifie, sinon on en crée un nouveau
                if (_cours == null)
                {
                    _cours = new Cours();
                }

                _cours.nomCours = txtnomcours.Text;
                _cours.description = txtdescription.Text;

                // Gérer les matières associées au cours
                _cours.matieres = new List<Matiere>();

                foreach (var item in checkedListBoxMatieres.CheckedItems)
                {
                    var matiere = _context.Matieres.FirstOrDefault(m => m.nomMatiere == item.ToString());
                    if (matiere != null)
                    {
                        _cours.matieres.Add(matiere);
                    }
                }

                // Ajouter ou mettre à jour le cours dans la base de données
                if (_cours.id == 0)
                {
                    _context.Cours.Add(_cours);
                }
                

                // Sauvegarder les changements dans la base de données
                _context.SaveChanges();
                refresh();

                // Message de succès
                MessageBox.Show("Cours sauvegardé avec succès !");
                
            }
            catch (DbUpdateException dbEx)
            {
                // Exception liée à la base de données (problème lors de la mise à jour ou de l'ajout)
                MessageBox.Show("Erreur lors de la mise à jour du cours dans la base de données. Détails : " + dbEx.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Exception générale
                MessageBox.Show("Une erreur s'est produite. Détails : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ChargerMatieres()
        {
            // Charger les matières depuis la base de données et les afficher dans la CheckedListBox
            var matieres = _context.Matieres.ToList();
            checkedListBoxMatieres.Items.Clear();
            foreach (var matiere in matieres)
            {
                checkedListBoxMatieres.Items.Add(matiere.nomMatiere);
            }
        }
        private void FormCours_Load(object sender, EventArgs e)
        {
            refresh();
            // Charger les matières disponibles
            ChargerMatieres();

            // Si un cours existe déjà (pour la modification)
            if (_cours != null)
            {
                txtnomcours.Text = _cours.nomCours;
                foreach (var matiere in _cours.matieres)
                {
                    // Sélectionner les matières associées au cours
                    checkedListBoxMatieres.SetItemChecked(checkedListBoxMatieres.Items.IndexOf(matiere.nomMatiere), true);
                }
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
