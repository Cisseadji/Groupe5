using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionSystemeEtudiant;

namespace GestionSchoolApp.Forms
{
    public partial class FormProfesseur : Form
    {
        private AppDBContext _context = new AppDBContext();
        private Professeur _professeur;
        private ErrorProvider errorProviderProf = new ErrorProvider(); // Instance de l'ErrorProvider
        public FormProfesseur()
        {
            InitializeComponent();
        }

        private void FormProfesseur_Load(object sender, EventArgs e)
        {
            refresh();
            // Charger les matières disponibles
            ChargerMatieres();
            // Charger les classes disponibles
            ChargerClasses();

            // Si un cours existe déjà (pour la modification)
            if (_professeur != null)
            {
                foreach (var matiere in _professeur.matieres)
                {
                    // Sélectionner les matières associées au cours
                    cklistmatieres.SetItemChecked(cklistmatieres.Items.IndexOf(matiere.nomMatiere), true);
                }
            }

            // Si un cours existe déjà (pour la modification)
            if (_professeur != null)
            {
                foreach (var classe in _professeur.classes)
                {
                    // Sélectionner les matières associées au cours
                    cklistclasses.SetItemChecked(cklistclasses.Items.IndexOf(classe.nomClasse), true);
                }
            }
        }
        public void refresh()
        {

            dataGridView1.DataSource = null;
            using (var db = new AppDBContext())
            {
                dataGridView1.DataSource = db.Professeurs.ToList();
            }
        }
        private void ChargerMatieres()
        {
            // Charger les matières depuis la base de données et les afficher dans la CheckedListBox
            var matieres = _context.Matieres.ToList();
            cklistmatieres.Items.Clear();
            foreach (var matiere in matieres)
            {
                cklistmatieres.Items.Add(matiere.nomMatiere);
            }
        }
        private void ChargerClasses()
        {
            // Charger les matières depuis la base de données et les afficher dans la CheckedListBox
            var classes = _context.Classes.ToList();
            cklistclasses.Items.Clear();
            foreach (var classe in classes)
            {
                cklistclasses.Items.Add(classe.nomClasse);
            }
        }


