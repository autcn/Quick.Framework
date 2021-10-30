using Newtonsoft.Json;
using Quick;
using System;
using System.Collections.Generic;

namespace LocalStorageDemo.ViewModel
{
    public class MainWindowViewModel : QValidatableBase
    {
        #region Bindable Properties

        public string Name
        {
            get => GetLs<string>(); //自动从LocalStorage中获取，如果不存在返回null
            set => SetLs(value);   //自动保存到LocalStorage
        }

        public string Address
        {
            get => GetLsOrDefault("地址"); //自动从LocalStorage中获取，如果不存在返回"地址"
            set => SetLs(value);
        }

        public int Age
        {
            get => Ls.GetValueOrDefault("My.Age", 10); //通过Ls的方法自定义存储的Key，如果不存在返回默认值10
            set => Ls["My.Age"] = value;  //通过Ls的方法保存到自定义的Key，注意：key为全局唯一
        }
        public double Weight
        {
            get => GetLs<double>();
            set => SetLs(value);
        }

        #endregion

        public void WriteToLocalStorage()
        {
            Ls["PhoneNumber"] = "10291828183";
        }
        public void ReadFromLocalStorage()
        {
            if (Ls.TryGetValue<string>("PhoneNumber", out string number))
            {
                MsgBox.Show($"电话号码是：{number}");
            }
            else
            {
                MsgBox.Show("电话号码不存在!");
            }
        }

        public void RemoveFromLocalStorage()
        {
            Ls.Remove("PhoneNumber");
        }

        public void ClearLocalStorage()
        {
            Ls.Clear();
        }

        public void SaveObject()
        {
            Student st = new Student()
            {
                Name = "李明",
                Age = 16,
                BirthDate = DateTime.Now,
                Height = 1.76,
                PhoneNumber = "15928273616",
                Scores = new Dictionary<string, int>()
                {
                    { "语文", 98},
                    { "数学", 68},
                    { "英语", 81}
                }
            };
            Ls["StudentTestKey"] = QConvert.ToBase64String(st);
        }

        public void ReadObject()
        {
            if (Ls.TryGetValue<string>("StudentTestKey", out string base64))
            {
                var student = QConvert.FromBase64String<Student>(base64);
                MsgBox.Show(JsonConvert.SerializeObject(student, Formatting.Indented));
            }
            else
            {
                MsgBox.Show("学生不存在!");
            }
        }
    }

    public class Student
    {
        public Student()
        {
            Scores = new Dictionary<string, int>();
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public double Height { get; set; }
        public string PhoneNumber { get; set; }
        public Dictionary<string, int> Scores { get; set; }
    }
}
