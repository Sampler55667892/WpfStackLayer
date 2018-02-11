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
        /// <summary>
        /// 変換処理を実行します
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="targetType">ターゲットの型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="culture">カルチャー</param>
        /// <returns>変換結果</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) => (B)value;

        /// <summary>
        /// 逆変換処理を実行します
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="targetType">ターゲットの型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="culture">カルチャー</param>
        /// <returns>逆変換結果</returns>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => (A)value;
    }
}
