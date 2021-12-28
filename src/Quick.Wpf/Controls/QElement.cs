using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Controls
{
    public class QElement : SimplePanel
    {
        public static readonly DependencyProperty ContentStyleProperty = DependencyProperty.Register(
            "ContentStyle", typeof(Style), typeof(QElement),
                new FrameworkPropertyMetadata(null, ContentStyleChangedCallBack));

        public Style ContentStyle
        {
            get => (Style)GetValue(ContentStyleProperty);
            set => SetValue(ContentStyleProperty, value);
        }

        public static void ContentStyleChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as QElement).UpdateStyle();
        }

        public void UpdateStyle()
        {
            if (_mainElement != null)
            {
                _mainElement.Style = ContentStyle;
            }
        }

        private static Brush _designBrush;
        public QElement()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.MinWidth = 30;
                if (_designBrush == null)
                {
                    UniformGrid grid = new UniformGrid();
                    grid.Rows = 2;
                    grid.Columns = 2;
                    grid.Width = 64;
                    grid.Height = 64;
                    grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(0xD0, 0xD0, 0xD0)) });
                    grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)) });
                    grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)) });
                    grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromRgb(0xD0, 0xD0, 0xD0)) });
                    VisualBrush vBrush = new VisualBrush(grid);
                    vBrush.Viewport = new Rect(0, 0, 16, 16);
                    vBrush.ViewportUnits = BrushMappingMode.Absolute;
                    vBrush.TileMode = TileMode.Tile;
                    _designBrush = vBrush;
                }

                this.Background = _designBrush;
            }
        }

        public void SetContent(FrameworkElement layoutElement, FrameworkElement mainElement)
        {
            _mainElement = mainElement;
            if(ContentStyle != null)
            {
                _mainElement.Style = ContentStyle;
            }
            
            Children.Clear();
            Children.Add(layoutElement);
        }

        private FrameworkElement _mainElement;

        public FrameworkElement Content
        {
            get => _mainElement;
        }
    }
}
