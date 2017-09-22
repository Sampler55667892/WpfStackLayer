using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfStackLayer
{
    /// <summary>
    /// コマンドホルダー
    /// </summary>
    public partial class CommandHolder : UserControl
    {
        /// <summary>
        /// コマンド依存関係プロパティ
        /// </summary>
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached( "Command",
                typeof(ICommand),
                typeof(CommandHolder),
                new PropertyMetadata( null ) );

        /// <summary>
        /// コマンドパラメータ依存関係プロパティ
        /// </summary>
        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached( "CommandProperty",
                typeof(object),
                typeof(CommandHolder),
                new PropertyMetadata( null ) );

        /// <summary>
        /// コマンド
        /// </summary>
        public ICommand Command
        {
            get { return GetValue( CommandProperty ) as ICommand; }
            set { SetValue( CommandProperty, value ); }
        }

        /// <summary>
        /// コマンドパラメータ
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue( CommandParameterProperty ); }
            set { SetValue( CommandParameterProperty, value ); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandHolder()
        {
            InitializeComponent();
            Loaded += CommandHolder_Loaded;
        }

        void CommandHolder_Loaded( object sender, RoutedEventArgs e )
        {
            Loaded -= CommandHolder_Loaded;
            SetupHandler();
        }

        void SetupHandler()
        {
            if (Content is FrameworkElement view)
                view.PreviewMouseLeftButtonDown += View_PreviewMouseLeftButtonDown;
        }

        /// <summary>
        /// (強参照などを) 破棄します
        /// </summary>
        public void Dispose()
        {
            if (Content is FrameworkElement view)
                view.PreviewMouseLeftButtonDown -= View_PreviewMouseLeftButtonDown;
        }

        void View_PreviewMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            Command?.Execute( CommandParameter );
            e.Handled = true;
        }
    }
}