        private bool ValiderChamps()
        {
            bool estValide = true;

            // Vérifier si le champ Nom est vide
            if (string.IsNullOrWhiteSpace(txtnom.Text))
            {
                errorProviderProf.SetError(txtnom, "Le nom du professeur est requis !");
                estValide = false;
            }
            else
            {
                errorProviderProf.SetError(txtnom, ""); // Effacer l'erreur si valide
            }

            // Vérifier si le champ Prénom est vide
            if (string.IsNullOrWhiteSpace(txtprenom.Text))
            {
                errorProviderProf.SetError(txtprenom, "Le prénom du professeur est requis !");
                estValide = false;
            }
            else
            {
                errorProviderProf.SetError(txtprenom, ""); // Effacer l'erreur si valide
            }

            // Vérifier si le champ Email est vide ou incorrect
            if (string.IsNullOrWhiteSpace(txtemail.Text) || !Regex.IsMatch(txtemail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorProviderProf.SetError(txtemail, "Entrer l'email et ça doit être valide !");
                estValide = false;
            }
            else
            {
                errorProviderProf.SetError(txtemail, ""); // Effacer l'erreur si valide
            }
            // Vérifier si le champ Téléphone est vide ou incorrect
            if (string.IsNullOrWhiteSpace(txttelephone.Text) || !Regex.IsMatch(txttelephone.Text, @"^(\+221\s?)?(77|78|70|76|75)\d{7}$"))
            {
                errorProviderProf.SetError(txttelephone, "Entrez un numéro de téléphone valide au format sénégalais !");
                estValide = false;
            }
            else
            {
                errorProviderProf.SetError(txttelephone, ""); // Effacer l'erreur si valide
            }


            // Vérifier si au moins une matière est sélectionnée dans la CheckedListBox
            if (cklistmatieres.CheckedItems.Count == 0)
            {
                errorProviderProf.SetError(cklistmatieres, "Veuillez sélectionner au moins une matière !");
                estValide = false;
            }
            else
            {
                errorProviderProf.SetError(cklistmatieres, ""); // Effacer l'erreur si valide
            }

            // Vérifier si au moins une classe est sélectionnée dans la CheckedListBox des classes
            if (cklistclasses.CheckedItems.Count == 0)
            {
                errorProviderProf.SetError(cklistclasses, "Veuillez sélectionner au moins une classe !");
                estValide = false;
            }
            else
            {
                errorProviderProf.SetError(cklistclasses, ""); // Effacer l'erreur si valide
            }

            return estValide;
        }

        private void btnregister_Click(object sender, EventArgs e)
        {
            // Vérifier la validité des champs avant de procéder
            if (!ValiderChamps())
            {
                return; // Si la validation échoue, on arrête l'exécution
            }

            try
            {
                // Créer une nouvelle instance de Professeur
                var professeur = new Professeur
                {
                    nom = txtnom.Text,
                    prenom = txtprenom.Text,
                    email = txtemail.Text,
                    tel = txttelephone.Text
                };

                // Associer les matières sélectionnées
                professeur.matieres = new List<Matiere>();
                foreach (var item in cklistmatieres.CheckedItems)
                {
                    var matiere = _context.Matieres.FirstOrDefault(m => m.nomMatiere == item.ToString());
                    if (matiere != null)
                    {
                        professeur.matieres.Add(matiere);
                    }
                }


                // Associer les classes sélectionnées
                professeur.classes = new List<Classe>();
                foreach (var item in cklistclasses.CheckedItems)
                {
                    var classe = _context.Classes.FirstOrDefault(c => c.nomClasse == item.ToString());
                    if (classe != null)
                    {
                        professeur.classes.Add(classe);
                    }
                }

                // Ajouter le professeur à la base de données
                _context.Professeurs.Add(professeur);
                _context.SaveChanges();
                refresh();

                // Afficher un message de succès
                MessageBox.Show("Professeur ajouté avec succès !");
               
            }
            catch (Exception ex)
            {
                // Capturer les exceptions et afficher un message d'erreur
                MessageBox.Show("Une erreur est survenue lors de l'ajout du professeur. Détails : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txttelephone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txttelephone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser uniquement les chiffres, l'espace, le tiret, et les signes "+" pour l'indicatif international
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '+' && e.KeyChar != ' ' && e.KeyChar != '-')
            {
                e.Handled = true; // Bloquer la touche si elle n'est pas un chiffre, un espace, un tiret ou un plus
            }
        }

        // Méthode pour charger un professeur à modifier
        private void ChargerProfesseur(int professeurId)
        {
            var professeur = _context.Professeurs.FirstOrDefault(p => p.id == professeurId);

            // Les matières seront automatiquement chargées quand vous y accédez
            var matieres = professeur.matieres;

            if (professeur != null)
            {
                txtnom.Text = professeur.nom;
                txtprenom.Text = professeur.prenom;
                txtemail.Text = professeur.email;
                txttelephone.Text = professeur.tel;

                // Cocher les matières associées
                foreach (var matiere in matieres)
                {
                    for (int i = 0; i < cklistmatieres.Items.Count; i++)
                    {
                        if (cklistmatieres.Items[i].ToString() == matiere.nomMatiere)
                        {
                            cklistmatieres.SetItemChecked(i, true);
                        }
                    }
                }

                var classes = professeur.classes;
                // Cocher les classes associées
                foreach (var classe in classes)
                {
                    for (int i = 0; i < cklistclasses.Items.Count; i++)
                    {
                        if (cklistclasses.Items[i].ToString() == classe.nomClasse)
                        {
                            cklistclasses.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            // Vérifier si les champs sont valides
            if (!ValiderChamps())
                return;

            // Trouver le professeur à modifier par son ID
            int professeurId = Convert.ToInt32(txtid.Text); // Assurez-vous que vous avez un champ pour l'ID
            var professeur = _context.Professeurs
                                     .FirstOrDefault(p => p.id == professeurId);

            if (professeur == null)
            {
                MessageBox.Show("Professeur non trouvé.");
                return;
            }

            // Mettre à jour les informations de base du professeur
            professeur.nom = txtnom.Text;
            professeur.prenom = txtprenom.Text;
            professeur.email = txtemail.Text;
            professeur.tel = txttelephone.Text;

            // Mettre à jour les matières associées
            professeur.matieres.Clear(); // Effacer les matières précédentes associées

            // Ajouter les matières sélectionnées
            foreach (var item in cklistmatieres.CheckedItems)
            {
                var matiere = _context.Matieres.FirstOrDefault(m => m.nomMatiere == item.ToString());
                if (matiere != null)
                {
                    professeur.matieres.Add(matiere);
                }
            }

            // Mettre à jour les classes associées
            professeur.classes.Clear(); // Effacer les classes précédentes associées

            // Ajouter les classes sélectionnées
            foreach (var item in cklistclasses.CheckedItems)
            {
                var classe = _context.Classes.FirstOrDefault(c => c.nomClasse == item.ToString());
                if (classe != null)
                {
                    professeur.classes.Add(classe);
                }
            }

            // Sauvegarder les modifications dans la base de données
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Professeur modifié avec succès !");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification du professeur : {ex.Message}");
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Récupérer l'ID de l'étudiant depuis la ligne double-cliquée
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);

                // Charger les informations de l'étudiant dans le formulaire
                ChargerProfesseur(id);
            }
        }
    }
}

