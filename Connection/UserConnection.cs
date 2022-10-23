using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestCrud.Models;

namespace TestCrud.Connection
{
    public class UserConnection
    {
        //DB connection
        DBConnection Connection = new DBConnection();

        //Message cods
        public const int SUCCESSFUL = 0;
        public const int ERROR_EXISTING_USER = 1;
        public const long ERROR_NON_EXISTENT_USER = -1;
        public const int ERROR_INCOMPLETE_DATA = 3;
        public const int ERROR_EXECUTION = -2;

        //Método que crea Usuarios en la tabla Users
        public  int Create(Users user)
        {


            int ret = SUCCESSFUL;
            SqlCommand cmd = null;
            SqlConnection connection = null;
            try
            {

                using (connection = Connection.Connection)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CreateUser";
                    cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar, 50));
                    cmd.Parameters.Add(new SqlParameter("@password", System.Data.SqlDbType.VarChar, 50));
                    cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 50));
                    cmd.Parameters.Add(new SqlParameter("@profile", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@codReturn", System.Data.SqlDbType.Int)); cmd.Parameters["@codReturn"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@email"].Value = user.Email;
                    cmd.Parameters["@name"].Value = user.Name;
                    cmd.Parameters["@password"].Value = user.Password;
                    cmd.Parameters["@profile"].Value = user.Profile;

                    cmd.ExecuteNonQuery();

                    ret = Convert.ToInt32(cmd.Parameters["@codReturn"].Value);

                }
            }
            catch (Exception ex)
            {

                ret = ERROR_EXECUTION;
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return ret;
        }
        //Método que cambia el estado de los Usuarios en activo/inactivo

        public int ChangeState(long code)
        {

            int ret = SUCCESSFUL;
            SqlCommand cmd = null;
            SqlConnection connection = null;
            try
            {

                using (connection = Connection.Connection)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "ChangeState";
                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));

                    cmd.Parameters.Add(new SqlParameter("@codReturn", System.Data.SqlDbType.Int)); cmd.Parameters["@codReturn"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@id"].Value = code;

                    cmd.ExecuteNonQuery();

                    ret = Convert.ToInt32(cmd.Parameters["@codReturn"].Value);

                }
            }
            catch (Exception ex)
            {

                ret = ERROR_EXECUTION;
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return ret;
        }
        //Método que edita un Usuarios

        public int UpdateUser(Users user)
        {

            int ret = SUCCESSFUL;
            SqlCommand cmd = null;
            SqlConnection connection = null;
            try
            {

                using (connection = Connection.Connection)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateUser";
                    cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar, 50));
                    cmd.Parameters.Add(new SqlParameter("@password", System.Data.SqlDbType.VarChar, 50));
                    cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 50));
                    cmd.Parameters.Add(new SqlParameter("@profile", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));

                    cmd.Parameters.Add(new SqlParameter("@codReturn", System.Data.SqlDbType.Int)); cmd.Parameters["@codReturn"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@email"].Value = user.Email;
                    cmd.Parameters["@name"].Value = user.Name;
                    cmd.Parameters["@password"].Value = user.Password;
                    cmd.Parameters["@profile"].Value = user.Profile;
                    cmd.Parameters["@id"].Value = user.Id;

                    cmd.ExecuteNonQuery();

                    ret = Convert.ToInt32(cmd.Parameters["@codReturn"].Value);

                }
            }
            catch (Exception ex)
            {

                ret = ERROR_EXECUTION;
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return ret;
        }
        //Método que trae un Usuarios por ID

        public long GetUserId(Users user)
        {
            long ret = ERROR_NON_EXISTENT_USER;
            using (SqlConnection connection = Connection.Connection)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT TOP 1 id FROM Users WHERE UPPER('"+user.Email+"') = UPPER(Email) AND UPPER('"+user.Password+"') = UPPER(Password) AND "+user.Profile+" = Profile AND isActive = 1";
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ret = reader.GetInt64(0);
                }
                reader.Close();
            }
            return ret;
        }

        //Método que trae la lista de todos los Usuarios
        public List<Users> GetUserList()
        {
            List<Users> ret = new List<Users>();
            using (SqlConnection connection = Connection.Connection)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users ";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Users user = new Users();
                    user.Id = reader.GetInt64(0);
                    user.Name = reader.GetString(2);
                    user.Email = reader.GetString(3);
                    user.Profile = reader.GetInt32(4);
                    user.CreateDate = reader.GetDateTime(5);
                    user.IsActive = reader.GetBoolean(7);
                    ret.Add(user);
                }
                reader.Close();
            }
            return ret;
        }
        public Users GetUserById(long id)
        {

            Users ret = new Users();
            using (SqlConnection connection = Connection.Connection)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT id,name,email FROM Users WHERE id = "+id;
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ret.Id = reader.GetInt64(0);
                    ret.Name = reader.GetString(1);
                    ret.Email = reader.GetString(2);
                }
                reader.Close();
            }
            return ret;
        }
        //Método que trae la lista de tipos de Usuarios

        public List<UsersProfiles> GetProfiles()
        {
            var listProfiles = new List<UsersProfiles>();

            using (SqlConnection connection = Connection.Connection)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM USERSPROFILES";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var newProfile = new UsersProfiles();
                    newProfile.Usersid = reader.GetInt32(0);
                    newProfile.Description = reader.GetString(1);
                    listProfiles.Add(newProfile);
                }
                reader.Close();
            }
            return listProfiles;
        }
    }
}
