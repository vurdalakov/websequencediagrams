namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    // <vurdalakov:FormatMenuItemHeaderConverter x:Key="FormatMenuItemHeaderConverter" />
    // <MenuItem Header="{Binding FileName, Converter={StaticResource FormatMenuItemHeaderConverter}, ConverterParameter='Save {0} _As...'}" />
    // NOTE: Yes, I am aware of HeaderStringFormat, but it a) crashes if Header is null, b) underscores are not converted to access keys.
    public class FormatMenuItemHeaderConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return String.Format(parameter as String, value as String);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
