using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class Loan
    {
        public string Book { get; set; }
        public int loanDay { get; set; }
        public int returnDay { get; set; }
    }
}
