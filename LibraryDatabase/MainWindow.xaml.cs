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
using System.Reflection.Emit;
using static Azure.Core.HttpHeader;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<BookTitle> BookList;
        // private List<BookTitle> BookList;
        // private List<Patron> PatronList;
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

            SetItemSources();
        }

        private void SetItemSources()
        {
            LibraryListView.ItemsSource = GetBookList();
            PatronListView.ItemsSource = GetPatronList();
            GenreCountListView.ItemsSource = GetGenreCounts();
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

      
        private void PatronListButton_Click(object sender, RoutedEventArgs e)
        {
            DisableOtherViews(PatronListView, PatronListButton);
            SetItemSources();
        }

        private void BookListButton_Click(object sender, RoutedEventArgs e)
        {
            DisableOtherViews(LibraryListView, BookListButton);
            SetItemSources();
        }

        private void GenreCountButton_Click(object sender, RoutedEventArgs e)
        {
            DisableOtherViews(GenreCountListView, GenreCountButton);
            SetItemSources();
        }

        private void DisableOtherViews(ListView listView, Button button)
        {
            foreach (UIElement element in LibraryGrid.Children)
            {
                if (element is Button button2)
                {
                    if (button.Name == button2.Name) { button.IsEnabled = false; }
                    else { button2.IsEnabled = true; }
                }

                if (element is ListView listView2)
                {
                    if (listView.Name == listView2.Name)
                    {
                        listView.IsEnabled = true;
                        listView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        listView2.IsEnabled = false;
                        listView2.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        #region InsertMethods (Moved)

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
                        "VALUES('" + authID + "', '" + audienceType + "', '" + genreName + "','" + isbn + "', '" + title + "', '" + publishDate + "', '" + publisher + "')";

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
                    command.CommandText = "INSERT INTO History (BookCopyID, CheckedOutDate, CheckedInDate) VALUES('1', '2000-01-01 05:30:00', '2000-01-01 05:30:00')";
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

        #endregion

        #region InsertGenre/Audience (Removed)
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
            if (exists == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + genreName + "')";

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

        #endregion

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
                    command.CommandText = "SELECT BookTitleID, Title, AudienceID, GenreID, ISBN, PublishDate, Publisher  FROM [LibraryDB].[BookTitle] ORDER BY Title ASC";
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
                                //form = String.Format("{0}", reader["GenreID"]);

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
        /// Gets the bookList
        /// </summary>
        /// <returns>The book list</returns>
        private List<BookTitle> GetBookList()
        {
            List<BookTitle> list = new List<BookTitle>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [LibraryDB].[BookTitle] ORDER BY Title ASC";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            
                            string bookTitleID = String.Format("{0}", reader["BookTitleID"]);
                            string title = String.Format("{0}", reader["Title"]);
                            string authorID = String.Format("{0}", reader["AuthorID"]);
                            string isbn = String.Format("{0}", reader["ISBN"]);
                            string publishDate = String.Format("{0}", reader["PublishDate"]);
                            string publisher = String.Format("{0}", reader["Publisher"]);
                            string audienceID = String.Format("{0}", reader["AudienceID"]);
                            string genreID = String.Format("{0}", reader["GenreID"]);
                            if (!(title.Equals("Test")))
                            {
                                string aName = GetAuthor(Convert.ToInt32(authorID));
                                BookTitle newBook = new BookTitle(Convert.ToInt32(bookTitleID), aName, Convert.ToInt32(audienceID), Convert.ToInt32(genreID), isbn, title, publisher, DateOnly.FromDateTime(Convert.ToDateTime(publishDate)));

                                list.Add(newBook);
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
            return list;
        }

        /// <summary>
        /// formats the book list
        /// </summary>
        /// <param name="books"></param>
        /// <returns></returns>
        private string GetAuthor(int id)
        {
            string name = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author] WHERE AuthorID = " + id;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["AuthorID"]);
                            if (form.Equals(id.ToString()))
                            {
                                form = String.Format("{0}", reader["FullName"]);
                                name = form;
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
             
             return name;
         }
             
        private List<Patron> GetPatronList()
        {
            List<Patron> list = new List<Patron>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [LibraryDB].[Patron] ORDER BY [FullName] ASC";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            int patronID = Convert.ToInt32(String.Format("{0}", reader["PatronID"]));
                            int cardNumber = Convert.ToInt32(String.Format("{0}", reader["CardNumber"]));
                            string fullName = String.Format("{0}", reader["FullName"]);
                            string phoneNumber = String.Format("{0}", reader["PhoneNumber"]);
                            string address = String.Format("{0}", reader["Address"]);
                            DateOnly birthDate = DateOnly.FromDateTime(Convert.ToDateTime(String.Format("{0}", reader["BirthDate"])));
                            bool kidReader = Convert.ToBoolean(String.Format("{0}", reader["KidReader"]));

                            Patron newPatron = new Patron(patronID, cardNumber, fullName, phoneNumber, address, birthDate, kidReader);                              

                            list.Add(newPatron);
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    connection.Close();
                }
            }
            return list;
        }

        private List<KeyValuePair<string, int>> GetGenreCounts()
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =   "SELECT G.GenreName, COUNT(BT.BookTitleID) AS BookCount\r\n" +
                                            "FROM LibraryDB.Genre G\r\n" +
                                            "LEFT JOIN LibraryDB.BookTitle BT ON BT.GenreID = G.GenreID\r\n" +
                                            "GROUP BY G.GenreName\r\n" +
                                            "ORDER BY G.GenreName;";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string genreName = (String.Format("{0}", reader["GenreName"]));
                            int bookCount = Convert.ToInt32(String.Format("{0}", reader["BookCount"]));

                            list.Add(new KeyValuePair<string, int>(genreName, bookCount));
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    connection.Close();
                }
            }
           
            return list;
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
                    command.CommandText = "SELECT BookTitleID, Title FROM [LibraryDB].[BookTitle] WHERE Title = '" + name + "'";
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
            SetItemSources();
        }

        #region PatronListView

        #endregion
    }
}
