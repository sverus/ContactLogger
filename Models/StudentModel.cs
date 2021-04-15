
using ContactLogger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLogger.Models
{
    public class StudentModel
    {
        public string Moniker { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<ContactLogModel> Contacts { get; set; }
    }
}
