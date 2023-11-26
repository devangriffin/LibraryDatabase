using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public BookInfoWindow(BookTitle book, AddWindow addWindow)
        {
            Book = book;
            ParentWindow = addWindow;

            InitializeComponent();
            InitializeTextBlocks();

            Closed += OnClosing;
        }

        public void InitializeTextBlocks()
        {
            BookTitleBlock.Text = Book.Title;
            BookAuthorBlock.Text = "[PlaceHolder]";
            BookISBNBlock.Text = Book.ISBN.ToString();
            BookPublisherBlock.Text = Book.Publisher;
            BookPublishDateBlock.Text = Book.PublishDate.ToString();
            BookAudienceBlock.Text = ((AudienceEnum)Book.AudienceID).ToString();
            BookGenreBlock.Text = ((GenreEnum)Book.GenreID).ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Cancel(Book);
            Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Confirm(Book);
            Close();
        }

        protected void OnClosing(object sender, EventArgs e) { ParentWindow.IsEnabled = true; }
    }
}
