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

        public Patron(int patronID, int cardNum, string name, string phoneNum, string add, DateOnly birthDate, bool kid)
        {
            PatronID = patronID;
            CardNumber = cardNum;
            Name = name;
            PhoneNumber = phoneNum;
            Address = add;
            BirthDate = birthDate;
            KidReader = kid;

            // Temp Numbers
            LoanID = -1;
        }
    }
}
