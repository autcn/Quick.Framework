using Quick;
using Serilog;
using System.ComponentModel;

namespace ConvertersDemo
{
    [SingletonDependency]
    public class MainWindowViewModel : QBindableAppBase, IInitializable
    {
        public MainWindowViewModel()
        {
        }
        public string ConverterDescription { get; set; }

        //使用自动UI机制，请参考其他Demo
        [QEnumComboBox]
        public ConnectionStatus Status { get; set; } = ConnectionStatus.Disconnected;

        public bool IsLocked { get; set; }

        public void Initialize()
        {
            ConverterDescription = @"提供了以下内置转换器(HandyControl提供的转换器见HC文档，这里不列出)：
QBoolToReverseValueConverter          bool值反转
QBoolToCollapsedConverter             bool值转Visibility.Collapsed
QBoolToHiddenConverter                bool值转Visibility.Hidden
QBoolToVisibleConverter               bool值转Visibility.Visible，否则Visibility.Collapsed
QBoolToVisibleElseHiddenConverter     bool值转Visibility.Visible，否则Visibility.Hidden
QNullableToTrueConverter              null转true
QNullableToFalseConverter             null转false
QNullableToCollapsedConverter         null转Visibility.Collapsed
QNullableToHiddenConverter            null转Visibility.Hidden
QFileToBitmapImageConverter           图片路径转BitmapImage
QObjectToGeometryConverter            几何路径语法字符串转Geometry
QUniversalConverter                   万能转换器";

            /* 万能转换器（QUniversalConverter）或QBinding的用法：
             * ConverterParameter格式：[默认值][表达式1:转换值][表达式2:转换值][表达式3:转换值]...
             * 默认值：不符合任何一个表达式或转换发生异常时，转换为该值
             * 表达式：支持与(&，注意在xaml里应使用转义字符&amp;代替)，或(|)，非(!)，和小括号，表达式不区分大小写
             * 转换值：转换后的值，前面加@号表示从全局资源中搜索，若找到，取资源的值，找不到则取默认值
             * 绑定例子如下：
             * <Label Content="ABC" Background="{Binding Path=TestProperty, Converter={StaticResource QUniversalConverter},ConverterParameter=[Blue][1|2|3:Red][4|5|6:@MyColor]}" />
             * 以下ConverterParameter均为合法：
             * [Blue][1|2|3:Red][4|5|6:Green]   注释：默认为Blue,输入为1，2，3时，输出为Red，输入为4,5,6时，输出为Green
             * [][1|2|3:张三][4|5|6:李四]  注释：默认为""，输入为1，2，3时，输出为张三，输入为4，5，6时，输入为李四
             * [1|2|3:Red][4|5|6:Green] 注释：默认为Red，输入为1，2，3时，输出Red，输入为4，5，6时，输出为Green
             * [@MyColor][1|2|3:Red][4|5|6:Green] 注释：默认为资源MyColor，输入为1，2，3时，输出Red，输入为4，5，6时，输出Green
             * [@MyColor][!(1|2|3):Red] 注释：默认为资源MyColor，输入不为1,2,3时，输出为Red
             */
            //特别说明：QBinding相当于Binding自动指定了QUniversalConverter转换器，因此需要使用QUniversalConverter时，考虑直接用QBinding，更加简洁
        }
    }

    public enum ConnectionStatus
    {
        Connected,
        Connecting,
        Disconnected
    }
}
