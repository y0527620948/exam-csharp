using System.Collections.Generic;

namespace App.Models
{
    public class InputResult
    {
        public List<Reader> Readers { get; set; }
        public List<string> Errors { get; set; }
    }
}
