namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ReverseBooleanConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return !System.Convert.ToBoolean(value);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return !System.Convert.ToBoolean(value);
        }
    }
}
