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

        /// <summary>
        /// 指定条件を充たした最初の子UIを返します。
        /// </summary>
        /// <typeparam name="T">型 (DependencyObject)</typeparam>
        /// <param name="current">起点</param>
        /// <param name="predicate">検索条件</param>
        /// <returns>検索結果</returns>
        public static T FindFirstChild<T>( this DependencyObject current, Predicate<T> predicate )
            where T : DependencyObject
        {
            int countChildren = VisualTreeHelper.GetChildrenCount( current );

            for (var i = 0; i < countChildren; ++i) {
                var child = VisualTreeHelper.GetChild( current, i );
                if (child is T target && predicate( target ))
                    return target;
                var found = FindFirstChild( child, predicate );
                if (found != null)
                    return found;
            }

            return null;
        }

        /// <summary>
        /// (無条件で) 最初の子UIを返します。
        /// </summary>
        /// <typeparam name="T">型 (DependencyObject)</typeparam>
        /// <param name="current">起点</param>
        /// <returns>検索結果</returns>
        public static T FindFirstChild<T>( this DependencyObject current )
            where T : DependencyObject =>
            current.FindFirstChild( new Predicate<T>( x => true ) );

        // Memo: Name 依存関係プロパティは FrameworkElement で定義されている
        /// <summary>
        /// 指定した名前に一致した最初の子UIを返します。
        /// </summary>
        /// <param name="current">起点</param>
        /// <param name="targetName">検索対象の名称</param>
        /// <returns>検索結果</returns>
        public static FrameworkElement FindFirstChild( this FrameworkElement current, string targetName ) =>
            current.FindFirstChild( new Predicate<FrameworkElement>( x => string.Compare( x.Name, targetName ) == 0 ) );

        /// <summary>
        /// 自分が指定したUIオブジェクトの (1世代以上上の) 親であるかどうかを判定します。
        /// </summary>
        /// <param name="current">起点</param>
        /// <param name="view">(子であると期待される) UIオブジェクト</param>
        /// <returns>true: 自分は親、false: それ以外</returns>
        public static bool IsParentOf( this DependencyObject current, DependencyObject view ) =>
            current.FindFirstChild( new Predicate<DependencyObject>( x => x == view ) ) != null;
    }
}
