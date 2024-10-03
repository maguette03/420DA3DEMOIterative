using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _420DA3DEMOIterative.Business.Domaine
{
    public class Student
    {
        //pour FirstName
        public const int MaxFirstnameLength = 64;
        private string firstName;

        //pour LastName
        public const int MaxLastNameLength = 64;
        private string lastName;

        //pour le code
        public const int MaxCodeLength = 12;
        private string code;


        public int Id { get; set; }
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (value.Length > MaxFirstnameLength)
                {
                    throw new Exception($"Maximun length of Student.FirstName is {MaxFirstnameLength} characters.");
                }
                firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (value.Length > MaxLastNameLength)
                {
                    throw new Exception($"Maximun length of Student lastName is {MaxLastNameLength} characteres ");
                }
                lastName = value;
            }
        }
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                if (value.Length < MaxCodeLength)
                {
                    throw new Exception($"Maximum length of Student Code is {MaxCodeLength} charactere");
                }
                code = value;
            }
        }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }


        public Student(string firstName, string lastName, string code, DateTime registrationDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Code = code;
            RegistrationDate = registrationDate;
        }

        public Student(int id, string firstName, string lastName, string code,
            DateTime registrationDate, DateTime dateCreated, DateTime? dateModified,
            DateTime? dateDeleted)
            : this(firstName, lastName, code, registrationDate)
        {
            Id = Id;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;

        }


    }
}
