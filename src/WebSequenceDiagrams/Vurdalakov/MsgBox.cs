namespace Vurdalakov
{
    using System;
    using System.Windows;

    public static class MsgBox
    {
        public static void Info(String format, params Object[] parameters)
        {
            Show(MessageBoxImage.Information, MessageBoxButton.OK, format, parameters);
        }

        public static void Warning(String format, params Object[] parameters)
        {
            Show(MessageBoxImage.Warning, MessageBoxButton.OK, format, parameters);
        }

        public static void Error(String format, params Object[] parameters)
        {
            Show(MessageBoxImage.Error, MessageBoxButton.OK, format, parameters);
        }

        public static void Error(Exception ex, String format, params Object[] parameters)
        {
            var message = String.Format(format, parameters);
            Show(MessageBoxImage.Error, MessageBoxButton.OK, "{0}\n\n{1}", message, ex.Message);
        }

        public static Boolean YesNo(String format, params Object[] parameters)
        {
            return MessageBoxResult.Yes == Show(MessageBoxImage.Question, MessageBoxButton.YesNoCancel, format, parameters);
        }

        private static MessageBoxResult Show(MessageBoxImage icon, MessageBoxButton buttons, String format, params Object[] parameters)
        {
            var mainWindow = Application.Current.MainWindow;
            var message = String.Format(format, parameters);
            return MessageBox.Show(mainWindow, message, mainWindow.Title, buttons, icon);
        }
    }
}
