using System;
using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    /// <summary>
    /// QEditControl.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = ElementAddButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementEditButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementDeleteButton, Type = typeof(Button))]
    public class QEditBar : Control
    {
        #region Constants
        private const string ElementAddButton = "PART_AddButton";
        private const string ElementEditButton = "PART_UpdateButton";
        private const string ElementDeleteButton = "PART_DeleteButton";
        #endregion

        public static readonly RoutedEvent AddRoutedEvent =
        EventManager.RegisterRoutedEvent(nameof(Add), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(QEditBar));

        public event RoutedEventHandler Add
        {
            add { this.AddHandler(AddRoutedEvent, value); }
            remove { this.RemoveHandler(AddRoutedEvent, value); }
        }

        public static readonly RoutedEvent UpdateRoutedEvent =
        EventManager.RegisterRoutedEvent(nameof(Update), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(QEditBar));

        public event RoutedEventHandler Update
        {
            add { this.AddHandler(UpdateRoutedEvent, value); }
            remove { this.RemoveHandler(UpdateRoutedEvent, value); }
        }

        public static readonly RoutedEvent DeleteRoutedEvent =
        EventManager.RegisterRoutedEvent(nameof(Delete), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(QEditBar));

        public event RoutedEventHandler Delete
        {
            add { this.AddHandler(DeleteRoutedEvent, value); }
            remove { this.RemoveHandler(DeleteRoutedEvent, value); }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            EditableTarget?.Insert();
            this.RaiseEvent(new RoutedEventArgs(AddRoutedEvent, this));
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            EditableTarget?.Update();
            this.RaiseEvent(new RoutedEventArgs(UpdateRoutedEvent, this));
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            EditableTarget?.Delete();
            this.RaiseEvent(new RoutedEventArgs(DeleteRoutedEvent, this));
        }

        public static readonly DependencyProperty EditModeProperty = DependencyProperty.Register(
          "EditMode", typeof(QEditBarEditMode), typeof(QEditBar),
        new FrameworkPropertyMetadata(QEditBarEditMode.Add | QEditBarEditMode.Update | QEditBarEditMode.Delete, new PropertyChangedCallback(EditModePropertyChangedCallback)));

        public static void EditModePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as QEditBar).UpdateEditMode();
        }

        public QEditBarEditMode EditMode
        {
            get => (QEditBarEditMode)this.GetValue(EditModeProperty);
            set => this.SetValue(EditModeProperty, value);
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
         "Content", typeof(object), typeof(QEditBar), new FrameworkPropertyMetadata(null, null) );

        public object Content
        {
            get => this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        private Button _btnAdd;
        private Button _btnUpdate;
        private Button _btnDelete;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _btnAdd = (Button)GetTemplateChild(ElementAddButton);
            _btnUpdate = (Button)GetTemplateChild(ElementEditButton);
            _btnDelete = (Button)GetTemplateChild(ElementDeleteButton);
            if (_btnAdd == null)
            {
                return;
            }

            _btnAdd.Click += ButtonAdd_Click;
            _btnUpdate.Click += ButtonUpdate_Click;
            _btnDelete.Click += ButtonDelete_Click;

            UpdateEditMode();
        }

        public void UpdateEditMode()
        {
            if(_btnAdd == null)
            {
                return;
            }
            _btnAdd.Visibility = EditMode.HasFlag(QEditBarEditMode.Add) ? Visibility.Visible : Visibility.Collapsed;
            _btnUpdate.Visibility = EditMode.HasFlag(QEditBarEditMode.Update) ? Visibility.Visible : Visibility.Collapsed;
            _btnDelete.Visibility = EditMode.HasFlag(QEditBarEditMode.Delete) ? Visibility.Visible : Visibility.Collapsed;
        }

        public IEditableControl EditableTarget { get; set; }
    }

    [Flags]
    public enum QEditBarEditMode : int
    {
        Add = 1,
        Update = 2,
        Delete = 4
    }
}
