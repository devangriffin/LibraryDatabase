using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Net;
using LibraryDatabase.Objects;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// used anytime we need to connect to the database and interface with any of the information
        /// </summary>
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[""].ConnectionString;
        /*
         * When we have a server up and running this is for security and would be used instead for connectionString
        "Data Source=serverName;" +
        "Initial Catalog=LibraryDB;" +
        "User id=UserName;" + 
        "Password=Secret;";
        */




        public MainWindow()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Inserts a book into the database along with a new author and the books audiance
        /// </summary>
        /// <param name="authorsName">the authors name</param>
        /// <param name="isbn">the ISBN Number</param>
        /// <param name="title">The books Title</param>
        /// <param name="publishDate">the day the book was published in year-month-day like "2015-01-24"</param>
        /// <param name="publisher">The name of the publisher</param>
        /// <param name="readerType">The books target Audience</param>
        /// <param name="forKids">whether or not the book is for kids</param>
        private void InsertBook(string authorsName, int isbn, string title, string publishDate, string publisher, string readerType, bool forKids)
        {
            int authID = 0;
            int audID = 0;
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
                    command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author]";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["FullName"]);
                            if(form == authorsName)
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

                    //gets the AudianceID
                    if (forKids)
                    {
                        command.CommandText = "INSERT INTO Audience (AudienceName, KidsRead) VALUES('" + readerType + "', '1')";
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO Audience (AudienceName, KidsRead) VALUES('" + readerType + "', '0')";
                    }
                    command.ExecuteNonQuery();

                    //retrieves the new Identity value for audience
                    //unable to get a proper value as this entire table is stupid
                    command.CommandText = "SELECT AudienceID, AudienceName, KidsRead FROM [LibraryDB].[Audience] WHERE AudienceName = " + readerType;
                    SqlDataReader reader2 = command.ExecuteReader();
                    try
                    {
                        while (reader2.Read())
                        {
                            string form = String.Format("{0}", reader2["AudienceName"]);
                            if (form == readerType)
                            {

                                form = String.Format("{0}", reader2["AudienceID"]);
                                audID = int.Parse(form);
                            }

                        }
                    }
                    finally
                    {
                        reader2.Close();
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
                        command.CommandText = "INSERT INTO Author (FullName) VALUES('" + authorsName + "')";

                        connection.Open();
                        command.ExecuteNonQuery();

                        command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author] WHERE FullName = " + authorsName;
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
                    command.CommandText = "INSERT INTO BookTitle (AuthorID, AudienceID, GenreID, ISBN, Title, PublishDate, Publisher) " +
                        "VALUES('"+ authID + "', '"+ audID + "', '0','" + isbn + "', '"+ title + "', '"+ publishDate + "', '"+ publisher + "')";
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        /// <summary>
        /// gets a list of books from the database
        /// </summary>
        /// <returns> a list of all book titles in the library</returns>
        private List<string> GetBooks()
        {
            List<string> books = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT BookTitleID, Title FROM [LibraryDB].[BookTitle]";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["Title"]);
                            books.Add(form);

                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    connection.Close();
                }
            }
            return books;
        }

        /// <summary>
        /// gets a list of patrons from the database
        /// </summary>
        /// <returns>a list of patrons</returns>
        private List<string> GetPatrons()
        {
            List<string> patrons = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT PatronID, [FullName] FROM [LibraryDB].[Patron]";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["[FullName]"]);
                            patrons.Add(form);

                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    connection.Close();
                }
            }
            return patrons;
        }
    }
}
