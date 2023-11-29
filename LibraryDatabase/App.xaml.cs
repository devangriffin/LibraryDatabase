using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics.Metrics;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using LibraryDatabase.Objects;
using System.Security.Policy;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog= LibraryDB;Integrated Security=SSPI;";

        public App()
        {
            string sqlConnectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog= LibraryDB;Integrated Security=SSPI;";
            string userName = Environment.UserName;
            FileInfo file = new FileInfo(@"C:\Users\"+userName+ @"\source\repos\LibraryDatabase\LibraryDatabase\Scripts\CreateLibraryDatabase.sql");
            string script = file.OpenText().ReadToEnd();
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Microsoft.SqlServer.Management.Common.ServerConnection connection = new Microsoft.SqlServer.Management.Common.ServerConnection(conn);
            Server server = new Server(connection);

            server.ConnectionContext.ExecuteNonQuery(script);
            setupTables();
            int x = 1;
            InsertBook("Dummy", x, x, "Test", "0001-01-01", "Nobody", x);

            using (SqlConnection connection2 = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection2.CreateCommand())
                {

                    command.CommandText = "INSERT INTO [LibraryDB].[Shelf](ShelfNumber, Section) VALUES('0', '0')";
                    connection2.Open();
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[BookCopy](BookTitleID, ShelfID, [PageCount], DamageLevel) VALUES('1', '1', '0', '999')";
                    command.ExecuteNonQuery();

                    connection2.Close();
                }
            }
        }

        private void setupTables()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    #region The Genre generic Inserts
                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Action+"')";
                    connection.Open();
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Adventure+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Art+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.AutoBiography+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Biography+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Dystopian+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Fantasy+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.GraphicNovel+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.HistoricalFiction + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.History + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Horror + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Humor + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Mystery + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Religious + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Romance + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Science + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.ScienceFiction + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.SelfHelp + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Thriller + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Travel + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.TrueCrime + "')";
                    command.ExecuteNonQuery();
                    #endregion

                    #region The Audience generic Inserts
                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Children + "', '0')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Teens + "', '0')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.YoungAdults + "', '0')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Adults + "', '1')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Men + "', '1')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Women + "', '1')";
                    command.ExecuteNonQuery();
                    #endregion

                    command.CommandText = "INSERT INTO [LibraryDB].[Author](FullName) VALUES('Dummy')";
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }

            
        }

        /// <summary>
        /// Inserts a book into the database along with a new author and the books audiance
        /// </summary>
        /// <param name="authorsName">the authors name</param>
        /// <param name="genreName">the name of the genre</param>
        /// <param name="isbn">the ISBN Number</param>
        /// <param name="title">The books Title</param>
        /// <param name="publishDate">the day the book was published in year-month-day like "2015-01-24"</param>
        /// <param name="publisher">The name of the publisher</param>
        /// <param name="readerType">The books target Audience</param>
        private void InsertBook(string authorsName, int genreName, int isbn, string title, string publishDate, string publisher, int audienceType)
        {
            int authID = 0;
            bool exists = false;
            //Checks to see if authors name exists in database
            /*
             * If SqlConnection and SqlCommand are NOT Functioning then
             * 1) Right Click 'LibraryDatabase'
             * 2) Click on Manage NuGet Packages option
             * 3) In the NuGet Package Manager window, Select the Browser Tab. Search for System.Data.SqlClient
             * 4) System.Data.SqlClient by Microsoft and click install
             */
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author] WHERE FullName = '" + authorsName + "'";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["FullName"]);
                            if (form == authorsName)
                            {
                                exists = true;
                                form = String.Format("{0}", reader["AuthorID"]);
                                authID = int.Parse(form);
                            }

                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    connection.Close();
                }
            }

            //if author doesn't exist then insert Author's name into LibraryDB.Author
            //Then gets the Authors AuthorID from LibraryDB.Author
            if (exists == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [LibraryDB].[Author](FullName) VALUES('" + authorsName + "')";

                        connection.Open();
                        command.ExecuteNonQuery();

                        command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author] WHERE FullName = '" + authorsName + "'";
                        SqlDataReader reader = command.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                string form = String.Format("{0}", reader["AuthorID"]);
                                authID = int.Parse(form);
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                        connection.Close();
                    }
                }
            }
            //inserts the book into LibraryDB.BookTitle but no AudienceID and No GenreID
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [LibraryDB].[BookTitle] (AuthorID, AudienceID, GenreID, ISBN, Title, PublishDate, Publisher) " +
                        "VALUES('"+ authID + "', '" + audienceType + "', '" + genreName + "','" + isbn + "', '" + title + "', '" + publishDate + "', '" + publisher + "')";

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }





    }
}
