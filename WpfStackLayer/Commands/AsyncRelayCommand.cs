using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfStackLayer
{
    /// <summary>
    /// 指定した CanExecute() と Execute() をリレーするコマンド (非同期版)
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        #pragma warning disable 0067
        /// <summary>
        /// 実行可能かどうかが変化したタイミングで発生するイベント (使用しないで下さい)
        /// </summary>
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 0067

        Func<object, bool> canExecuteFunc;
        Func<object, Task> executeAsyncFunc;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="canExecuteFunc">実行可能かどうかを判定するFunc</param>
        /// <param name="executeAsyncFunc">実行アクション (非同期)</param>
        public AsyncRelayCommand( Func<object, bool> canExecuteFunc, Func<object, Task> executeAsyncFunc )
        {
            this.canExecuteFunc = canExecuteFunc;
            this.executeAsyncFunc = executeAsyncFunc;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executeAsyncFunc">実行アクション (非同期)</param>
        public AsyncRelayCommand( Func<object, Task> executeAsyncFunc ) : this( parameter => true, executeAsyncFunc )
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executeAsyncTask">実行アクション (非同期)</param>
        public AsyncRelayCommand( Task executeAsyncTask ) : this( parameter => executeAsyncTask )
        {
        }

        /// <summary>
        /// 実行可能かどうかを判定します
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns>true: 実行可能，false: 実行不可能</returns>
        public bool CanExecute( object parameter ) => canExecuteFunc( parameter );

        /// <summary>
        /// コマンドを非同期に実行します
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        public async void Execute( object parameter ) => await executeAsyncFunc( parameter );
    }
}
