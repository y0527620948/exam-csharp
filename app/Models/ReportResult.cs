using System.Collections.Generic;

namespace App.Models
{
    public class ReportResult
    {
        public List<FineResult> Fines { get; set; }
        public List<FlaggedResult> Flagged { get; set; }
        public List<string> Errors { get; set; }
        public int Total { get; set; }
        public List<TopBookResult> TopBooks { get; set; }
        public List<AtRiskResult> AtRisk { get; set; }
    }
}
