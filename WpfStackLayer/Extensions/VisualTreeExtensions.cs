using System;
using System.Windows;
using System.Windows.Media;

namespace WpfStackLayer
{
    /// <summary>
    /// VisualTree (UIの木構造) 用の拡張メソッド
    /// </summary>
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// 指定した型の子UIに対して指定アクションを実行します。
        /// </summary>
        /// <typeparam name="T">型 (DependencyObject)</typeparam>
        /// <param name="current">起点</param>
        /// <param name="action">アクション</param>
        public static void ActionOnChildren<T>( this DependencyObject current, Action<T> action )
            where T : DependencyObject
        {
            int countChildren = VisualTreeHelper.GetChildrenCount( current );

            for (int i = 0; i < countChildren; ++i) {
                var child = VisualTreeHelper.GetChild( current, i );
                var target = child as T;
                if (target == null)
                    child.ActionOnChildren( action );
                else
                    action( target );
            }
        }

        // Memo : Name 依存関係プロパティは FrameworkElement で定義されている
        /// <summary>
        /// 指定した名前に一致した最初の子UIを返します。
        /// </summary>
        /// <param name="current">起点</param>
        /// <param name="targetName">検索対象の名称</param>
        /// <returns>検索結果</returns>
        public static FrameworkElement FindFirstChild( this FrameworkElement current, string targetName )
        {
            int countChildren = VisualTreeHelper.GetChildrenCount( current );
 
            for (int i = 0; i < countChildren; ++i) {
                var child = VisualTreeHelper.GetChild( current, i ) as FrameworkElement;
                if (child == null)
                    continue;
 
                if (string.Compare( targetName, child.Name ) == 0)
                    return child;
 
                var foundTarget = FindFirstChild( child, targetName );
                if (foundTarget != null)
                    return foundTarget;
            }
 
            return null;
        }
    }
}
