using System;
using System.Collections.Generic;
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
    /// Interaction logic for PatronInfoWindow.xaml
    /// </summary>
    public partial class PatronInfoWindow : Window
    {
        Patron Patron;
        Window ParentWindow;

        /// <summary>
        /// used anytime we need to connect to the database and interface with any of the information
        /// </summary>
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog= LibraryDB;Integrated Security=SSPI;";

        public PatronInfoWindow(Patron patron, Window parentWindow)
        {
            Patron = patron;
            ParentWindow = parentWindow;

            InitializeComponent();
            InitializeTextBlocks();

            Closing += OnClosing;
        }

        public void InitializeTextBlocks()
        {
            NameTextBlock.Text = Patron.Name;
            CardNumberTextBlock.Text = Patron.CardNumber.ToString();
            PhoneNumberTextBlock.Text = Patron.PhoneNumber;
            AddressTextBlock.Text = Patron.Address;
            BirthDateTextBlock.Text = Patron.BirthDate.ToString();

            if (Patron.KidReader == true) { KidReaderTextBlock.Text = "Yes"; }
            else { KidReaderTextBlock.Text = "No"; }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            InsertPatron(Patron.CardNumber, Patron.Name, Patron.PhoneNumber, Patron.Address, Patron.BirthDate.ToString(), Patron.KidReader);
            ParentWindow.Close();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.IsEnabled = true;
            Close();
        }

        /// <summary>
        /// Inserts a patron into the database
        /// </summary>
        /// <param name="cardNum">The number on the card</param>
        /// <param name="name">the persons full name</param>
        /// <param name="phoneNum">the individuals phone number</param>
        /// <param name="address">the individuals address</param>
        /// <param name="dateofBirth">the individuals Date of birth</param>
        /// <param name="isKid">Whether or not the patron is a kid</param>
        private void InsertPatron(string cardNum, string name, string phoneNum, string address, string dateofBirth, bool isKid)
        {
            int histID = 0;



            //This retrieves the HistoryId number from the database
            //when retrieving patron history information ingore all results whose checked out date equals its checked in date 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    //with no book copyids that exist this causes an error
                    command.CommandText = "INSERT INTO [LibraryDB].[History](BookCopyID, CheckedOutDate, CheckedInDate) VALUES('1', '2000-01-01 05:30:00', '2000-01-01 05:30:00')";
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
                        command.CommandText = "INSERT INTO [LibraryDB].[Patron](HistoryID, CardNumber, [FullName], PhoneNumber, [Address], BirthDate, KidReader) " +
                        "VALUES('" + histID + "', '" + cardNum + "', '" + name + "','" + phoneNum + "', '" + address + "', '" + dateofBirth + "', '0')";
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO [LibraryDB].[Patron](HistoryID, CardNumber, [FullName], PhoneNumber, [Address], BirthDate, KidReader) " +
                        "VALUES('" + histID + "', '" + cardNum + "', '" + name + "','" + phoneNum + "', '" + address + "', '" + dateofBirth + "', '1')";
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        private void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }     
    }
}
