using Quick;
using System;
using System.Collections.ObjectModel;

namespace MaterialThemeDemo.ViewModel
{
    public class DataGridDemoViewModel : QEditBindableBase
    {
        public DataGridDemoViewModel()
        {
            Students = new ObservableCollection<StudentAdvancedItem>();
            for (int i = 1; i <= 10; i++)
            {
                StudentAdvancedItem student = new StudentAdvancedItem();
                student.Name = $"李明{i}";
                student.GenderCode = i % 1 == 0 ? GenderType.Men : GenderType.Women;
                student.Age = i * 10;
                student.Weight = 91.23 + (double)i;
                student.DateOfBirth = DateTime.Now + TimeSpan.FromDays(i);
                Students.Add(student);
            }
        }

        [QTime(Title = "上学时间", GroupHeader = "Basic Info")]
        [QRequired]
        public DateTime? Time2 { get; set; }

        [QLabel]
        public string Race { get; set; } = "汉族";

        [QLabel]
        public string Province { get; set; } = "北京市";

        [QDataGrid(MaxHeight = 400, UseEditBar = true,  IsReadOnly = false)]
        public ObservableCollection<StudentAdvancedItem> Students { get; set; }
    }
}
