using System;
using App.Enums;
using App.Models;

namespace App.Services
{
    public class FineCalculator
    {
        private const int StudentFreeDays = 14;
        private const int StudentGraceDays = 1;
        private const int StudentMaximumFine = 30;
        private const int GuestFreeDays = 7;
        private const decimal GuestFinePerDay = 2.5m;

        public int Calculate(ReaderType readerType, Loan loan, int reportDay)
        {
            int numberOfDays = CalculateNumberOfDays(loan, reportDay);

            switch (readerType)
            {
                case ReaderType.Student:
                    return CalculateStudentFine(numberOfDays);
                case ReaderType.Teacher:
                    return 0;
                case ReaderType.Guest:
                    return CalculateGuestFine(numberOfDays);
                default:
                    return 0;
            }
        }

        public bool ShouldFlag(ReaderType readerType, Loan loan, int reportDay)
        {
            return readerType == ReaderType.Teacher
                && !loan.ReturnDay.HasValue
                && CalculateNumberOfDays(loan, reportDay) > 60;
        }

        private int CalculateStudentFine(int numberOfDays)
        {
            int lateDays = numberOfDays - StudentFreeDays;

            if (lateDays <= 0 || lateDays == StudentGraceDays)
            {
                return 0;
            }

            return Math.Min(lateDays, StudentMaximumFine);
        }

        private int CalculateGuestFine(int numberOfDays)
        {
            int lateDays = Math.Max(0, numberOfDays - GuestFreeDays);
            decimal fine = lateDays * GuestFinePerDay;
            return (int)Math.Ceiling(fine);
        }

        private int CalculateNumberOfDays(Loan loan, int reportDay)
        {
            return (loan.ReturnDay ?? reportDay) - loan.LoanDay;
        }
    }
}
