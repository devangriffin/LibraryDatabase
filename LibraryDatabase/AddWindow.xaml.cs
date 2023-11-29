using LibraryDatabase.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.ComponentModel;
using System.Data.SqlClient;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        MainWindow ParentWindow;

        /// <summary>
        /// AddWindow Constructor
        /// </summary>
        /// <param name="parentWindow">The previous window</param>
        public AddWindow(Window parentWindow)
        {
            ParentWindow = parentWindow;

            InitializeComponent();

            // Sets the Combo Boxes to the the right values
            GenreComboBox.ItemsSource = Enum.GetValues(typeof(GenreEnum));
            AudienceComboBox.ItemsSource = Enum.GetValues(typeof(AudienceEnum));

            Closed += OnClosing;
        }
        
        /// <summary>
        /// gets the author name from this textbox
        /// </summary>
        /// <returns></returns>
        public string GetAuthor()
        {
            return AuthorTextBox.Text;
        }

        /// <summary>
        /// Exits out of the window and enables the previous window
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Event Args</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.IsEnabled = true;
            Close();
        }

        /// <summary>
        /// When the Add Button is pressed, a confirmation window is opened.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Event Args</param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BookTitle Book;

            // Checks to make sure all fields have information
            if (TitleTextBox.Text == "" || AuthorTextBox.Text == "" || ISBNTextBox.Text == "" || PublisherTextBox.Text == "" || PublishDateBox.SelectedDate == null || AudienceComboBox.SelectedItem == null || GenreComboBox.SelectedItem == null)
            {
                ErrorTextBlock.Visibility = Visibility.Visible;
                return; 
            }

            // Temporary Values for ID
            try
            {
                Book = new BookTitle(0, (int)AudienceComboBox.SelectedItem, (int)GenreComboBox.SelectedItem, Convert.ToInt32(ISBNTextBox.Text), TitleTextBox.Text, PublisherTextBox.Text, DateOnly.FromDateTime(PublishDateBox.DisplayDate));
            }
            catch // Catches any formatting errors - FormatException?
            {
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            ErrorTextBlock.Visibility = Visibility.Hidden;

            // Creates a confimation window
            BookInfoWindow InfoWindow = new BookInfoWindow(Book, this);

            InfoWindow.Show();

            IsEnabled = false;
        }

        /// <summary>
        /// Makes sure the parent window is enabled when this window is closed.
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event Arguments</param>
        private void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }
    }
}
