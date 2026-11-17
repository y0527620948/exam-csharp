using System;
using System.Collections.Generic;
using App.Models;

namespace App.Writers
{
    public class FinesReportWriter
    {
        public void Write(ReportResult report)
        {
            Console.WriteLine("FINES");

            foreach (FineResult fine in report.Fines)
            {
                Console.WriteLine(fine.ReaderId + "," + fine.Name + "," + fine.Fine);
            }

            Console.WriteLine("FLAGGED");

            foreach (FlaggedResult flaggedReader in report.Flagged)
            {
                Console.WriteLine(flaggedReader.ReaderId + "," + flaggedReader.Name);
            }

            Console.WriteLine("ERRORS");

            foreach (string error in report.Errors)
            {
                Console.WriteLine(error);
            }

            Console.WriteLine("TOTAL," + report.Total);

            Console.WriteLine("TOP_BOOKS");

            foreach (TopBookResult topBook in report.TopBooks)
            {
                Console.WriteLine(topBook.Book + "," + topBook.Count);
            }

            Console.WriteLine("AT_RISK");

            foreach (AtRiskResult reader in report.AtRisk)
            {
                Console.WriteLine(reader.ReaderId + "," + reader.Name);
            }
        }
    }
}
