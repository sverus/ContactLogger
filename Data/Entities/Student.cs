
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLogger.Data
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Moniker { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<ContactLog> Contacts { get; set; }
    }
}
