using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionSystemeEtudiant;

namespace GestionSchoolApp.Forms
{
    public partial class FormClasse : Form
    {
        private AppDBContext _context = new AppDBContext();
        private ErrorProvider errorProviderClasse = new ErrorProvider();
        public FormClasse()
        {
            InitializeComponent();
            refresh();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        public void refresh()
        {

            dataGridView1.DataSource = null;
            using (var db = new AppDBContext())
            {
                dataGridView1.DataSource = db.Classes.ToList();
            }
        }

        private bool ValiderChamps()
        {
            bool estValide = true;

            // Vérifier si le champ NomClasse est vide
            if (string.IsNullOrWhiteSpace(txtnomclasse.Text))
            {
                errorProviderClasse.SetError(txtnomclasse, "Le nom de la classe est requis !");
                estValide = false;
            }
            else
            {
                errorProviderClasse.SetError(txtnomclasse, ""); // Effacer l'erreur si valide
            }

            // Vérifier si la classe existe déjà
            if (_context.Classes.Any(c => c.nomClasse.ToLower() == txtnomclasse.Text.ToLower()))
            {
                errorProviderClasse.SetError(txtnomclasse, "Cette classe existe déjà !");
                estValide = false;
            }

            return estValide;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (!ValiderChamps()) return; // Vérifier les erreurs avant d'ajouter

            try
            {
                var nouvelleClasse = new Classe
                {
                    nomClasse = txtnomclasse.Text.Trim()
                };

                _context.Classes.Add(nouvelleClasse);
                _context.SaveChanges();

                refresh();
                txtnomclasse.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnassocier_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

