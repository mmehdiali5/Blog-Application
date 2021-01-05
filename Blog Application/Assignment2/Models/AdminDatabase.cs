/*This class is responsible for admin Database working like admin signup, login , add , delete, update users etc*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Assignment2.Models
{
    public class AdminDatabase
    {
        /*Check whether username already exist in DB*/
        public static bool isUsernameExist(string username)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from admins where username=@u";
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

        /*Function to add credtentials of admin in database if username is unique*/
        public static void addAdmin(CredentialsAdmin c)
        {

            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            string query = $"insert into admins(username,password) values(@n,@p)";
            SqlParameter p1 = new SqlParameter("n", c.username);
            SqlParameter p2 = new SqlParameter("p", c.password);

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();

            con.Close();

        }

        /*Function to authenticate admin*/
        public static bool authenticateAdmin(loginCredentials c)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from admins where username=@u and password=@p";
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
    }
}
