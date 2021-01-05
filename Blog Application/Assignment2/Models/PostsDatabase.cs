using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Assignment2.Models
{
    public class PostsDatabase
    {
        /*get data of all posts from DB*/
        public static List<posts> getPosts()
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            List<posts> postsInfo = new List<posts>();
            string query = $"Select * from posts ORDER BY date DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                posts post = new posts { ID = System.Convert.ToInt32(dr.GetValue(0)), date = System.Convert.ToDateTime((dr.GetValue(1)).ToString()), title = (dr.GetValue(2)).ToString(),content= (dr.GetValue(3)).ToString(),userId= System.Convert.ToInt32((dr.GetValue(4)).ToString()) };
                postsInfo.Add(post);
            }
            return postsInfo;
        }

        /*Add post to DB*/
        public static void addPost(posts p,string username,int userId,string userImage)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            string query = $"insert into posts(date,title,content,userId) values(@d,@t,@c,@userId)";
            SqlParameter p1 = new SqlParameter("d", DateTime.Now);
            SqlParameter p2 = new SqlParameter("t", p.title);
            SqlParameter p3 = new SqlParameter("c", p.content);
            SqlParameter p4 = new SqlParameter("userId",userId);
           

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
          

            cmd.ExecuteNonQuery();

            con.Close();
        }

        /*Get Post from DB by Id*/
        public static posts getPostById(int id)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from posts where ID=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("id", id);

            cmd.Parameters.Add(p1);

            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();

            posts post = new posts { ID = System.Convert.ToInt32(dr.GetValue(0)), date = System.Convert.ToDateTime((dr.GetValue(1)).ToString()), title = (dr.GetValue(2)).ToString(), content = (dr.GetValue(3)).ToString(), userId = System.Convert.ToInt32((dr.GetValue(4)).ToString()) };

            return post;
        }

        /*Delete all posts of given userId Actually we will delete all posts of deleted user*/
        public static void deletePostsOfUser(int userId)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Delete from posts where userId=@id";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("id", userId);

            cmd.Parameters.Add(p1);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        /*Check whether post is posted by LoggedInUser by checking condition 
         ID=postId and userId=loggedInUserId*/
        public static bool isPostByLoggedInUser(int postId,int loggedInUserId)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Select * from posts where ID=@id and userId=@userId";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("id", postId);
            SqlParameter p2 = new SqlParameter("userId", loggedInUserId);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);


            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*Delete Post By Id*/
        public static void deletePostFromDB(int id)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Delete from posts where Id=@id";
            
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("id", id);
            
            cmd.Parameters.Add(p1);
            
            cmd.ExecuteNonQuery();
            
            con.Close();
           
        }

        /*Update post's title, content and date(with current date) with receiving post*/
        public static void updatePost(posts post)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Assignment2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string query = $"Update posts set title=@t,content=@c,date=@d where Id=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("t", post.title);
            SqlParameter p2 = new SqlParameter("c", post.content);
            SqlParameter p3 = new SqlParameter("d", DateTime.Now);
            SqlParameter p4 = new SqlParameter("id", post.ID);

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);

            cmd.ExecuteNonQuery();
        }

    }
}
