namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    // <vurdalakov:ZeroToFalseConverter x:Key="ZeroToFalseConverter" />
    // <MenuItem Header="Open _Recent" ItemsSource="{Binding RecentFilesMenuItems}" IsEnabled="{Binding RecentFilesMenuItems.Count, Converter={StaticResource ZeroToFalseConverter}}" />
    public class ZeroToFalseConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (Int32)value != 0;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return ((Boolean)value) ? 1 : 0;
        }
    }
}
