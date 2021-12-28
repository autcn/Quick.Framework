using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Quick
{
    [TemplatePart(Name = ElementScorllViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = ElementItemsPresenter, Type = typeof(ItemsPresenter))]
    [TemplatePart(Name = ElementColumnHeadersPresenter, Type = typeof(DataGridColumnHeadersPresenter))]
    public class QDataGrid : DataGrid, IEditableControl
    {
        #region Constants
        private const string ElementScorllViewer = "DG_ScrollViewer";
        private const string ElementItemsPresenter = "PART_ItemsPresenter";
        private const string ElementColumnHeadersPresenter = "PART_ColumnHeadersPresenter";
        #endregion

        #region Constructor
        public QDataGrid()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _localization = QServiceProvider.GetService<ILocalization>();
                _messageBox = QServiceProvider.GetService<IMessageBox>();
            }
        }
        #endregion

        #region Private members
        private ScrollViewer _scrollViewer;
        private ItemsPresenter _itemsPre;
        private ILocalization _localization;
        private IMessageBox _messageBox;
        private Type _modelType;
        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty DeleteWarningProperty =
        DependencyProperty.Register("DeleteWarning", typeof(bool), typeof(QDataGrid), new FrameworkPropertyMetadata(true, null));

        public bool DeleteWarning
        {
            get
            {
                return (bool)this.GetValue(DeleteWarningProperty);
            }
            set
            {
                this.SetValue(DeleteWarningProperty, value);
            }
        }

        public static readonly DependencyProperty DeleteKeepSelectionProperty =
        DependencyProperty.Register("DeleteKeepSelection", typeof(bool), typeof(QDataGrid), new FrameworkPropertyMetadata(false, null));

        public bool DeleteKeepSelection
        {
            get
            {
                return (bool)this.GetValue(DeleteKeepSelectionProperty);
            }
            set
            {
                this.SetValue(DeleteKeepSelectionProperty, value);
            }
        }

        #endregion

        #region Private functions
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (newValue == null)
            {
                _modelType = null;
                return;
            }
            Type type = ItemsSource.GetType();
            if (type.GenericTypeArguments.Length == 0)
            {
                return;
            }
            _modelType = type.GenericTypeArguments[0];
            //if (!_modelType.IsSubclassOf(typeof(EditBase)))
            //{
            //    throw new Exception("The generic type must be EditBase");
            //}
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = (ScrollViewer)GetTemplateChild(ElementScorllViewer);
            _itemsPre = (ItemsPresenter)GetTemplateChild(ElementItemsPresenter);
            _scrollViewer.Loaded += _viewer_Loaded;
        }
        private string GetTitle<TAttribute>(QEditContext<TAttribute> editContext) where TAttribute : QEditAttribute, new()
        {
            string title = editContext.Attr.Title;
            if (title != null)
            {
                title = title.TrimEnd(':', '：');
            }
            return title;
        }

        private void SetColumnHeader(DataGridColumn newCol, QEditContext editContext)
        {
            QEditAttribute editAttr = editContext.GetAttr();


            TextBlock headerTbk = new TextBlock();
            string normalText = editAttr.Title.StrongTextToNormal();
            if (editAttr.Title.IsResourceKey())
            {
                headerTbk.SetResourceReference(TextBlock.TextProperty, normalText);
            }
            else if (editAttr.Title != null)
            {
                headerTbk.Text = normalText;
            }
            else
            {
                headerTbk.Text = editContext.PropertyName;
            }

            headerTbk.VerticalAlignment = VerticalAlignment.Center;

            //headerTbk.Background = Brushes.Red;
            newCol.Header = headerTbk;

            if (newCol is DataGridTemplateColumn templateColumn)
            {
                templateColumn.CanUserSort = true;
                templateColumn.SortMemberPath = editContext.PropertyName;
            }


            if (editAttr.DataGridColumnAlignment == TextAlignment.Left)
            {
                newCol.HeaderStyle = (Style)this.FindResource(StyleKeysProperties.QDataGridColumnHeaderLeftStyleKey);
            }
            else if (editAttr.DataGridColumnAlignment == TextAlignment.Right)
            {
                newCol.HeaderStyle = (Style)this.FindResource(StyleKeysProperties.QDataGridColumnHeaderRightStyleKey);
            }
            else
            {
                newCol.HeaderStyle = (Style)this.FindResource(StyleKeysProperties.QDataGridColumnHeaderCenterStyleKey);
            }
            //ToggleButton button = new ToggleButton();
            //if (!CanUserSortColumns)
            //{
            //    button.IsHitTestVisible = false;
            //}
            //button.Style = (Style)this.TryFindResource("dgSortHeaderBtnStyle");
            //if ()
            //{
            //    button.SetResourceReference(Button.ContentProperty, unItem.Attr.Title.GetResourceName());
            //}
            //else
            //{
            //    string title = GetTitle(unItem);
            //    button.Content = title;
            //}

            //button.Tag = unItem.PropertyName;
            //if (unItem.Attr is QComboBoxAttribute unCbxAttr)
            //{
            //    if (unCbxAttr.CbxBindTarget == CbxBindTarget.Item)
            //    {
            //        button.Tag = $"{unItem.PropertyName}.{unCbxAttr.DisplayMemberPath}";
            //    }
            //}

            //ContextMenu menu = new ContextMenu();
            //MenuItem mi = new MenuItem();
            //mi.Header = "Clear Sorting";
            //mi.Click += (sender, e) =>
            //{
            //    if (!CanUserSortColumns)
            //    {
            //        return;
            //    }
            //    if (_isEditing)
            //    {
            //        CancelEdit();
            //        return;
            //    }
            //    CancelEdit();
            //    ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
            //    view.SortDescriptions.Clear();
            //    foreach (DataGridColumn col in Columns)
            //    {
            //        ToggleButton tempBtn = col.Header as ToggleButton;
            //        tempBtn.IsChecked = null;
            //    }
            //};
            //menu.Items.Add(mi);
            //button.ContextMenu = menu;
            //button.PreviewMouseLeftButtonDown += (sender, e) =>
            //{
            //    if (!CanUserSortColumns)
            //    {
            //        return;
            //    }
            //    if (_isEditing)
            //    {
            //        CancelEdit();
            //        //e.Handled = true;
            //    }
            //};
            //button.Click += (sender, e) =>
            //{
            //    if (!CanUserSortColumns)
            //    {
            //        return;
            //    }
            //    CancelEdit();
            //    ToggleButton clickButton = sender as ToggleButton;
            //    foreach (DataGridColumn col in Columns)
            //    {
            //        ToggleButton tempBtn = col.Header as ToggleButton;
            //        if (tempBtn != clickButton)
            //        {
            //            tempBtn.IsChecked = null;
            //        }
            //    }
            //    ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
            //    view.SortDescriptions.Clear();
            //    view.SortDescriptions.Add(new SortDescription((string)clickButton.Tag, clickButton.IsChecked.Value ? ListSortDirection.Descending : ListSortDirection.Ascending));
            //};

            //newCol.Header = button;
        }

        private void SetColumnWidth(DataGridColumn column, string strWidth)
        {
            if (strWidth != null)
            {
                if (strWidth.Contains("*"))
                {
                    strWidth = strWidth.Trim().Replace("*", "");
                    if (strWidth == "")
                    {
                        strWidth = "1";
                    }
                    column.Width = new DataGridLength(Convert.ToDouble(strWidth), DataGridLengthUnitType.Star);
                }
                else
                {
                    column.Width = new DataGridLength(Convert.ToDouble(strWidth), DataGridLengthUnitType.Pixel);
                }
            }
        }

        private QEditContext EditAttributeRedist(QEditContext editContext)
        {
            QEditContext toContext = null;
            QEditAttribute editAttr = editContext.GetAttr();
            if (editAttr is QRadioSelectorAttribute radioAttr)
            {
                QEditContext<QComboBoxAttribute> newContext = new QEditContext<QComboBoxAttribute>();
                newContext.Import(editContext);

                QComboBoxAttribute toAttr = newContext.Attr;
                toAttr.ItemsSourcePath = radioAttr.ItemsSourcePath;
                toAttr.SelectedValuePath = radioAttr.SelectedValuePath;
                toAttr.DisplayMemberPath = radioAttr.ContentMemberPath;
                toAttr.ItemsSourcePath = radioAttr.ItemsSourcePath;
                toAttr.BindType = radioAttr.BindType == RadioSelectorBindType.Item ? ComboBoxBindType.Item : ComboBoxBindType.Value;

                toContext = newContext;
            }
            else if (editAttr is QEnumRadioSelectorAttribute enumRadioAttr)
            {
                QEditContext<QEnumComboBoxAttribute> newContext = new QEditContext<QEnumComboBoxAttribute>();
                newContext.Import(editContext);
                toContext = newContext;
            }
            else if (editAttr is QToggleButtonAttribute)
            {
                QEditContext<QCheckBoxAttribute> newContext = new QEditContext<QCheckBoxAttribute>();
                newContext.Import(editContext);
                toContext = newContext;
            }
            return toContext ?? editContext;
        }

        protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs evt)
        {
            PropertyDescriptor propertyDesc = evt.PropertyDescriptor as PropertyDescriptor;
            QEditContext editContext = null;
            foreach (Attribute tempAttr in propertyDesc.Attributes)
            {
                if (tempAttr is QEditAttribute qEditAttr
                   && !(tempAttr is QDataGridAttribute))
                {
                    editContext = QEditContext.CreateGeneric(tempAttr.GetType());
                    editContext.PropertyName = evt.PropertyName;
                    editContext.PropertyType = evt.PropertyType;
                    editContext.SetAttr(qEditAttr);
                    if (qEditAttr.Place != QEditPlaces.DataGrid && qEditAttr.Place != QEditPlaces.All)
                    {
                        evt.Cancel = true;
                        return;
                    }
                    break;
                }
            }

            if (editContext == null)
            {
                evt.Cancel = true;
                return;
            }

            editContext = EditAttributeRedist(editContext);

            QEditAttribute editAttr = editContext.GetAttr();
            //获取创建器
            Type creatorType = typeof(IQEditControlCreator<>).MakeGenericType(editAttr.GetType());
            object creator = QServiceProvider.GetService(creatorType);
            MethodInfo createFun = creatorType.GetMethod(nameof(IQEditControlCreator<QEditAttribute>.CreateDataGridColumn));
            DataGridColumn newCol = (DataGridColumn)createFun.Invoke(creator, new object[] { this, editContext });

            SetColumnWidth(newCol, editAttr.Width);
            SetColumnHeader(newCol, editContext);

            evt.Column = newCol;
        }

        #endregion

        #region Events
        #region Events
        public event Action<QEditControl> OnConfigEditControl;
        #endregion
        #endregion

        #region Edit interface
        public void Insert()
        {
            if (_modelType == null)
            {
                return;
            }
            object toAddModel = null;
            if (SelectedItem != null && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                toAddModel = SelectedItem.CloneWithProperties();
            }
            else
            {
                toAddModel = QEditObjectHelper.CreateObject(_modelType);
            }

            //QEditWindow wnd = new QEditWindow(toAddModel);
            var editContext = QEditWindowHelper.Create(toAddModel);
            editContext.Content.OnConfigEditControl += ctrl => OnConfigEditControl?.Invoke(ctrl);
            editContext.Content.OkAgain += Wnd_OKAgain;
            editContext.Content.UseOKAndAgain = true;
            editContext.Content.IsEditMode = false;
            if (editContext.Window.ShowDialog() == true)
            {
                (ItemsSource as IList).Add(toAddModel);
            }
        }

        private void Wnd_OKAgain(object sender, EventArgs args)
        {
            (ItemsSource as IList).Add((sender as QEditWindowContent).EditVM);
        }

        public void Update()
        {
            if (_modelType == null || SelectedItem == null)
            {
                return;
            }

            object editVM = SelectedItem.CloneWithProperties();
            if (editVM is IEditableObject editObj)
            {
                editObj.IsEditMode = true;
            }
            //QEditWindow wnd = new QEditWindow(editVM);
            var editContext = QEditWindowHelper.Create(editVM);
            editContext.Content.OnConfigEditControl += ctrl => OnConfigEditControl?.Invoke(ctrl);
            editContext.Content.UseOKAndAgain = false;
            editContext.Content.IsEditMode = true;
            if (editContext.Window.ShowDialog() == true)
            {
                editContext.Content.EditVM.CopyPropertiesTo(SelectedItem);
            }
        }

        public void Delete()
        {
            if (SelectedItem == null)
            {
                return;
            }
            if (DeleteWarning && !Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (_messageBox.QuestionOKCancel(QLocalizationProperties.StQEditWndDeleteWarning) == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            object vmObj = SelectedItem;
            try
            {
                if (vmObj is IEditableObject editObj)
                {
                    editObj.DeleteAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                _messageBox.Show(ex.Message);
                return;
            }
            IList dataSource = (ItemsSource as IList);
            int selIndex = SelectedIndex;
            dataSource.RemoveAt(selIndex);
            if (DeleteKeepSelection)
            {
                if (selIndex <= dataSource.Count - 1)
                {
                    SelectedIndex = selIndex;
                }
                else if (dataSource.Count > 0)
                {
                    SelectedIndex = dataSource.Count - 1;
                }
            }
        }
        #endregion

        private void _viewer_Loaded(object sender, RoutedEventArgs e)
        {
            //_columns = (DataGridColumnHeadersPresenter)_scrollViewer.Template.FindName(ElementColumnHeadersPresenter, _scrollViewer);
            //_columns.SizeChanged += _columns_SizeChanged;
            //_itemsPre.Width = _columns.ActualWidth;
        }

        private void _columns_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //_itemsPre.Width = (sender as FrameworkElement).ActualWidth;
        }

        private IEditableObject _lastEditBase;
        protected override void OnRowEditEnding(DataGridRowEditEndingEventArgs e)
        {
            base.OnRowEditEnding(e);
            _lastEditBase = e.Row.DataContext as IEditableObject;
        }

        protected override void OnExecutedCommitEdit(ExecutedRoutedEventArgs e)
        {
            base.OnExecutedCommitEdit(e);
            if (_lastEditBase != null)
            {
                _lastEditBase.IsEditMode = true;
                _lastEditBase.SubmitAsync().Wait();
                _lastEditBase = null;
            }
        }

        //protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
        //{
        //    base.OnCellEditEnding(e);
        //    _isEditing = false;
        //}
        //protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
        //{
        //    base.OnBeginningEdit(e);
        //    _isEditing = true;
        //}


    }
}
