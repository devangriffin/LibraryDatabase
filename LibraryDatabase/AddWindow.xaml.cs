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
        Window ParentWindow;




        public AddWindow(Window parentWindow)
        {
            ParentWindow = parentWindow;

            InitializeComponent();

            GenreComboBox.ItemsSource = Enum.GetValues(typeof(GenreEnum));
            AudienceComboBox.ItemsSource = Enum.GetValues(typeof(AudienceEnum));

            Closed += OnClosing;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.IsEnabled = true;
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BookTitle Book;

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
            catch // FormatException?
            {
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            ErrorTextBlock.Visibility = Visibility.Hidden;

            BookInfoWindow InfoWindow = new BookInfoWindow(Book, this);

            InfoWindow.Show();

            IsEnabled = false;
        }



        private void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }
    }
}
