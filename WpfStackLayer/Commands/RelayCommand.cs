using System;
using System.Windows.Input;

namespace WpfStackLayer
{
    /// <summary>
    /// CanExecute() と Execute() を指定するコマンド
    /// </summary>
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public RelayCommand( Func<object, bool> CanExecuteFunc, Action<object> ExecuteAction )
        {
            this.canExecuteFunc = CanExecuteFunc;
            this.executeAction = ExecuteAction;
        }

        public bool CanExecute( object parameter )
        {
            return canExecuteFunc( parameter );
        }

        public void Execute( object parameter )
        {
            executeAction( parameter );
        }

        Func<object, bool> canExecuteFunc;
        Action<object> executeAction;
    }
}
