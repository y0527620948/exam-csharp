using System;
using System.Collections.Generic;
using App.Enums;
using App.Models;

namespace App.Readers
{
    public class LoanReader
    {
        public InputResult Read(List<string> rows)
        {
            List<Reader> readers = new List<Reader>();
            List<string> errors = new List<string>();

            foreach (string line in rows)
            {
                string[] fields = line.Split(',', StringSplitOptions.None);

                if (!TryCreateLoan(fields, out Reader reader, out Loan loan))
                {
                    errors.Add(line);
                    continue;
                }

                Reader existingReader = FindReader(readers, reader.ReaderId);

                if (existingReader == null)
                {
                    readers.Add(reader);
                    existingReader = reader;
                }

                existingReader.Loans.Add(loan);
            }

            return new InputResult
            {
                Readers = readers,
                Errors = errors
            };
        }

        private bool TryCreateLoan(string[] row, out Reader reader, out Loan loan)
        {
            reader = null;
            loan = null;

            if (row.Length != 6)
            {
                return false;
            }

            for (int i = 0; i < row.Length; i++)
            {
                row[i] = row[i].Trim();
            }

            for (int i = 0; i < row.Length - 1; i++)
            {
                if (row[i] == "")
                {
                    return false;
                }
            }

            if (!TryReadReaderType(row[2], out ReaderType readerType)
                || !int.TryParse(row[4], out int loanDay)
                || !IsValidDay(loanDay))
            {
                return false;
            }

            int? returnDay = null;

            if (row[5] != "")
            {
                if (!int.TryParse(row[5], out int parsedReturnDay)
                    || !IsValidDay(parsedReturnDay))
                {
                    return false;
                }

                returnDay = parsedReturnDay;
            }

            if (returnDay.HasValue && returnDay.Value < loanDay)
            {
                return false;
            }

            reader = new Reader
            {
                ReaderId = row[0],
                Name = row[1],
                ReaderType = readerType,
                Loans = new List<Loan>()
            };

            loan = new Loan
            {
                Book = row[3],
                LoanDay = loanDay,
                ReturnDay = returnDay
            };

            return true;
        }

        private bool TryReadReaderType(string value, out ReaderType readerType)
        {
            switch (value.ToLowerInvariant())
            {
                case "student":
                    readerType = ReaderType.Student;
                    return true;
                case "teacher":
                    readerType = ReaderType.Teacher;
                    return true;
                case "guest":
                    readerType = ReaderType.Guest;
                    return true;
                case "alumna":
                    readerType = ReaderType.Alumna;
                    return true;
                default:
                    readerType = ReaderType.Student;
                    return false;
            }
        }

        private Reader FindReader(List<Reader> readers, string readerId)
        {
            foreach (Reader reader in readers)
            {
                if (reader.ReaderId == readerId)
                {
                    return reader;
                }
            }

            return null;
        }

        private bool IsValidDay(int day)
        {
            return day >= 1 && day <= 365;
        }
    }
}
