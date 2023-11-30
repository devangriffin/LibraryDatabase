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
using System.Windows.Shapes;
using LibraryDatabase.Objects;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for PatronWindow.xaml
    /// </summary>
    public partial class PatronWindow : Window
    {
        Window ParentWindow;

        public PatronWindow(Window parentWindow)
        {
            ParentWindow = parentWindow;
            InitializeComponent();

            Closing += OnClosing;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Patron newPatron;

            if (CardNumberTextBox.Text == "" || NameTextBox.Text == "" || PhoneNumberTextBox.Text == "" || AddressTextBox.Text == "" || CityTextBox.Text == "" || StateTextBox.Text == "" || BirthDateTextBox == null)
            {
                ErrorBlock.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                newPatron = new Patron(-1, CardNumberTextBox.Text.Trim(), NameTextBox.Text.Trim(), PhoneNumberTextBox.Text.Trim(), MergeAddressBoxes(), DateOnly.FromDateTime(BirthDateTextBox.DisplayDate), (bool)KidReaderCheckBox.IsChecked);
            }
            catch (Exception ex)
            {
                ErrorBlock.Text = ex.Message;
                ErrorBlock.Visibility = Visibility.Visible;
                return;
            }

            ErrorBlock.Visibility = Visibility.Hidden;

            PatronInfoWindow InfoWindow = new PatronInfoWindow(newPatron, this);

            InfoWindow.Show();

            IsEnabled = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.IsEnabled = true;
            Close();
        }

        private string MergeAddressBoxes()
        {
            string streetAddress = AddressTextBox.Text.Trim();
            string city = CityTextBox.Text.Trim();
            string state = StateTextBox.Text.Trim();
            string zipCode = ZipCodeTextBox.Text.Trim();

            return streetAddress + "\n" + city + ", " + state + " " + zipCode;
        }

        private void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }
    }
}
