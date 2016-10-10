using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfStackLayer.Converters
{
    /// <summary>
    /// Enum用のコンバータ
    /// </summary>
    /// <remarks>継承して利用されることを想定</remarks>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    public class AToBConverter<A, B> : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return (B)value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return (A)value;
        }
    }
}
