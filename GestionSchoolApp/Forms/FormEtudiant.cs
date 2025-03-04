using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionSystemeEtudiant;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GestionSchoolApp.Forms
{
    public partial class FormEtudiant : Form
    {
        private AppDBContext _context = new AppDBContext();
        private ErrorProvider errorProviderEtudiant = new ErrorProvider();
        public FormEtudiant()
        {
            InitializeComponent();
            ChargerClasses(); // Charger les classes dans le ComboBox
        }

        private void FormEtudiant_Load(object sender, EventArgs e)
        {
            ReinitialiserChamps();
            ChargerClasses(); // Charger les classes dans le ComboBox
            refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbclasse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void refresh()
        {
            dataGridView1.DataSource = null;
            using (var db = new AppDBContext())
            {
                dataGridView1.DataSource = db.Etudiants.Select(e => new EtudiantView { id = e.id,matricule= e.matricule, prenom = e.prenom, nom = e.nom, classe = e.classe.nomClasse, sexe = e.sexe, adresse = e.adresse, datenaiss = e.datenaiss, email = e.email , tel = e.tel }).ToList();

            }
            
        }

        // Méthode de validation des champs
        private bool ValiderChamps()
        {
            bool estValide = true;

     

            // Vérifier si le champ Nom est vide
            if (string.IsNullOrWhiteSpace(txtnom.Text))
            {
                errorProviderEtudiant.SetError(txtnom, "Le nom est requis !");
                estValide = false;
            }
            else
            {
                errorProviderEtudiant.SetError(txtnom, ""); // Effacer l'erreur si valide
            }

            // Vérifier si le champ Prénom est vide
            if (string.IsNullOrWhiteSpace(txtprenom.Text))
            {
                errorProviderEtudiant.SetError(txtprenom, "Le prénom est requis !");
                estValide = false;
            }
            else
            {
                errorProviderEtudiant.SetError(txtprenom, ""); // Effacer l'erreur si valide
            }

            // Vérifier si le champ Adresse est vide
            if (string.IsNullOrWhiteSpace(txtadresse.Text))
            {
                errorProviderEtudiant.SetError(txtadresse, "Veuiller saisir une adresse !");
                estValide = false;
            }
            else
            {
                errorProviderEtudiant.SetError(txtadresse, ""); // Effacer l'erreur si valide
            }

            // Vérifier si le champ Email est valide
            if (string.IsNullOrWhiteSpace(txtemail.Text) || !Regex.IsMatch(txtemail.Text, @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$"))
            {
                errorProviderEtudiant.SetError(txtemail, "L'email est requis et doit être valide !");
                estValide = false;
            }
            else
            {
                errorProviderEtudiant.SetError(txtemail, ""); // Effacer l'erreur si valide
            }

            // Vérifier si le champ Téléphone est valide
            if (string.IsNullOrWhiteSpace(txttelephone.Text) || !Regex.IsMatch(txttelephone.Text, @"^\+?[0-9\s\(\)-]+$"))
            {
                errorProviderEtudiant.SetError(txttelephone, "Le téléphone est requis et doit être valide !");
                estValide = false;
            }
            else
            {
                errorProviderEtudiant.SetError(txttelephone, ""); // Effacer l'erreur si valide
            }

            // Vérifier si un sexe est sélectionné
            if (!rdhomme.Checked && !rdfemme.Checked)
            {
                errorProviderEtudiant.SetError(panelsexe, "Veuillez sélectionner une option !");
                estValide = false;
            }
            else
            {
                errorProviderEtudiant.SetError(panelsexe, ""); // Effacer l'erreur si valide
            }

            // Vérifier si la classe a été sélectionnée
            if (cmbclasse.SelectedIndex == 0)
            {
                errorProviderEtudiant.SetError(cmbclasse, "La classe est requise !");
                estValide = false;
            }
            else
            {
                errorProviderEtudiant.SetError(cmbclasse, ""); // Effacer l'erreur si valide
            }

            return estValide;
        }

        // Réinitialiser les champs après ajout
        private void ReinitialiserChamps()
        {
            txtmatricule.Clear();
            txtnom.Clear();
            txtprenom.Clear();
            dtpdatenaiss.Value = DateTime.Now;
            rdhomme.Checked = false;
            rdfemme.Checked = false;
            txtadresse.Clear();
            txttelephone.Clear();
            txtemail.Clear();
            cmbclasse.Text = "choisir une classe";
            cmbclasse.SelectedIndex = -1;
        }

        private void ChargerClasses()
        {
            var classes = _context.Classes.ToList();
            cmbclasse.DataSource = classes;
            cmbclasse.DisplayMember = "nomClasse"; // Afficher le nom de la classe dans le ComboBox
            cmbclasse.ValueMember = "id"; // Id de la classe
        }

        private void dtnadd_Click(object sender, EventArgs e)
        {
            // Validation des champs
        if (!ValiderChamps())
                return;

            // Créer un nouvel étudiant
            var etudiant = new Etudiant
            {
                nom = txtnom.Text,
                prenom = txtprenom.Text,
                datenaiss = dtpdatenaiss.Value,
                sexe = rdhomme.Checked ? "Homme" : "Femme", // Assurez-vous que l'un des boutons radio est sélectionné
                adresse = txtadresse.Text,
                tel = txttelephone.Text,
                email = txtemail.Text,
                idC = (int)cmbclasse.SelectedValue,
                classe = (Classe)cmbclasse.SelectedItem
            };

            // Récupérer la classe sélectionnée
            var classe = _context.Classes.FirstOrDefault(c => c.id == (int)cmbclasse.SelectedValue);

            if (classe != null)
            {
                etudiant.classe = classe;
            }
            else
            {
                MessageBox.Show("Classe non trouvée !");
                return;
            }

            // Ajouter l'étudiant à la base de données
            try
            {
                _context.Etudiants.Add(etudiant);
                _context.SaveChanges();
                string matriculeGenere = $"{etudiant.id}-{etudiant.nom}-{etudiant.datenaiss:yyyyMMdd}";

                etudiant.matricule= matriculeGenere;
                _context.SaveChanges();
                refresh();
                MessageBox.Show($"Étudiant ajouté avec succès !\nMatricule : {matriculeGenere}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReinitialiserChamps();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de l'étudiant : {ex.Message}");
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        // Réinitialiser les champs après ajout


    }
}
