using System;
using System.Collections.Generic;
using System.Globalization;

namespace Plugins.DatePicker.Scripts
{
    /// <summary>
    /// A class to assist viewing DateTime Dates for different Cultures
    /// </summary>
    public class DateView
    {
        public CultureInfo CultureInfo { get; private set; }
        
        public DateView() => CultureInfo = CultureInfo.CurrentCulture;
        
        public DateView(CultureInfo location) => SetCulture(location);
        
        /// <summary>
        /// Changes the Culture/Language of this class, typically this is done in the constructor
        /// </summary>
        public void SetCulture(CultureInfo culture) => CultureInfo = culture;

        /// <summary>
        /// returns specific Cultures for DateView,
        /// see CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        /// </summary>
        public static IEnumerable<CultureInfo> CultureInfos => CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        /// <summary>
        ///  returns the Days of the Week as a string Array in the selected Culture
        /// </summary>
        public string[] DaysOfTheWeek() => CultureInfo.DateTimeFormat.DayNames;

        /// <summary>
        /// returns the Months of the Year as a string Array in the selected Culture
        /// </summary>
        public string[] MonthsOfTheYear() => CultureInfo.DateTimeFormat.MonthNames;
        
        /// <summary>
        /// Invariant Culture version would look like: 01/01/1970
        /// </summary>
        public string ToFormattedDate(DateTime dateTime) => dateTime.ToString("d", CultureInfo);
        
        /// <summary>
        /// Invariant Culture version would look like: Thursday, 01 January 1970
        /// </summary>
        public string ToFullDateAsString(DateTime dateTime) => dateTime.ToString("D", CultureInfo);
        
        /// <summary>
        /// Invariant Culture version would look like: January 01
        /// </summary>
        public string ToShortDateAsString(DateTime dateTime) => dateTime.ToString("m", CultureInfo);
        
        /// <summary>
        /// Invariant Culture version would look like: 1970 January
        /// </summary>
        public string ToMonthOfYear(DateTime dateTime) => dateTime.ToString("Y", CultureInfo);

        public override string ToString() => ToFormattedDate(DateTime.Now);
    }
}