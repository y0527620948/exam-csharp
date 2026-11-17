using System;
using System.Collections.Generic;
using System.Linq;
using App.Models;

namespace App.Services
{
    public class FineReportService
    {
        private readonly FineCalculator fineCalculator;

        public FineReportService(FineCalculator fineCalculator)
        {
            this.fineCalculator = fineCalculator;
        }

        public ReportResult CreateReport(InputResult input, int reportDay)
        {
            ReportResult report = new ReportResult
            {
                Fines = new List<FineResult>(),
                Flagged = new List<FlaggedResult>(),
                Errors = input.Errors,
                Total = 0,
                TopBooks = new List<TopBookResult>(),
                AtRisk = new List<AtRiskResult>()
            };

            Dictionary<string, int> bookCounts = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (Reader reader in input.Readers)
            {
                int readerFine = 0;
                int positiveFineLoans = 0;

                foreach (Loan loan in reader.Loans)
                {
                    AddBook(bookCounts, loan.Book);

                    int fine = fineCalculator.Calculate(reader.ReaderType, loan, reportDay);
                    readerFine += fine;

                    if (fine > 0)
                    {
                        positiveFineLoans++;
                    }

                    AddFlaggedReaderIfNeeded(report.Flagged, reader, loan, reportDay);
                }

                report.Fines.Add(new FineResult
                {
                    ReaderId = reader.ReaderId,
                    Name = reader.Name,
                    Fine = readerFine
                });

                report.Total += readerFine;

                if (positiveFineLoans > 2)
                {
                    report.AtRisk.Add(new AtRiskResult
                    {
                        ReaderId = reader.ReaderId,
                        Name = reader.Name
                    });
                }
            }

            report.TopBooks = bookCounts
                .OrderByDescending(book => book.Value)
                .ThenBy(book => book.Key, StringComparer.Ordinal)
                .Take(3)
                .Select(book => new TopBookResult
                {
                    Book = book.Key,
                    Count = book.Value
                })
                .ToList();

            return report;
        }

        private void AddBook(Dictionary<string, int> bookCounts, string book)
        {
            if (bookCounts.ContainsKey(book))
            {
                bookCounts[book]++;
                return;
            }

            bookCounts[book] = 1;
        }

        private void AddFlaggedReaderIfNeeded(
            List<FlaggedResult> flaggedReaders,
            Reader reader,
            Loan loan,
            int reportDay)
        {
            if (!fineCalculator.ShouldFlag(reader.ReaderType, loan, reportDay)
                || ContainsReader(flaggedReaders, reader.ReaderId))
            {
                return;
            }

            flaggedReaders.Add(new FlaggedResult
            {
                ReaderId = reader.ReaderId,
                Name = reader.Name
            });
        }

        private bool ContainsReader(List<FlaggedResult> flaggedReaders, string readerId)
        {
            foreach (FlaggedResult flaggedReader in flaggedReaders)
            {
                if (flaggedReader.ReaderId == readerId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
