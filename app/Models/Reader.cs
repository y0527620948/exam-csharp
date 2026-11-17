using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class Reader
    {
        public string Reader_id { get; set; }
        public string Name { get; set; }
        public Type ReaderType { get; set; }
        public List<Loan> Loans { get; set; }

    }
}
