using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDatabase.Objects
{
    public class BookTitle
    {
        /// <summary>
        /// The books title id
        /// </summary>
        public int BookTitleID { get; }
        public string AuthorsName { get; }
        public int AudienceID { get; }
        public int GenreID { get; }
        public string ISBN { get; }
        public string Title { get; }
        public string Publisher { get; }
        public DateOnly PublishDate { get; }
        public AudienceEnum Audience { get { return (AudienceEnum)AudienceID; } }
        public GenreEnum Genre { get { return (GenreEnum)GenreID; } }
        public string DateString { get { return PublishDate.ToString(); } }


        public BookTitle(int bookTitleID, string authName, int audienceID, int genreID, string isbn, string title, string publisher, DateOnly publishDate)
        {
            BookTitleID = bookTitleID;
            AuthorsName = authName;
            AudienceID = audienceID;
            GenreID = genreID;
            ISBN = isbn;
            Title = title;
            Publisher = publisher;
            PublishDate = publishDate;
        }

    }
}
