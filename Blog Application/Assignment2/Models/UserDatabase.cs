/*This class handles CRUD operations for Users*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Assignment2.Models
{
    public class UserDatabase
    {
        /*Check whether username already exist in DB*/
        public static bool isUsernameExist(string username)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from users where username=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("u", username);

            cmd.Parameters.Add(p1);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                con.Close();
                dr.Close();
                return true;
            }
            else
            {
                con.Close();
                dr.Close();
                return false;
            }
        }

        /*Check whether username already exist in DB more than once*/
        public static bool isUsernameMoreThanOnce(string username)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from users where username=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("u", username);

            cmd.Parameters.Add(p1);

            SqlDataReader dr = cmd.ExecuteReader();

            int count = 0;
            while (dr.Read())
            {
                count++;
            }
            if (count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*Function to add credtentials of user in database if username is unique*/
        public static void addUser(CredentialsUser c)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            string query = $"insert into users(username,password,email,imageName) values(@n,@p,@e,@i)";
            SqlParameter p1 = new SqlParameter("n", c.username);
            SqlParameter p2 = new SqlParameter("p", c.password);
            SqlParameter p3 = new SqlParameter("e", c.email);
            SqlParameter p4 = new SqlParameter("i", "default.jpg");


            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        /*Function to get Data of users from DB*/
        public static List<CredentialsUserForUpdate> getData()
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            List<CredentialsUserForUpdate> users = new List<CredentialsUserForUpdate>();
            string query = $"Select * from users";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CredentialsUserForUpdate user = new CredentialsUserForUpdate { ID = System.Convert.ToInt32(dr.GetValue(0)), username = (dr.GetValue(1)).ToString(), password = (dr.GetValue(2)).ToString(), email = (dr.GetValue(3)).ToString(),imageName= (dr.GetValue(4)).ToString() };
                users.Add(user);
            }
            return users;
        }

        /*Function to delete user from DB*/
        public static bool deleteUser(int id)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Delete from users where Id=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("id", id);
            cmd.Parameters.Add(p1);
            int deletedRows = cmd.ExecuteNonQuery();
            con.Close();
            if (deletedRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*Function to get user based on ID from DB*/
        public static CredentialsUserForUpdate getUser(int id)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from users where ID=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("id", id);

            cmd.Parameters.Add(p1);

            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();

            CredentialsUserForUpdate user = new CredentialsUserForUpdate { ID = System.Convert.ToInt32(dr.GetValue(0)), username = (dr.GetValue(1)).ToString(), password = (dr.GetValue(2)).ToString(), email = (dr.GetValue(3)).ToString(),imageName= (dr.GetValue(4)).ToString() };

            return user;
        }

        /*Function to update the user*/
       public static void updateUser(CredentialsUserForUpdate c)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Update users set username=@u,password=@p,email=@e,imageName=@image where Id=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("u", c.username);
            SqlParameter p2 = new SqlParameter("p", c.newPassword);
            SqlParameter p3 = new SqlParameter("e", c.email);
            SqlParameter p4 = new SqlParameter("image", c.imageName);
            SqlParameter p5 = new SqlParameter("id",c.ID);


            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            
            cmd.ExecuteNonQuery();
        }

        /*Function to return password n base of ID*/
       public static string getPassword(int id)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select password from users where ID=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("id", id);

            cmd.Parameters.Add(p1);

            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();
            return dr.GetValue(0).ToString();
        }

        /*Function to authenticate user*/
        public static bool authenticateUser(loginCredentials c)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from users where username=@u and password=@p";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("u", c.username);
            SqlParameter p2 = new SqlParameter("p", c.password);

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                con.Close();
                dr.Close();
                return true;
            }
            else
            {
                con.Close();
                dr.Close();
                return false;
            }
        }

        /*Check if username other than given id's username occur more than once*/
        public static bool isUsernameMoreThanOnce(string username ,int id)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from users where username=@u and ID!=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("u", username);
            SqlParameter p2 = new SqlParameter("id", id);

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                con.Close();
                dr.Close();
                return true;
            }
            else
            {
                con.Close();
                dr.Close();
                return false;
            }
        }

        /*get ID of given username*/
        public static int getId(string username)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from users where username=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("u", username);

            cmd.Parameters.Add(p1);

            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();
            return System.Convert.ToInt32(dr.GetValue(0));
        }

        public static CredentialsUserForUpdate getUserByName(string username)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from users where username=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("u", username);

            cmd.Parameters.Add(p1);

            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();
            
            CredentialsUserForUpdate user = new CredentialsUserForUpdate { ID = System.Convert.ToInt32(dr.GetValue(0)), username = (dr.GetValue(1)).ToString(), password = (dr.GetValue(2)).ToString(), email = (dr.GetValue(3)).ToString(), imageName = (dr.GetValue(4)).ToString() };
            return user;
        }

    }
}
