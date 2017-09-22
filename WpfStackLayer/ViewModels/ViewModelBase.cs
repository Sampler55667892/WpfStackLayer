using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace WpfStackLayer
{
    /// <summary>
    /// ViewModelのベース
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 依存関係プロパティが変更されたタイミングで発生するイベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 値の更新とPropertyChangedイベントの発行を行います (Prismの SetProperty() と同様)。
        /// </summary>
        /// <typeparam name="T">プロパティの型</typeparam>
        /// <param name="currentValue">現在の値</param>
        /// <param name="newValue">新しい値</param>
        public void RaisePropertyChanged<T>( ref T currentValue, T newValue )
        {
            var isValueChanged =
                (currentValue != null && newValue == null) ||
                (currentValue == null && newValue != null) ||
                !currentValue.Equals( newValue );

            if (isValueChanged) {
                currentValue = newValue;
                // 呼出し元のメソッド名を取得 (http://d.hatena.ne.jp/hikaruright/20120820/1345454949)
                var callerFrame = new StackFrame( 1 );
                var methodName = callerFrame.GetMethod().Name;
                if (methodName.StartsWith( SetPropertyPrefix ))
                    methodName = methodName.Replace( SetPropertyPrefix, string.Empty );
                RaisePropertyChanged( methodName );
            }
        }

        // https://blogs.msdn.microsoft.com/csharpfaq/2010/03/11/how-can-i-get-objects-and-property-values-from-expression-trees/
        /// <summary>
        /// PropertyChangedイベントの発行を行います (Livetが提供するものと同様)。
        /// </summary>
        /// <typeparam name="T">プロパティの型</typeparam>
        /// <param name="Property">プロパティのラムダ式</param>
        public void RaisePropertyChanged<T>( Expression<Func<T>> Property )
        {
            var propertyName = ((MemberExpression)Property.Body).Member.Name;
            RaisePropertyChanged( propertyName );
        }

        void RaisePropertyChanged( string propertyName )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        const string SetPropertyPrefix = "set_";
    }
}
