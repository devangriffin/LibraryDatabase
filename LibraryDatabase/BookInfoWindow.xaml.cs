using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
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
using System.Windows.Shapes;
using LibraryDatabase.Objects;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for BookSummaryWindow.xaml
    /// </summary>
    public partial class BookInfoWindow : Window
    {
        BookTitle Book;
        AddWindow ParentWindow;

        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog= LibraryDB;Integrated Security=SSPI;";


        public BookInfoWindow(BookTitle book, AddWindow parentWindow)
        {
            Book = book;
            ParentWindow = parentWindow;

            InitializeComponent();
            InitializeTextBlocks();

            Closed += OnClosing;
        }

        public void InitializeTextBlocks()
        {
            BookTitleBlock.Text = Book.Title;
            BookAuthorBlock.Text = ParentWindow.GetAuthor();
            BookISBNBlock.Text = Book.ISBN.ToString();
            BookPublisherBlock.Text = Book.Publisher;
            BookPublishDateBlock.Text = Book.PublishDate.ToString();
            BookAudienceBlock.Text = ((AudienceEnum)Book.AudienceID).ToString();
            BookGenreBlock.Text = ((GenreEnum)Book.GenreID).ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.IsEnabled = true;
            Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            InsertBook(ParentWindow.GetAuthor(), Book.GenreID, Book.ISBN, Book.Title, Book.PublishDate.ToString(), Book.Publisher, Book.AudienceID);
            ParentWindow.Close();
            Close();
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
        private void InsertBook(string authorsName, int genreName, string isbn, string title, string publishDate, string publisher, int audienceType)
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
                    command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author]";
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
                    command.CommandText = "INSERT INTO [LibraryDB].[BookTitle](AuthorID, AudienceID, GenreID, ISBN, Title, PublishDate, Publisher) " +
                        "VALUES('" + authID + "', '" + audienceType + "', '" + genreName + "','" + isbn + "', '" + title + "', '" + publishDate + "', '" + publisher + "')";

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        protected void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }
    }
}
