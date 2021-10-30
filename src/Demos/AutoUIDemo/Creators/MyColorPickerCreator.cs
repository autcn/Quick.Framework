using AutoUIDemo.Controls;
using Quick;
using System.Windows;

namespace AutoUIDemo.Creators
{
    /*实现自定义自动UI的步骤：
     * 1.在AppModule的ConfigureServices里添加支持context.ServiceBuilder.AddUniversalEditCreator();
     * 2.从QEditAttribute继承，创建自定义控件对应的特性，可以根据需要添加属性
     * 3.从QEditCreatorBase<TEditAttribute>继承，创建一个控件构造器类，实现CreateElement方法
     * 原理：AddUniversalEditCreator会扫描所有从QEditCreatorBase派生的类，并将其加入IOC容器，并与自定义的Attribute建立映射关系，
     *       当ViewModel中某个属性标记了自定义的Attribute，Render引擎会找到Attribute对应的Creator，然后调用其CreateElement方法来创建UI控件
     *      
     */
    public class MyColorPickerAttribute : QEditAttribute
    {

    }

    public class MyColorPickerCreator : QEditCreatorBase<MyColorPickerAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<MyColorPickerAttribute> qEditContext)
        {
            MyColorPickerAttribute attr = qEditContext.Attr;
            MyColorPicker picker = new MyColorPicker();

            //数据源绑定
            picker.SetBinding(MyColorPicker.SelectedColorProperty, CreateBinding(qEditContext));

            return picker;
        }
    }
}
