using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Task_Manager.Models;

namespace Task_Manager.Converters
{
    public class ImportanceToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskImportance importance)
            {
                switch (importance)
                {
                    case TaskImportance.Low:
                        return Brushes.LightGreen;
                    case TaskImportance.Medium:
                        return Brushes.Orange;
                    case TaskImportance.High:
                        return Brushes.Red;
                    case TaskImportance.Critical:
                        return Brushes.DarkRed;
                }
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
