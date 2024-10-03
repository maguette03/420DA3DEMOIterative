using _420DA3DEMOIterative.Business.Domaine;
using _420DA3DEMOIterative.DataAccess.Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _420DA3DEMOIterative.Business.Services
{
    public class StudentService
    {
        private StudentDAO dao;

        public StudentService()
        {
            this.dao = new StudentDAO();
        }


        public List<Student> GetAllStudents()
        {
            return this.dao.GetAll();
        }

        public Student CreateStudent(string firstName, string lastName ,string code ,DateTime registrationDtae) 
        {
            Student student = new Student(firstName, lastName, code, registrationDtae); 

             return this.dao.Create(student);
        }
        public Student GetStudentById(int id) 
        {
            return this.dao.GetById(id);
        }

        public Student UpdateStudent(Student student) 
        {
            return this.dao.Update(student);
        }

        public void DeleteStudent(Student student) { 
        
            this.dao.Delete(student);

        }

    }
}
