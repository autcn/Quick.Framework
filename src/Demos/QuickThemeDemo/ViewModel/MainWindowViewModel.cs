using Quick;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace QuickThemeDemo.ViewModel
{
    /*
     本框架的MVVM优势在于:
     1.采用DXEvent替代了ICommand，简化的代码
     2.使用DXBinding，以及本框架提供的QBinding，减少了95%的转换器编写
     3.使用PropertyChanged.Fody简化了通知属性的编写，但框架依然保留了对原始写法的支持
     4.本框架提供了注解的数据校验方式，很容易对输入进行检查
     5.在需要操作界面时，支持在View.cs中编写界面后，再调用ViewModel的公开方法，大大增加灵活性，解决了弹窗，ViewModel执行完移动界面焦点等传统老大难问题
     6.在基类中提供了IOC，消息框，日志，模型映射，消息通知的基础设施支持，极大提高开发效率
     ***还有很多其他特点，在其他Demo中将一一介绍***
     */

    /*
    本示例解决了Mvvm开发中的以下痛点：
    1.ViewModel执行完毕后移动界面输入焦点问题
    2.ViewModel执行完毕后发起界面动画问题
    3.必须在ViewModel弹窗的问题
    */
    public class MainWindowViewModel : QValidatableBase
    {
        public MainWindowViewModel()
        {
            SimpleStudent = ServiceProvider.GetService<StudentSimpleItem>();
            AdvancedStudent = ServiceProvider.GetService<StudentAdvancedItem>();
            LanguageStudent = ServiceProvider.GetService<StudentLanguageItem>();


        }
        public StudentSimpleItem SimpleStudent { private set; get; }
        public StudentAdvancedItem AdvancedStudent { private set; get; }
        public StudentLanguageItem LanguageStudent { private set; get; }

        public void SetCulture(string cultureName)
        {
            Localization.SetCulture(cultureName);
        }
    }
}
