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
using Microsoft.Data.SqlClient;
using System.Net;
using LibraryDatabase.Objects;
using static System.Reflection.Metadata.BlobBuilder;
using System.Security.Policy;
using System.Xml.Linq;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<BookTitle> BookList;

        /// <summary>
        /// used anytime we need to connect to the database and interface with any of the information
        /// </summary>
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog= LibraryDB;Integrated Security=SSPI;";
        
        /*
         * When we have a server up and running this is for security and would be used instead for connectionString
        "Data Source=serverName;" +
        "Initial Catalog=LibraryDB;" +
        "User id=UserName;" + 
        "Password=Secret;";
        */

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            BookList = new List<BookTitle>();
            BookList.Add(new BookTitle(-1, 1, 1, 12356, "Narnia", "No Clue", new DateOnly(2022, 1, 2)));
            LibraryListView.ItemsSource = BookList;
     
            //ResizeColumns();
        }
        
        /// <summary>
        /// Creates a new Add Window for books when Add button is clicked
        /// </summary>
        /// <param name="sender">The Button</param>
        /// <param name="e">Event Arguments</param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow AddBookWindow = new AddWindow(this);
            AddBookWindow.Show();

            IsEnabled = false;
        }

        /// <summary>
        /// Creates a new Add Window for patrons when Add Patron button is clicked
        /// </summary>
        /// <param name="sender">The Button</param>
        /// <param name="e">Event Arguments</param>
        private void AddPatronButton_Click(object sender, RoutedEventArgs e)
        {
            PatronWindow NewPatronWindow = new PatronWindow(this);
            NewPatronWindow.Show();

            IsEnabled = false;
        }

        public void PopulateData()
        {
            //AudienceColumn.DisplayMemberBinding = "GenreID";
            //BookList = GetBooks();
            //LibraryDataGrid.ItemsSource = BookList;
        }

        private void ResizeColumns()
        {
            double ColumnWidth = 1;
            TitleColumn.Width = ColumnWidth;
            AuthorColumn.Width = ColumnWidth;
            ISBNColumn.Width = ColumnWidth;
            PublishDateColumn.Width = ColumnWidth;
            PublisherColumn.Width = ColumnWidth;
            AudienceColumn.Width = ColumnWidth;
            GenreColumn.Width = ColumnWidth;
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

                        command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author] WHERE FullName = " + authorsName + "'";
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
                        "VALUES('"+ authID + "', '"+ audienceType + "', '"+ genreName + "','" + isbn + "', '"+ title + "', '"+ publishDate + "', '"+ publisher + "')";
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //when retrieving patron history information ingore all results whose checked out date equals its checked in date 

        /// <summary>
        /// Inserts a patron into the database
        /// </summary>
        /// <param name="cardNum">The number on the card</param>
        /// <param name="name">the persons full name</param>
        /// <param name="phoneNum">the individuals phone number</param>
        /// <param name="address">the individuals address</param>
        /// <param name="dateofBirth">the individuals Date of birth</param>
        /// <param name="isKid">Whether or not the patron is a kid</param>
        private void InsertPatron(int cardNum, string name, string phoneNum, string address, string dateofBirth, bool isKid)
        {
            int histID = 0;



            //This retrieves the HistoryId number from the database
            //when retrieving patron history information ingore all results whose checked out date equals its checked in date 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO History (BookCopyID, CheckedOutDate, CheckedInDate) VALUES('0', '2000-01-01 05:30:00', '2000-01-01 05:30:00')";
                    connection.Open();
                    command.ExecuteNonQuery();
                    
                    command.CommandText = "SELECT HistoryID FROM [LibraryDB].[History]";
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["HistoryID"]);
                            histID = int.Parse(form);
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                    connection.Close();
                }
            }



            //inserts the Patron into the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "";
                    if (isKid)
                    {
                        command.CommandText = "INSERT INTO Patron (HistoryID, CardNumber, [FullName], PhoneNumber, [Address], BirthDate, KidReader) " +
                        "VALUES('" + histID + "', '" + cardNum + "', '" + name + "','" + phoneNum + "', '" + address + "', '" + dateofBirth + "', '0')";
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO Patron (HistoryID, CardNumber, [FullName], PhoneNumber, [Address], BirthDate, KidReader) " +
                        "VALUES('" + histID + "', '" + cardNum + "', '" + name + "','" + phoneNum + "', '" + address + "', '" + dateofBirth + "', '1')";
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Inserts a Genre into the database
        /// </summary>
        /// <param name="genreName">the name of the genre</param>
        /// <returns>a bool of if the value was inserted or not</returns>
        private bool InsertGenre(string genreName)
        {
            bool inserted = true;
            bool exists = false;

            //checks to see if genre already exists
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT GenreName FROM [LibraryDB].[Genre] WHERE GenreName =" + genreName + "'";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["GenreName"]);
                            if (form == genreName)
                            {
                                exists = true;
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

            //inserts if genre is not in the database
            if(exists == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + genreName +"')";
                    
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            else
            {
                inserted = false;
            }
            
            return inserted;
        }

        /// <summary>
        /// Inserts an audience into the database
        /// </summary>
        /// <param name="audienceName">the name of the audience the book is geared for</param>
        /// <returns>a bool of if the value was inserted or not</returns>
        private bool InsertAudience(string audienceName, bool forKids)
        {
            bool inserted = true;
            bool exists = false;

            //checks to see if audience already exists
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT AudienceName FROM [LibraryDB].[Audience] WHERE AudienceName =" + audienceName + "'";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["AudienceName"]);
                            if (form == audienceName)
                            {
                                exists = true;
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

            //inserts if audience is not in the database
            if (exists == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "";
                        if (forKids)
                        {
                            command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + audienceName + "', '0')";
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + audienceName + "', '1')";
                        }
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            else
            {
                inserted = false;
            }

            return inserted;
        }

        /// <summary>
        /// gets a list of all books from the database
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
                            if (!(form.Equals("Test")))
                            {
                                books.Add(form);
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
            return books;
        }

        /// <summary>
        /// gets a list of books from the database with set search parameters TODO
        /// </summary>
        /// <param name="name">the books title</param>
        /// <returns> a list of all book titles in the library</returns>
        private List<string> GetBook(string name)
        {
            List<string> books = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT BookTitleID, Title FROM [LibraryDB].[BookTitle] WHERE Title = " + name + "'";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["Title"]);
                            if (form.Equals(name))
                            {
                                form = String.Format("{0}", reader["Title"]);
                                books.Add(form);
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

        /// <summary>
        /// updates the list view when an item is added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            List<string> BookTitles = GetBooks();
            TheBooks.ItemsSource = BookTitles;
            
        }
    }
}
