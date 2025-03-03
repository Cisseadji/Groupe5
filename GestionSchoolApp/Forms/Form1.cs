using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionSchoolApp.Forms;

namespace GestionSchoolApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void etudiantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEtudiant formEtudiant = new FormEtudiant();
            formEtudiant.Show();
            formEtudiant.MdiParent = this;
        }

        private void classesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClasse formClasse = new FormClasse();
            formClasse.Show();
            formClasse.MdiParent = this;
        }

        private void coursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCours formCours = new FormCours();
            formCours.Show();
            formCours.MdiParent = this;
        }

        private void matièresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMatiere formMatiere = new FormMatiere();
            formMatiere.Show();
            formMatiere.MdiParent = this;
        }

        private void professeursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProfesseur formProfesseur = new FormProfesseur();
            formProfesseur.Show();
            formProfesseur.MdiParent = this;
        }

        private void notesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNote formNote = new FormNote();
            formNote.Show();
            formNote.MdiParent = this;
        }

        private void utilisateursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtilisateur formUtilisateur = new FormUtilisateur();
            formUtilisateur.Show();
            formUtilisateur.MdiParent = this;
        }

        private void matièresToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormMatiere formMatiere = new FormMatiere();
            formMatiere.Show();
            formMatiere.MdiParent = this;
        }
    }
}
