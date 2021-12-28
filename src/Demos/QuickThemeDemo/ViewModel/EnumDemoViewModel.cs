using Quick;
using System;
using System.Collections.ObjectModel;

namespace QuickThemeDemo.ViewModel
{
    public class EnumDemoViewModel : QEditBindableBase
    {
        /// <summary>
        /// 下拉框
        /// </summary>
        [QEnumComboBox]
        public OccupationType Occupation { get; set; }


        /// <summary>
        /// 下拉框，可以为空
        /// </summary>
        [QEnumComboBox]
        public OccupationType? Occupation2 { get; set; }


        /// <summary>
        /// 下拉框，可以为空
        /// </summary>
        [QComboBox(ItemsSourcePath = nameof(OccupationList), SelectedValuePath = nameof(EnumItemViewModel.EnumValue),
            DisplayMemberPath = nameof(EnumItemViewModel.EnumDesc), BindType = ComboBoxBindType.Value)]
        public OccupationType? Occupation5 { get; set; }

        /// <summary>
        /// 构造新的集合，过滤掉某项
        /// </summary>
        public ObservableCollection<EnumItemViewModel> OccupationList
        {
            get
            {
                var enumManager = ServiceProvider.GetService<IEnumCollectionManager>();
                return enumManager.GetEnumCollection(typeof(OccupationType?), p => (OccupationType?)p != OccupationType.Student);
            }
        }

        /// <summary>
        /// 单选框
        /// </summary>
        [QEnumRadioSelector]
        public GenderType Gender { get; set; }


        /// <summary>
        /// 单选框，初值
        /// </summary>
        [QEnumRadioSelector]
        public OccupationType Occupation3 { get; set; } = OccupationType.Teacher;

        /// <summary>
        /// 单选框，可以为空
        /// </summary>
        [QEnumRadioSelector]
        public OccupationType? Occupation4 { get; set; }
    }
}
