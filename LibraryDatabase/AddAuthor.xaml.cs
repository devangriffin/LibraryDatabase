using LibraryDatabase.Objects;
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

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for AddAuthor.xaml
    /// </summary>
    public partial class AddAuthor : Window
    {
        Window ParentWindow;
        BookTitle Book;

        public AddAuthor(BookTitle book, Window parentWindow)
        {
            this.ResizeMode = ResizeMode.NoResize;
            Book = book;
            InitializeComponent();
            InitializeTextBlocks();
            ParentWindow = parentWindow;
            Closing += OnClosing;
        }

        private void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }

        public void InitializeTextBlocks()
        {
            BookTitleBlock.Text = Book.Title;
            BookAuthorBlock.Text = Book.AuthorsName;
            BookISBNBlock.Text = Book.ISBN.ToString();
            BookPublisherBlock.Text = Book.Publisher;
            BookAudienceBlock.Text = ((AudienceEnum)Book.AudienceID).ToString();
            BookGenreBlock.Text = ((GenreEnum)Book.GenreID).ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.IsEnabled = true;
            Close();
        }
    }
}
