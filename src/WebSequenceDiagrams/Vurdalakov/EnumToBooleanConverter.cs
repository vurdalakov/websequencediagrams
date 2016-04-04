namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    // http://wpf.2000things.com/2011/08/05/358-binding-a-radiobutton-to-an-enumerated-type/
    public class EnumToBooleanConverter : IValueConverter
    {
        // Convert enum [value] to boolean, true if matches [param]
        public object Convert(object value, Type targetType, object param, CultureInfo culture)
        {
            return value.ToString().Equals(param.ToString());
        }

        // Convert boolean to enum, returning [param] if true
        public object ConvertBack(object value, Type targetType, object param, CultureInfo culture)
        {
            return Enum.Parse(targetType, param.ToString(), true);
        }
    }
}
