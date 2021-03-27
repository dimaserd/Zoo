using System;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Logic.Models
{
    /// <summary>
    /// Модель месяца для календаря
    /// </summary>
    public class CalendarMonthViewModel
    {
        private DateTime GetLastDayInLastMonthDate(DateTime date)
        {
            if (date.Month == 1)
            {
                return new DateTime(date.Year - 1, 12, GetDaysInLastMonth(date));
            }

            return new DateTime(date.Year, date.Month - 1, GetDaysInLastMonth(date));
        }

        private DateTime GetDayInNextMonth(DateTime date, int day)
        {
            bool isLastMonth = date.Month == 12;

            if (isLastMonth)
            {
                return new DateTime(date.Year + 1, 1, day);
            }

            return new DateTime(date.Year, date.Month + 1, day);
        }

        private int GetDaysInLastMonth(DateTime date)
        {
            if (date.Month == 1)
            {
                return DateTime.DaysInMonth(date.Year - 1, 12);
            }

            return DateTime.DaysInMonth(date.Year, date.Month - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthShift"></param>
        public CalendarMonthViewModel(int monthShift)
        {
            DateTime date = DateTime.Now;

            date = date.AddMonths(monthShift);

            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            GetDaysInLastMonth(date);

            DateTime lastDayInLastMonthDate = GetLastDayInLastMonthDate(date);


            DateTime firstDayInThisMonthDate = new DateTime(date.Year, date.Month, 1);

            DateTime lastDayInThisMonthDate = new DateTime(date.Year, date.Month, daysInMonth);

            int lastDayInLastMonthDayOfWeekInt = (int)lastDayInThisMonthDate.DayOfWeek;

            int startDayOfWeekInThisMonth = (int)firstDayInThisMonthDate.DayOfWeek;

            //для того чтобы догружались даты из предыдущего месяца
            startDayOfWeekInThisMonth = startDayOfWeekInThisMonth == 0 ? 7 : startDayOfWeekInThisMonth;


            TodayDate = date.Date;

            DaysInPrevMonth = new List<DateTime>();
            DaysInThisMonth = new List<DateTime>();
            DaysInNextMonth = new List<DateTime>();

            int forParam = startDayOfWeekInThisMonth == 1 ? 8 : startDayOfWeekInThisMonth;

            for (int i = 1; i < forParam; i++)
            {
                DateTime dateP = lastDayInLastMonthDate.AddDays(1 - i);
                DaysInPrevMonth.Add(dateP);
            }
            DaysInPrevMonth = DaysInPrevMonth.OrderBy(x => x).ToList();

            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime dateP = new DateTime(date.Year, date.Month, i);

                DaysInThisMonth.Add(dateP);
            }

            for (var i = lastDayInLastMonthDayOfWeekInt; i < 7; i++)
            {
                DateTime dateP = GetDayInNextMonth(date, 1 + i);

                DaysInNextMonth.Add(dateP);
            }
        }

        /// <summary>
        /// Дата сегодняшнего дня
        /// </summary>
        public DateTime TodayDate { get; set; }

        /// <summary>
        /// Дни для дат из предыдущего месяца
        /// </summary>
        public List<DateTime> DaysInPrevMonth { get; set; }

        /// <summary>
        /// Дни для дат из этого месяца
        /// </summary>
        public List<DateTime> DaysInThisMonth { get; set; }

        /// <summary>
        /// Дни для дат в следующем месяце
        /// </summary>
        public List<DateTime> DaysInNextMonth { get; set; }
    }
}