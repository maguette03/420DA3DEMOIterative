using _420DA3DEMOIterative.Business.Domaine;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _420DA3DEMOIterative.DataAccess.Daos
{
    public class CourseDAO
    {

        private SqlConnection connection;
        private string tableName = "Course";

        public CourseDAO()
        {
            connection = DbConnectionProvider.GetConnection();
        }

        public Course GetById(int id)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM  {tableName} WHERE Id = {id};";

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                throw new Exception($"No database record for Course Id #{id}.");

            }
            int id_db = reader.GetInt32(0);
            string nomCours = reader.GetString(1);
            string codeCours = reader.GetString(2);
            int nbrHeure = reader.GetInt32(3);

            reader.Close();

            return new Course(id_db, nomCours, codeCours, nbrHeure);
        }

        public Course Create(Course cours)
        {
            string sqlQuery = "INSERT INTO {0} (NomCours, CodeCours, NbrHeure) "
                + "VALUE( '{1}','{2}','{3}'); SELECT SCOPE_IDENTITY();";
            SqlCommand insertCommand = connection.CreateCommand();
            insertCommand.CommandText = string.Format(sqlQuery, tableName, cours.NomCours, cours.CodeCours, cours.NbrHeure);

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            int creationId = (int)insertCommand.ExecuteScalar();
            return GetById(creationId);
        }

        public Course Update(Course cours)
        {
            string sqlQuery = "UPDATE {0} SET NomCours = '{1}' , CodeCours = '{2}' , NbrHeure = '{3}' WHERE Id = {4}";
            SqlCommand updateCommand = connection.CreateCommand();
            updateCommand.CommandText = string.Format(sqlQuery, tableName, cours.NomCours,
                cours.CodeCours, cours.NbrHeure, cours.Id);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            int rowsModified = updateCommand.ExecuteNonQuery();
            if (rowsModified == 0)
            {
                throw new Exception($"Échec de la mise à jour de l'identifiant cours Id {cours.Id} Aucune ligne modifiée");
            }

            return GetById(cours.Id);

        }

        public Course Delete(Course cours)
        {

            string sqlQuery = "DELETE FROM {0} WHERE Id ={1}";
            SqlCommand deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = string.Format(sqlQuery, tableName, cours.Id);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            int rowsDelete = deleteCommand.ExecuteNonQuery();
            if (rowsDelete == 0)
            {
                throw new Exception($"Échec de la suppresion de l'identifiant étudiant Id {cours.Id} Aucune ligne supprimer");

            }
            return GetById(cours.Id);
        }

    }
}
