using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _420DA3DEMOIterative.Business.Domaine
{
    public class Course
    {

        public int Id { get; set; }
        public string NomCours { get; set; }
        public string CodeCours { get; set; }
        public int NbrHeure { get; set; }


        public Course(int id, string nomCours, string codeCours, int nbrHeure)
        {
            Id = id;
            NomCours = nomCours;
            CodeCours = codeCours;
            NbrHeure = nbrHeure;
        }
    }
}
