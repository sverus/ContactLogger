using ContactLogger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLogger.Models
{
    public class ContactLogModel
    {
        public int Id { get; set; }
        //public Student Student { get; set; }
        public string ContactSummary { get; set; }
        public DateTime ContactDate { get; set; } = DateTime.MinValue;
    }
}
