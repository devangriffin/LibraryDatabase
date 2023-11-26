using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDatabase.Objects
{
    public class Patron
    {
        public int PatronID { get; }
        public int LoanID { get; }
        public int CardNumber { get; }
        public string Name { get; }
        public string PhoneNumber { get; }
        public string Address { get; }
        public DateOnly BirthDate { get; }
        public bool KidReader { get; }
    }
}
