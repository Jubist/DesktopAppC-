using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TP2;

namespace ProjetBDD
{
    public class ArticlesManager
    {
        public string connectionString= "Data Source=PC404-02;Initial Catalog=ProjetBDD;Integrated Security=True";

        public ArticlesManager()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=ProjetBDD;Integrated Security=True"; //recuperer affichage>explorateur de serveurs
            conn.Open();
        }

        public void Add(Article article)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Articles (Code, Name, Description, Brand, Category, Price, Photo) VALUES (@Code, @Name, @Description, @Brand, @Category, @Price, @Photo)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Code", article.Code);
                cmd.Parameters.AddWithValue("@Name", article.Name);
                cmd.Parameters.AddWithValue("@Description", article.Description);
                cmd.Parameters.AddWithValue("@Brand", article.Brand);
                cmd.Parameters.AddWithValue("@Category", article.Category);
                cmd.Parameters.AddWithValue("@Price", article.Price);
                cmd.Parameters.AddWithValue("@Photo", string.IsNullOrEmpty(article.Photo) ? (object)DBNull.Value : article.Photo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Article> Read()
        {
            List<Article> articles = new List<Article>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=ProjetBDD;Integrated Security=True"; //recuperer affichage>explorateur de serveurs
            conn.Open();
            string query = "SELECT * FROM Articles";
                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        articles.Add(new Article
                        {
                            Id = (int)reader["Id"],
                            Code = reader["Code"].ToString(),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Brand = reader["Brand"].ToString(),
                            Category = reader["Category"].ToString(),
                            Price = (decimal)reader["Price"],
                            Photo = reader["Photo"].ToString()
                        });
                    }
                }
            return articles;
        }

        public void Update(Article article)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Articles SET Code = @Code, Name = @Name, Description = @Description, Brand = @Brand, Category = @Category, Price = @Price, Photo = @Photo WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", article.Id);
                cmd.Parameters.AddWithValue("@Code", article.Code);
                cmd.Parameters.AddWithValue("@Name", article.Name);
                cmd.Parameters.AddWithValue("@Description", article.Description);
                cmd.Parameters.AddWithValue("@Brand", article.Brand);
                cmd.Parameters.AddWithValue("@Category", article.Category);
                cmd.Parameters.AddWithValue("@Price", article.Price);
                cmd.Parameters.AddWithValue("@Photo", article.Photo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Articles WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Article> Search(string keyword)
        {
            List<Article> articles = new List<Article>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Articles " +
                               "WHERE Code LIKE @Keyword OR Name LIKE @Keyword OR Description LIKE @Keyword";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        articles.Add(new Article
                        {
                            Id = (int)reader["Id"],
                            Code = reader["Code"].ToString(),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Brand = reader["Brand"].ToString(),
                            Category = reader["Category"].ToString(),
                            Price = (decimal)reader["Price"],
                            Photo = reader["Photo"].ToString()
                        });
                    }
                }
            }

            return articles;
        }

    }
}
