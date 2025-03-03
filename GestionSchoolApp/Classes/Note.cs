using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionSystemeEtudiant
{
    internal class Note
    {
       public int id {  get; set; }
        public int idE {  get; set; }
        public Etudiant etudiant { get; set; }
        public int idM { get; set; }      
        public Matiere matiere { get; set; }
        public float note { get; set; }

    }
}
