using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDatabase.Objects
{
    public class BookTitle
    {
        public int BookTitleID { get; }
        public int AudienceID { get; }
        public int GenreID { get; }
        public int ISBN { get; }
        public string Title { get; }
        public string Publisher { get; }
        public DateOnly PublishDate { get; }

        public AudienceEnum Audience { get { return (AudienceEnum)AudienceID; } }

        public GenreEnum Genre { get { return (GenreEnum)GenreID; } }

        public BookTitle(int bookTitleID, int audienceID, int genreID, int isbn, string title, string publisher, DateOnly publishDate)
        {
            BookTitleID = bookTitleID;
            AudienceID = audienceID;
            GenreID = genreID;
            ISBN = isbn;
            Title = title;
            Publisher = publisher;
            PublishDate = publishDate;
        }
    }
}
