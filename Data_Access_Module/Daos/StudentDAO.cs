using _420DA3DEMOIterative.Business.Domaine;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Data_Access_Module.Daos
{
    internal class StudentDAO
    {
        private SqlConnection connection;
        private string tableName = "Students";
        private string datatimeDbFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

        public StudentDAO()
        {
            this.connection = DbConnectionProvider.GetConnection();
        }

        
        public List<Student> GetAll(SqlTransaction? transaction = null)
        {
            SqlConnection conn = transaction?.Connection ?? this.connection;
            SqlCommand getAllCommand = this.connection.CreateCommand();

            if (transaction != null)
            {
                getAllCommand.Transaction = transaction;
            }

            if (conn.State != System.Data.ConnectionState.Open) 
            {
                conn.Open();
            }


            SqlDataReader reader = getAllCommand.ExecuteReader();
            List<Student> etudiants = new List<Student>();
            while (reader.Read()) 
            {
                int db_id = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                string code = reader.GetString(3);
                DateTime registration = reader.GetDateTime(4);
                DateTime dateCreated = reader.GetDateTime(5);
                DateTime? dateModified = reader.GetValue(6) == DBNull.Value ? null : reader.GetDateTime(6);
                DateTime? dateDeleted = reader.GetValue(7) == DBNull.Value ? null : reader.GetDateTime(7);

                Student etudiant = new Student(db_id, firstName, lastName, code, registration, dateCreated, dateModified, dateDeleted);
                etudiants.Add(etudiant);
               
            }
            reader.Close();
            return etudiants;
        }
        
        public Student GetById(int id, SqlTransaction? transaction = null)
        {
            SqlConnection conn = transaction?.Connection?? this.connection;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM  {this.tableName} WHERE Id = @id;";




            SqlParameter idParam = cmd.CreateParameter();
            idParam.ParameterName = "@id";
            idParam.Value = id;
            idParam.DbType = DbType.Int32;
            cmd.Parameters.Add(idParam);

           

            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            SqlDataReader reader = cmd.ExecuteReader();



            if (!reader.Read()) 
            {
                throw new Exception($"No database record for Student Id #{id}.");

            }
            int db_id = reader.GetInt32(0);
            string firstName = reader.GetString(1);
            string lastName = reader.GetString(2);
            string code = reader.GetString(3);
            DateTime registration = reader.GetDateTime(4); 
            DateTime dateCreated = reader.GetDateTime(5);
            DateTime? dateModified = reader.GetValue(6) == DBNull.Value ? null : reader.GetDateTime(6);
            DateTime? dateDeleted = reader.GetValue(7) == DBNull.Value ? null : reader.GetDateTime(7);

            reader.Close();

            return new Student (db_id, firstName, lastName,code,registration,dateCreated,dateModified,dateDeleted);



        }

        public Student Create(Student etudiant ,SqlTransaction? transaction = null) 
        {
            SqlConnection conn = transaction?.Connection ?? this.connection;
            SqlCommand insertCommand = conn.CreateCommand();

             
            insertCommand.CommandText = $"INSERT INTO {this.tableName} (`FirstName`, `LastName`, `Code`, `RegistrationDate`) "
                + $"VALUES (@firstName, @lastName, @code, @registrationDate); SELECT SCOPE_IDENTITY();";



            SqlParameter firstNameParam = insertCommand.CreateParameter();
            firstNameParam.ParameterName = "@firstName";
            firstNameParam.Value = etudiant.FirstName;
            firstNameParam.DbType = DbType.String;
            insertCommand.Parameters.Add(firstNameParam);



            SqlParameter lastNameParam = insertCommand.CreateParameter();
            lastNameParam.ParameterName = "@lastName";
            lastNameParam.Value = etudiant.LastName;
            lastNameParam.DbType = DbType.String;
            insertCommand.Parameters.Add(lastNameParam);


            SqlParameter codeParam = insertCommand.CreateParameter();
            codeParam.ParameterName = "@code";
            codeParam.Value = etudiant.Code;
            codeParam.DbType = DbType.String;
            insertCommand.Parameters.Add(codeParam);


            SqlParameter registrationDate = insertCommand.CreateParameter();
            registrationDate.ParameterName = "@registrationDate";
            registrationDate.Value = etudiant.RegistrationDate;
            registrationDate.DbType = DbType.DateTime2;
            insertCommand.Parameters.Add(registrationDate);

            if (transaction != null)
            {
                insertCommand.Transaction = transaction;
            }


            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            int creationId = (int)insertCommand.ExecuteScalar();
            return this.GetById(creationId, transaction);
        }

        public Student Update(Student etudiant, SqlTransaction? transaction = null) 
        {

            SqlConnection conn = transaction?.Connection ?? this.connection;

            SqlCommand updateCommand = conn.CreateCommand();
            updateCommand.CommandText = $"UPDATE `{this.tableName}` " +
                $"SET FirstName = @firstName, "
                + "LastName = @lastName, "
                + "Code = @code, "
                + "RedistrationDate = @registrationDate "
                + "WHERE Id = @id ;";





            SqlParameter firstNameParam = updateCommand.CreateParameter();
            firstNameParam.ParameterName = "@firstName";
            firstNameParam.Value = etudiant.FirstName;
            firstNameParam.DbType = DbType.String;
            updateCommand.Parameters.Add(firstNameParam);



            SqlParameter lastNameParam = updateCommand.CreateParameter();
            lastNameParam.ParameterName = "@lastName";
            lastNameParam.Value = etudiant.LastName;
            lastNameParam.DbType = DbType.String;
            updateCommand.Parameters.Add(lastNameParam);


            SqlParameter codeParam = updateCommand.CreateParameter();
            codeParam.ParameterName = "@code";
            codeParam.Value = etudiant.Code;
            codeParam.DbType = DbType.String;
            updateCommand.Parameters.Add(codeParam);


            SqlParameter registrationDate = updateCommand.CreateParameter();
            registrationDate.ParameterName = "@registrationDate";
            registrationDate.Value = etudiant.RegistrationDate;
            registrationDate.DbType = DbType.DateTime2;
            updateCommand.Parameters.Add(registrationDate);


            SqlParameter idParam = updateCommand.CreateParameter();
            idParam.ParameterName = "@id";
            idParam.Value = etudiant.Id;
            idParam.DbType = DbType.Int32;
            updateCommand.Parameters.Add(idParam);


            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            int rowsModified = updateCommand.ExecuteNonQuery();
            if (rowsModified == 0)
            {
                throw new Exception($"Échec de la mise à jour de l'identifiant étudiant Id {etudiant.Id} Aucune ligne modifiée");
            }
            return this.GetById(etudiant.Id, transaction);
        }


        public void Delete(Student etudiant ,SqlTransaction? transaction = null) 
        {
            SqlConnection conn = transaction?.Connection ?? this.connection;
            //string sqlQuery = "DELETE FROM {0} WHERE Id ={1}";
            SqlCommand deleteCommand = this.connection.CreateCommand();
            deleteCommand.CommandText = $"DELETE FROM  {this.tableName} WHERE Id = @id;";
              
            SqlParameter idParam = deleteCommand.CreateParameter();
            idParam.ParameterName = "@id";
            idParam.Value = etudiant.Id;
            idParam.DbType = DbType.Int32;
            deleteCommand.Parameters.Add(idParam);

            if (transaction != null)
            {
                deleteCommand.Transaction = transaction;
            }


            if (conn.State != System.Data.ConnectionState.Open) 
            {
                conn.Open();
            }


            int rowsDelete = deleteCommand.ExecuteNonQuery();
            
            
            if(rowsDelete == 0)
            {
                throw new Exception($"Échec de la suppresion de l'identifiant étudiant Id {etudiant.Id} Aucune ligne supprimer");

            }
        }
    }
}
