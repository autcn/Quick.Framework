using Quick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickThemeDemo.ViewModel
{
    public class ComboBoxDemoViewModel : QEditBindableBase
    {
        public ComboBoxDemoViewModel()
        {
            StatusList = new ObservableCollection<StatusItem>();
            StatusList.Add(new StatusItem { Code = "UnRegistered", Name = "未注册" });
            StatusList.Add(new StatusItem { Code = "Registered", Name = "已注册" });
            StatusList.Add(new StatusItem { Code = "Disabled", Name = "禁用" });

            LocationList = new ObservableCollection<string>();
            LocationList.Add("前面");
            LocationList.Add("中部");
            LocationList.Add("后面");

            AutoCompleteDataSource = new ObservableCollection<string>();
            AutoCompleteDataSource.Add("TOLEIKIS Laurynas");
            AutoCompleteDataSource.Add("RAGAINYTE Enrika");
            AutoCompleteDataSource.Add("VILDE Edvins");
            AutoCompleteDataSource.Add("KACHANAVA Marharyta");
            AutoCompleteDataSource.Add("HRYNKO Viktoriia");
            AutoCompleteDataSource.Add("MUCHAROVA Yevheniia");
            AutoCompleteDataSource.Add("张三丰");
            AutoCompleteDataSource.Add("赵四");
            AutoCompleteDataSource.Add("北雁云依");

            SelItem = StatusList[1];
        }

        #region 数据源

        /// <summary>
        /// 为自动完成下拉框提供数据源
        /// </summary>
        public ObservableCollection<string> AutoCompleteDataSource { get; set; }

        /// <summary>
        /// 为下拉框提供数据
        /// </summary>
        public ObservableCollection<StatusItem> StatusList { get; set; }

        /// <summary>
        /// 简单数据源
        /// </summary>
        public ObservableCollection<string> LocationList { get; set; }

        #endregion

        /// <summary>
        /// 绑定简单String列表
        /// </summary>
        [QComboBox(Title = "位置", ItemsSourcePath = nameof(LocationList))]
        public string Location { get; set; }


        /// <summary>
        /// 绑定简单String列表，不可编辑
        /// </summary>
        [QComboBox(Title = "位置2", ItemsSourcePath = nameof(LocationList), IsReadOnly = true)]
        public string Location2 { get; set; }

        /// <summary>
        /// 绑定对象集合，绑定Value
        /// </summary>
        [QComboBox(ItemsSourcePath = nameof(StatusList), BindType = ComboBoxBindType.Value, IsReadOnly = true,
            DisplayMemberPath = nameof(StatusItem.Name), SelectedValuePath = nameof(StatusItem.Code))]
        public string StatusCode { get; set; } = "UnRegistered";

        /// <summary>
        /// 绑定对象集合，绑定SelectedItem
        /// </summary>
        [QComboBox(ItemsSourcePath = nameof(StatusList), BindType = ComboBoxBindType.Item, IsReadOnly = true,
            DisplayMemberPath = nameof(StatusItem.Name))]
        public StatusItem SelectedStatus { get; set; }

        /// <summary>
        /// 绑定对象集合，绑定Text
        /// </summary>
        [QComboBox(ItemsSourcePath = nameof(StatusList), BindType = ComboBoxBindType.Text,
            DisplayMemberPath = nameof(StatusItem.Name), SelectedValuePath = nameof(StatusItem.Code))]
        [QRequired]
        public string StatusText { get; set; }

        /// <summary>
        /// 自动完成，Item集合,Value绑定
        /// </summary>
        [QAutoCompleteComboBox(ItemsSourcePath = nameof(StatusList), DisplayMemberPath = nameof(StatusItem.Name),
           FilterMemberPath = nameof(StatusItem.Name), SelectedValuePath = nameof(StatusItem.Code), BindType = ComboBoxBindType.Value,
            CanDropDown = true)]
        [QRequired]
        public string StatusCode5 { get; set; } = "Disabled";

        /// <summary>
        /// 自动完成，Item集合,Item绑定
        /// </summary>
        [QAutoCompleteComboBox(ItemsSourcePath = nameof(StatusList), DisplayMemberPath = nameof(StatusItem.Name), FilterMemberPath = nameof(StatusItem.Name),
            CanDropDown = true)]
        [QRequired]
        public StatusItem SelItem { get; set; }

        /// <summary>
        /// 自动完成，Item集合,Item绑定,不可下拉选择，只能搜索
        /// </summary>
        [QAutoCompleteComboBox(ItemsSourcePath = nameof(StatusList), DisplayMemberPath = nameof(StatusItem.Name), FilterMemberPath = nameof(StatusItem.Name),
            CanDropDown = false)]
        [QRequired]
        public StatusItem SelItem2 { get; set; }

        /// <summary>
        /// 自动完成，string集合, Item绑定
        /// </summary>
        [QAutoCompleteComboBox(ItemsSourcePath = nameof(AutoCompleteDataSource), CanDropDown = true)]
        [QRequired]
        public string Friend { get; set; } = "张三丰";


        ///// <summary>
        ///// 直接绑定string集合
        ///// </summary>
        //[QRadioSelector(ItemsSourcePath = nameof(LocationList))]
        //public string StatusCode2 { get; set; } = "中部";


        ///// <summary>
        ///// 绑定对象集合，绑定对象
        ///// </summary>
        //[QRadioSelector(ItemsSourcePath = nameof(StatusList), ContentMemberPath = nameof(StatusItem.Name))]
        //public StatusItem Status { get; set; }

        ///// <summary>
        ///// 绑定对象集合，绑定值
        ///// </summary>
        //[QRadioSelector(ItemsSourcePath = nameof(StatusList), ContentMemberPath = nameof(StatusItem.Name),
        //                  BindType = RadioSelectorBindType.Value, SelectedValuePath = nameof(StatusItem.Code))]
        //public string StatusCode3 { get; set; } = "Disabled";
    }
}
