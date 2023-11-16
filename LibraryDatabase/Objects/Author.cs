using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDatabase.Objects
{
    public class Author
    {
        public int AuthorID { get; }
        public int BookTitleID { get; }
        public string FullName { get; }
        public string College { get; }
        public string HomeCountry { get; }
        public DateOnly BirthDate { get; }
        public bool LifeStatus { get; set; }
    }
}
