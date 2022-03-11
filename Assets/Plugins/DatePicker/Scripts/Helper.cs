using System.Globalization;

namespace Plugins.DatePicker.Scripts
{
    public static class Helper
    {
        public static string Date(this DatePicker datePicker, CultureInfo culture)
        {
            var dateView = new DateView(culture);
            return dateView.ToFormattedDate(datePicker.SelectedDate);
        }
    }
}