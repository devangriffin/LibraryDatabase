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
    }
}
