using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shell;

namespace Quick
{
    public class QWindow : Window
    {
        #region Constructor

        public QWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }

        #endregion

        #region Private members

        private Image _image;

        #endregion

        #region Public properties

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
          "Header", typeof(object), typeof(QWindow), new FrameworkPropertyMetadata(null, null));
        public object Header
        {
            get
            {
                return (object)this.GetValue(HeaderProperty);
            }
            set
            {
                this.SetValue(HeaderProperty, value);
            }
        }


        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(
         "HeaderVisibility", typeof(Visibility), typeof(QWindow), new FrameworkPropertyMetadata(Visibility.Visible, null));
        public Visibility HeaderVisibility
        {
            get
            {
                return (Visibility)this.GetValue(HeaderVisibilityProperty);
            }
            set
            {
                this.SetValue(HeaderVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register(
              "HeaderHeight", typeof(double), typeof(QWindow),
            new FrameworkPropertyMetadata(30.0, new PropertyChangedCallback(HeaderHeightPropertyChangedCallback)));

        public static void HeaderHeightPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as QWindow).UpdateHeaderHeight();
        }

        public double HeaderHeight
        {
            get => (double)this.GetValue(HeaderHeightProperty);
            set => this.SetValue(HeaderHeightProperty, value);
        }

        public void UpdateHeaderHeight()
        {
            if (IsLoaded)
            {
                WindowChrome windowChrome = WindowChrome.GetWindowChrome(this);
                if (windowChrome != null)
                    windowChrome.CaptionHeight = HeaderHeight;
            }
        }

        #endregion

        #region Events



        #endregion

        #region Private methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _image = (Image)this.GetTemplateChild("PART_IconImage");
            _image.MouseLeftButtonDown += _image_MouseLeftButtonDown;
            _image.MouseRightButtonDown += _image_MouseLeftButtonDown;
        }

        private void _image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowSystemMenu(null, null);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            WindowChrome windowChrome = WindowChrome.GetWindowChrome(this);
            if (windowChrome != null)
                windowChrome.CaptionHeight = HeaderHeight;
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();
        }

        //点击窗口任意地方移动
        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);
        //    if (e.ButtonState == MouseButtonState.Pressed)
        //        DragMove();
        //}

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }


        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            SystemPoint point = new SystemPoint();
            WindowsApi.GetCursorPos(ref point);
            SystemCommands.ShowSystemMenu(this, new Point() { X = point.X, Y = point.Y });
        }

        #endregion
    }
}
