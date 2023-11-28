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
    /// Interaction logic for PatronInfoWindow.xaml
    /// </summary>
    public partial class PatronInfoWindow : Window
    {
        Patron Patron;
        Window ParentWindow;

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
            ParentWindow.Close();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.IsEnabled = true;
            Close();
        }

        private void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }     
    }
}
