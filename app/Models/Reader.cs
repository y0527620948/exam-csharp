using System.Collections.Generic;
using App.Enums;

namespace App.Models
{
    public class Reader
    {
        public string ReaderId { get; set; }
        public string Name { get; set; }
        public ReaderType ReaderType { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
