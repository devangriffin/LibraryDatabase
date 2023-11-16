using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDatabase.Objects
{
    public class BookCopy
    {
        public int BookCopyID { get; }
        public int BookTitleID { get; }
        public int ShelfID { get; }
        public int PageCount { get; }
        public int DamageLevel { get; }
    }
}
