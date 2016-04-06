namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NullToFalseConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (Boolean)value ? new Object() : null;
        }
    }
}
