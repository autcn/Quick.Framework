**Nuget包地址：**
https://www.nuget.org/packages/QuickFramework.Wpf/

用vs的nuget搜索包时，请勾选“包括预发行版本”，搜索关键字：QuickFramework.Wpf

**QQ交流群：196554374**

注：本框架要求.net framework 4.6.1或以上版本，暂不支持.net  core和.net 5，6。编译源码需要vs2019或以上版本，当第一次编译提示错误时，在解决方案上右键，点击“清理解决方案”，然后再次编译，即可成功

Table of Contents
=================

* [1\.前言](#1前言)
* [2\.框架组成](#2框架组成)
  * [2\.1 \.Net核心扩展](#21-net核心扩展)
  * [2\.2 模块化和依赖注入](#22-模块化和依赖注入)
  * [2\.3 日志系统](#23-日志系统)
  * [2\.4 配置系统](#24-配置系统)
  * [2\.5 数据库多连接支持](#25-数据库多连接支持)
  * [2\.6 Dapper ORM](#26-dapper-orm)
  * [2\.7 Local Storage](#27-local-storage)
  * [2\.8 对象映射](#28-对象映射)
  * [2\.9 WPF应用程序](#29-wpf应用程序)
  * [2\.10 多语言支持](#210-多语言支持)
  * [2\.11 消息机制](#211-消息机制)
  * [2\.12 样式增强](#212-样式增强)
  * [2\.13 扩展控件](#213-扩展控件)
  * [2\.14 MVVM增强](#214-mvvm增强)
  * [2\.15 转换器](#215-转换器)
  * [2\.16 消息框](#216-消息框)
  * [2\.17 EF支持](#217-ef支持)
  * [2\.18 自动UI](#218-自动ui)
* [3 框架特色](#3-框架特色)
  * [3\.1 C\#基础扩展](#31-c基础扩展)
  * [3\.2 MVVM](#32-mvvm)
    * [3\.2\.1 通知属性编写难](#321-通知属性编写难)
    * [3\.2\.2 Command编写难](#322-command编写难)
    * [3\.2\.3 事件绑定难](#323-事件绑定难)
    * [3\.2\.4 转换器编写难](#324-转换器编写难)
    * [3\.2\.5 MVVM弹窗难](#325-mvvm弹窗难)
    * [3\.2\.6 MVVM操作界面难](#326-mvvm操作界面难)
    * [3\.2\.7 MVVM动画难](#327-mvvm动画难)
  * [3\.3 ViewModel基础类](#33-viewmodel基础类)
    * [3\.3\.1 框架提供了多个ViewModel基类：](#331-框架提供了多个viewmodel基类)
    * [3\.3\.2 核心类QBindableAppBase](#332-核心类qbindableappbase)
  * [3\.4 LocalStorage](#34-localstorage)
  * [3\.5 配置系统](#35-配置系统)
  * [3\.6 WPF应用程序增强](#36-wpf应用程序增强)
  * [3\.7 消息机制](#37-消息机制)
  * [3\.8 消息框](#38-消息框)
  * [3\.9 自动UI](#39-自动ui)
  * [3\.10 皮肤的集成](#310-皮肤的集成)
  * [3\.11 布局](#311-布局)

# 1.前言

​        从事了10多年的.net和WPF开发，用过各种各样的设计模式和框架，包括Prism，Abp等框架，自己也曾根据自己的理解搭建过一些开发框架。各种模式和框架有它的优点，也有缺点，这是很正常的事情，因为众口难调。有些开发者不喜欢层次太多，太复杂，过于考虑扩展性的框架，他们关注点往往是快速开发，希望框架提供很多能直接用的轮子，而不关注框架的扩展性。有些开发者又需要模块和层次足够多，为将来的扩展性留有余地，他们不太在意复杂度和开发速度，而更在于架构的扩展性和灵活性。因此没有一个框架能说是完美的，能满足如此多的开发人员的不同需求。基于多年的经验和走过的路，我常常在想，WPF开发应该用那种架构最为合适，应该提供哪些轮子能使开发变的容易。于是针对开发中的种种痛点，逐渐形成了自己的思路，决定基于已有的一些经验打造一个新的框架。这个新框架同样不可能满足所有的需求，必须基于一些特定的设计目标和设计原则。

​         Quick框架原则之一是要上手简单，快速开发，尽可能的提供轮子解决WPF开发中的痛点。原则之二是要求开发者遵循框架，但不会将方方面面框死，应该给使用者留有很大自由度，因此不会像领域驱动那样，强迫开发者去用DDD。这个框架不会满足所有人的需求，希望理解它，愿意用的人用之，欢迎提出宝贵意见，不喜欢用的人也不要喷，每个框架的关注重点不一样，还是我上面说的众口难调。即使像Abp框架这样看似强大的框架，也有很多人不买账，它的缺点恰恰就是过于强大，过于解耦，太重了，忽视了开发的便利性。

​          另外，本框架使用大量第三方优秀类库作为支撑，因为我认为没必要重复造轮子，很多轮子经过多年的打磨，付出了大量的精力。如果像Prism那样，什么都自己做，我觉得一是没有时间去造，二也造不好。

# 2.框架组成

![](https://raw.githubusercontent.com/autcn/Quick.Framework/master/README.assets/quick-framework.png)

​          上图是框架核心的组成，为了便利性和架构的平衡，并没有拆解的太细，而是相对集成，降低复杂度。因此把轻量的Dapper ORM也放到了核心里，而重量级的Entity Framework则单独提供了一个Dll来支持，根据需要进行引用。

## 2.1 .Net核心扩展

​        主要是对一些常用的.Net基础类进行了扩展，比如byte[]转md5，转hex字符串等等, 常用的有判断字符串IsNullOrEmpty()，判断集合IsNullOrEmpty()等，还包括一些常用的TaskScheduler。

## 2.2 模块化和依赖注入

​       模块化和依赖注入是整个框架的核心，尽管有些初学者难以理解这两个概念，但是我认为框架有必要使用这两个机制。模块化有助于架构的清晰，合理，规范化，而依赖注入能提高开发效率和解耦。这里不是为了高大上而用这两个机制，而是为了我们合理架构和快速开发，确实需要模块化和依赖注入机制的支持。对于模块化，参考了Abp框架，它的一个很大优点是模块依赖的概念，解决了依赖注入时因先后顺序不可控导致的问题。对于依赖注入机制，则使用了目前最为流行的Autofac，之所以选择Autofac，因为它提供了足够强大的特性和极高的效率，在业界有很好的口碑，它发展了多年，迭代了很多版本，非常成熟。我认为没有必要像prism那样去自己打造一个并不完善的IOC。

​        Autofac帮助文档：https://autofac.readthedocs.io/en/latest/getting-started/index.html

## 2.3 日志系统

采用流行的Serilog提供支持，帮助文档：[https://github.com/serilog/serilog/wiki/Getting-Started]()

## 2.4 配置系统

配置文件机制也有各种各样的实现方法，本框架实现基于json的，可读写的配置系统，方便之处在于，标记一个类即可写入配置，并自动全局注入。

## 2.5 数据库多连接支持

通常来说一个WPF应用程序只访问一个数据库，在访问多个数据库时往往架构比较混乱，本框架提供了一个高度解耦的多数据库机制

## 2.6 Dapper ORM

Dapper作为轻量级的数据库访问类库，深受广大开发者欢迎，本框架封装了一个强大的基于Dapper的ORM库，很容易在框架中进行数据库操作。当然，喜欢Entity Framework的开发者也可以继续使用EF，二者不冲突，甚至可以混合使用(不推荐)。

## 2.7 Local Storage

Local Storage的好处在于，可以随时随地的存储数据，极大提高了便利性，因为很多时候，一些配置和临时数据记录并不会都存放于数据库，开发者也不愿意专门为这些数据单独写存储机制。

## 2.8 对象映射

本框架使用AutoMapper来做对象映射。历史经验告诉我们，很多业务场景下，Entity与表现层的Dto分离是必要的，否则必将导致业务混乱。而Entity与Dto的转换又是一个非常繁琐的问题，AutoMapper恰恰就是来解决对象转换问题的。

AutoMapper帮助文档：https://docs.automapper.org/en/stable/index.html

## 2.9 WPF应用程序

包括应用单一实例设置，重复启动第二个实例时自动激活第一个，全局异常捕获，崩溃自动写日志等功能。

## 2.10 多语言支持

界面多语言切换有很多种解决方案，本框架还是采用WPF的DynamicResource机制，因为这个机制在Xaml中使用时是最简单的

## 2.11 消息机制

​        一个实现订阅发布机制的消息总线对于应用来说是非常有用的，提高了解决问题的能力，也能帮助实现一些场景下的解耦。本框架底层以DevExpress.Mvvm 的Messenger为基础实现了消息的发布订阅。这里需要特别说明的是，这类消息机制无一例外都是通过全局中间件来进行通信的，因此向全局容器订阅消息时，将把订阅者的引用注册到全局容器中，尽管使用了弱引用机制，但仍广泛存在垃圾回收不及时的问题。因此这里有两条建议：一是尽量避免在临时对象中注册消息，建议在常驻对象中订阅消息，比如主窗口类，单例服务等。二是如果确需在临时对象中注册消息，记得在对象不用时注销消息订阅，这样能使得引用立即被释放。比如在弹出一个窗口中订阅了消息，那么在窗口关闭时应该注销订阅。

## 2.12 样式增强

​        对于UI控件样式的增强，确实是一个耗费巨大精力和长耗时的工作，因此本框架没有做过多的控件样式增强，开发者可以使用任何第三方UI。Quick框架本身提供了一套简易的UI皮肤，用户可以参照源码，很容易进行重写。

## 2.13 扩展控件

​        本框架提供了一些增强的控件，如：AutoCompleteComboBox，AutoFitTextControl，FilePicker，IPAddressControl，LoadingBox，PageBar，PageGrid，PageItemsControl，RadioSelector，SpaceStackPanel，SpinnerControl等等，同时也增加了对TextBox输入的限制功能。

## 2.14 MVVM增强

​        本框架使用了DevExpress.Mvvm提供的DXEvent, DXBinding, DXCommand三大神器，但并没有使用Dev的WPF框架。本框架以这三个扩展为基础，展示了与众不同的MVVM思路，极大提高了MVVM下开发的便利性，这也是本框架的一个非常大的优势，解决各种MVVM开发中的痛点。

## 2.15 转换器

​        写转换器是令很多WPF开发人员反感的一件事，本框架提供了大量内置转换器，提供了QBinding，UniversalConverter两个强大的语法，再加上Dev的DXBinding的加持，基本告别了写转换器的历史。如果必须要写转换器，那么框架提供了Format机制，使得写转换器非常简单，一个函数即可。

## 2.16 消息框

​        MessageBox是WPF开发中一个很重要的角色，本框架重写了MessageBox，提供了设置选项，可以从系统，Handy Control和本框架自己实现的消息框中任选一种。由于Handy Control的消息框存在加载慢和按钮显示延迟问题，不建议使用。

## 2.17 EF支持

​        Entity Framework作为一个重量级的数据库访问框架，深受一些开发者喜爱，本框架对EF的支持主要在于提供了ViewModel基类，更规范和方便的调用EF，同时实现了自动UI下的对EF的无感调用。

## 2.18 自动UI

​        采用ViewModel属性标记的方式，由ViewModel自动生成UI，极大的减少了Xaml代码量。对于RadioButton组，枚举的绑定问题，框架也提供了完善的解决方案。除此之外，允许开发者自定义标记和编写控件生成器。

# 3 框架特色

​       本框架的核心目标是提升开发效率，同时兼顾架构，因此这是一个开发效率与架构平衡的框架，并不会像很多框架那样，为了架构而牺牲开发效率。本节重点介绍提升效率的功能特点，如果你看完本节觉得这些功能确实解决了你的问题，提升了开发效率，那么你可以试用一下这个框架。

## 3.1 C#基础扩展

string扩展

``` c#
// ------------原写法------------
string name = GetUserName(userId);
if( string.IsNullOrEmpty(name) )
{
    throw new Exception("The user is not found.");
}
// ++++++++++++扩展写法+++++++++++
if( name.IsNullOrEmpty() )
{
    
}
```

有些开发者说并没有节省多少代码，但是写多了之后，就会发现用扩展写法还是方便的多，下面列出一些常用的：

``` c#
str.IsNullOrEmpty();
str.IsNullOrWhiteSpace();
string[] lines = str.ReadAllLines();
string md5 = str.ToMd5();
byte[] bytes = str.ToHexBytes();
string strLeft = str.Left(5);
string strRight = str.Right(5);
byte[] utf8 = str.GetBytes(Encoding.UTF8);
```

byte[]扩展

``` c#
byte[] data;
string str = data.ToHexString();
string md5 = data.ToMd5UpperString();
```

IEnumerable<T>扩展

``` c#
// 原写法
List<User> users = GetUsers();
if( user == null || user.Count == 0 )
{
    throw new Exception("The users are empty!");
}
// 扩展写法
if( users.IsNullOrEmpty() )
{
    
}
```

还有很多其他扩展，不全部列出了

## 3.2 MVVM

很多新接触 WPF的开发人员，对于MVVM开发特别不适应，他们会觉得各种繁琐，甚至对该模式有所抵触，该节着重介绍本框架的MVVM的理念

### 3.2.1 通知属性编写难

为了普通类支持绑定，WPF要求类实现INotifyPropertyChanged 接口，每个属性的Set都要写Notify，这是很多开发人员很反感的。

``` c#
// ------------原写法1------------
private string _name;
public string Name
{
    get { return _name; }
    set
    {
        if( _name != value )
        {
            _name = value;
            NotifyPropertyChanged(nameof(Name));
        }
    }
}

// ------------改进写法2------------
private string _name;
public string Name
{
    get => _name;
    set => SetProperty(ref _name, value);
}

// ++++++++++++Quick框架写法+++++++++++
public string Name { get; set; }
```

### 3.2.2 Command编写难

我们知道按钮的执行要绑定到ViewModel，需要在ViewModel中定义ICommand，很多开发人员对此都十分反感，界面几十个按钮，就要写几十遍这个东西

``` c#
// ------------原写法------------
<Button Command="{Binding Path=SearchCommand}" />

private ICommand _searchCommand;
public ICommand SearchCommand
{
    get
    {
        if( _searchCommand == null )
        {
            _searchCommand = new RelayCommand(DoSearch(), null);
        }
        return _searchCommand;
    }
}

public void DoSearch(object param)
{
    
}

// ++++++++++++Quick框架写法+++++++++++
<Button Command="{DXCommand DoSearch()}" />
```

### 3.2.3 事件绑定难

​        令WPF开发人员困惑的是，有Command属性的控件，我们尚能绑定到ViewModel，而很多控件并没有Command属性。之前的做法要不用EventToCommand之类的附加属性来解决，要不用Interaction框架。这里特别说明，本人非常反感使用Interaction机制，极大了降低了开发效率，我相信很多开发者跟我也一样。

``` c#
// ------------原写法1------------
//缺点：写法繁琐，事件是字符串，无法智能提示，还要在ViewModel定义一个Command，每个Xaml还要声明ext命名空间
<ListBox ext:EventToCommand.Event="SelectionChanged" ext.EventToCommand.Handler="{Binding SelChangedCommand}" />

// ------------原写法2------------
//缺点：写法非常繁琐
<ListBox>
    <i:Interaction.Triggers>
	　　<i:EventTrigger EventName="SelectionChanged">
　　		　　<i:InvokeCommandAction Command="{Binding SelChangedCommand}" />
		</i:EventTrigger>
    </i:EventTrigger>
</ListBox>


// ++++++++++++Quick框架写法+++++++++++
<ListBox SelectionChanged="{DXEvent SelectionChange()}" />
<Button Click="{DXEvent DoSearch()}" />
```

​       有人提出这跟在View.cs里写事件处理有什么区别呢，这里的区别在于，绑定的函数必须在ViewModel里，保持了ViewModel的业务完整性，而ViewModel无法访问UI，保持了View和ViewModel的隔离，也是符合MVVM思想的。很多人的误区在于，ViewModel只有暴露ICommand才是MVVM，UI只有绑定了ICommand才是MVVM，这个思想是不对的，对于ViewModel而言，只要不访问UI，保证了业务完整性即可，不论是暴露ICommand还是暴露函数，都是没有问题的。回到最开始说的问题，如果在view.cs里写了处理函数，那么将难以避免将一些业务逻辑写在view.cs里，导致本应写在ViewModel里的功能分散到了view.cs里，破坏了ViewModel的完整性，导致ViewModel依赖View.cs，造成耦合。

### 3.2.4 转换器编写难

(1) 在界面绑定属性时，很多时候不能直接绑定，每遇到要写转换器，也是一件让开发人员头疼的事情，Quick框架提供了多种方式解决这个问题，详见ConvertersDemo源码

``` c#
//假设以下需求：根据ViewModel中网络状态，界面控件改变颜色
public enum NetworkStatus
{
    Disconnected,
    Connecting,
    Connected,
}
public class MainViewModel : ViewModelBase
{
    public NetworkStatus NetworkStatus { get; set; }
}

// ----------------------------原写法-------------------------------
//由于ViewModel中数据类型是一个枚举，那么我们需要定义一个转换器，将枚举转颜色
public class NetworkStatusToBrushConverter : IValueConverter
{
    public static NetworkStatusToBrushConverter Default { get; } = new NetworkStatusToBrushConverter();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Brushes.Gray;
        }
        try
        {
            NetworkStatus status = (NetworkStatus)value;
            if( status == NetworkStatus.Connecting )
            {
                return Brushes.Orange;
            }
            if( status == NetworkStatus.Connected )
            {
                return Brushes.Green;
            }
        }
        catch
        {
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException()
    }
}
<Rectangle Fill="{Binding Path=NetworkStatus, Converter={x:Static cnv:NetworkStatusToBrushConverter.Default}}" />
//或者定义一个资源
<cnv:NetworkStatusToBrushConverter x:Key="NetworkStatusToBrushConverter" />
<Rectangle Fill="{Binding Path=NetworkStatus, Converter={StaticResource NetworkStatusToBrushConverter}}" />

// +++++++++++++++++++++++++Quick框架写法+++++++++++++++++++++++++++++++++
<Rectangle Fill="{QBinding Path=NetworkStatus, ConverterParameter=[Gray][Connecting:Orange][Connected:Green]}" />
//语法含义：默认为Gray，Connecting为橙色，Connected为绿色
```


可以看到，通过使用Quick框架，无需定义转换器，只用一行代码就实现了同样的效果，大大提升开发效率



(2) 开发人员遇到需要绑定多个属性时，也经常不想去写转换器，觉得非常繁琐，Quick框架提供了以下解决方案，详见SimpleMvvmDemo源码

​        以下是需要绑定的ViewModel，需求是在界面上显示图片的尺寸描述，有两种单位制，一个是像素，一个是厘米，那么我们的界面需要同时绑定Image和UnitIndex，这样在图片或单位变化时，尺寸描述都可以自动变化

``` c#
public class MainWindowViewModel : QValidatableBase
{
    public BitmapImage Image { get; set; }  //图片
    public int UnitIndex { get; set; } = 0;  //单位下拉框的选择索引，0为像素，1为厘米
    
    public string GetImageSize()
    {
        if (Image == null)
        {
            return null;
        }
        if (UnitIndex == 0)
        {
            return $"{Image.PixelWidth} x {Image.PixelHeight}";
        }
        else
        {
            double widthInch = (double)Image.PixelWidth / (double)Image.DpiX;
            double heightInch = (double)Image.PixelHeight / (double)Image.DpiY;
            double widthCm = widthInch * 2.54;
            double heightCm = heightInch * 2.54;
            return $"{widthCm:F02}厘米 x {heightCm:F02}厘米";
        }
    }
}  
// ----------------------------原写法-------------------------------
//定义MultiConverter，这里只写出大致代码
public class ImageToSizeDesConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        
    }


    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return null;
    }
}
//XAML代码：
<cnv:ImageToSizeDesConverter x:Key="ImageToSizeDesConverter" />
<TextBlock>
    <TextBlock.Text>
        <MultiBinding Converter="{StaticResource ImageToSizeDesConverter}" 
            <Binding Path="Image" />
            <Binding Path="UnitIndex" />
        </MultiBinding>
    </TextBlock.Text>
</TextBlock>


// +++++++++++++++++++++++++Quick框架写法1+++++++++++++++++++++++++++++++++
<TextBlock Text="{DXBinding 'Triggers(Image, UnitIndex).GetImageSize()'}" />
//注释：指定触发属性为Image, UnitIndex，二者中有任何一个变化时，调用GetImageSize()函数获取返回值，
//这样做的好处是不必写转换器类了

// +++++++++++++++++++++++++Quick框架写法2+++++++++++++++++++++++++++++++++
/*   在ViewModel中编写颜色转换等UI代码实际上是违背原则的，因此不推荐1的写法，
Quick框架提供了一个全局的IFormat来集中编写转换函数，使得UI代码和业务代码分离
    首先定义一个Format类，继承自IFormat，框架启动时将自动注入单例，应用所有的
转换函数都写在这个类里，这样比写WPF的Converter要简单的多，同时也独立于ViewModel，
使得ViewModel不含界面代码*/
public class Format : IFormat
{
    public string GetImageSize(Image image, int unitIndex)
    {
        if (image == null)
        {
            return null;
        }
        if (unitIndex == 0)
        {
            return $"{image.PixelWidth} x {image.PixelHeight}";
        }
        else
        {
            double widthInch = (double)image.PixelWidth / (double)image.DpiX;
            double heightInch = (double)image.PixelHeight / (double)image.DpiY;
            double widthCm = widthInch * 2.54;
            double heightCm = heightInch * 2.54;
            return $"{widthCm:F02}厘米 x {heightCm:F02}厘米";
        }
    }
    
    public string ToDataSizeDes(long size)
    {
        double fSize = (double)size / 1024.0;
        string unit = "KB";
        if (fSize >= 1024)
        {
            fSize /= 1024.0;
            unit = "MB";
        }

        if (fSize >= 1024)
        {
            fSize /= 1024.0;
            unit = "GB";
        }
        return fSize.ToString("F02") + " " + unit;
    }
}
//XAML代码
<TextBlock Text="{DXBinding 'Format.GetImageSize(Image, UnitIndex)'}" />
```

### 3.2.5 MVVM弹窗难

​        在MVVM模式下弹出窗口，确实是一个难题，于是有了各种各样的解决方案，最为代表性的是IDialogService这种解决方式，然后又带来了ViewModel如何关窗的问题，如何拦截窗口关闭问题，如何设置初始窗口大小问题，如何控制窗口最大化，最小化按钮等等问题。解决方法不胜枚举，但很多都是绕了一大圈，要写一大堆代码才能实现。Quick框架的解决方案是回归原始，界面的问题由view.cs代码解决，业务的问题由ViewModel.cs解决

``` c#
//我们用最原始的写法写事件响应函数
<Button Content="登陆" Click="btnLogin_Click" Width="100" Margin="10,0,0,0"  />
    
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _vm;
    public MainWindow(MainWindowViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        this.DataContext = _vm;
    }
    //在按钮的响应函数中直接弹窗即可，但ViewModel的交互逻辑都在ViewModel中处理
    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
       LoginWindow loginWnd = QServiceProvider.GetService<LoginWindow>();
       loginWnd.Owner = this;
        
       if (loginWnd.ShowDialog().Value)
       {
           //调用ViewModel的公开函数，不可在View.cs中写业务代码
           _vm.SetLoginResult(loginWnd.VM);
       }
    }
}
```

### 3.2.6 MVVM操作界面难

​        试想一个需求场景：界面上有一个输入框和一个按钮，点击按钮之后， ViewModel执行搜索，搜索执行完毕之后，界面焦点需要回到输入框，并选中之前输入的关键词。这个需求令很多开发者无从下手，因为我们常用的方法是，界面Button->Command->ViewModel.Command->结束。由于不能在ViewModel中操作界面，因此输入焦点如何返回，输入的内容如何选中？

这里不再列举其他框架的解决方案，直接写出Quick框架的解决方式

```c#
<Button Content="搜索" Click="btnSearch_Click" Width="100" Margin="10,0,0,0"  />

//同样的，我们用传统方式给按钮添加事件响应
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _vm;
    public MainWindow(MainWindowViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        this.DataContext = _vm;
    }

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
       //ViewModel提供公开函数
       _vm.DoSearch();
       //操作界面焦点和输入全选
       tbKeywork.Focus();
       tbKeywork.SelectAll();
    }
}
```

​        我相信一些人看了以上两节的代码，马上会提出反对意见：你这不是MVVM，你在View.cs里写代码了，你在View里添加事件处理了，你没有用Command。作为一个有10年WPF开发经验的人，我非常理解他们的意见，因为这也是我曾经的认知。我想说的是，MVVM是一种思想，而不是某种固定的代码格式。ViewModel的职责是负责整体业务处理，并提供接口对外交互，接口的方式不仅限于ICommand，还包括公有函数，事件等。ViewModel是否合格，我们只需要看以下几点：

1. ViewModel是否业务完整？
2. ViewModel是否能独立跑通单元测试?
3. ViewModel是否操作UI了？
4. ViewModel能否适配另一个UI?

​        对照以上几条原则去检查上面的做法是没有问题的。因为ViewModel暴露了公开函数去跟UI交互，保持了自身业务的完整性和独立性。MainWindow.cs没有任何跟业务有关的代码，只是界面辅助代码和对ViewModel的接口函数调用而已。**唯一的缺点在于：框架本身没有限制开发者在view.cs里写业务代码，只能靠开发人员自我约束。**以上面的例子为例，如果_vm.DoSearch()的实现代码写在btnSearch_Click()里也不会报错，但这显然就破坏了ViewModel的完整性，回归到MVP开发的老模式上去了。本来想在ViewModel上提一层接口出来，阻止View直接看到ViewModel，从而避免在View.cs里写业务代码，但反复考虑之后，这会增加开发复杂度，违背Quick的初衷，同时考虑到这种情况是非常少，因此没有加接口机制。

### 3.2.7 MVVM动画难

我们知道WPF的动画是View层面的机制，我们常常有需求：ViewModel执行完某个操作后，让界面发起一段动画。详见SimpleMvvmDemo源码

``` c#
//在ViewModel里注定义一个事件，某个操作完成后，执行该事件
public class MainWindowViewModel : QValidatableBase
{
    public event EventHandler DownloadFinished;
    public async void Download()
    {
        //延时模拟
        await Task.Wait(2000);
        DownloadFinished?.Invoke();
    }
}
//在View.cs里注册事件
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _vm;
    public MainWindow(MainWindowViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        _vm.DownloadFinished += _vm_DownloadFinished;
        this.DataContext = _vm;
    }
    //ViewModel下载完图片后，走一个界面动画，通过注册ViewModel的事件来实现解耦
    private void _vm_DownloadFinished(object sender, EventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = -500;
        animation.Duration = TimeSpan.FromSeconds(1);
        animation.To = 0;
        animation.FillBehavior = FillBehavior.HoldEnd;
        animation.EasingFunction = new CircleEase();
        
        //imgTranslateTransform为界面定义的元素名称
        imgTranslateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
    }
}

//XAML代码
<Button Content="下载图片" Click="{DXEvent Download()}"   />
<Image Source="{Binding Path=Image}">
    <Image.RenderTransform>
	    <TranslateTransform x:Name="imgTranslateTransform" />
     </Image.RenderTransform>
</Image>    
```



## 3.3 ViewModel基础类

### 3.3.1 框架提供了多个ViewModel基类：

QBindableBase   最简单的基础类，继承INotifyPropertyChanged接口

​		QBindableAppBase  应用基础类，提供了应用开发需要的常用组件

​				DapperAppBindableBase<TMainDbContext>  支持Dapper数据库访问的应用基础类

​				EfAppBindableBase<TMainDbContext>   支持Entity Framework数据库访问的应用基础类

​				QValidatableBase  可校验的基础类，为输入提供校验支持

​						QEditBindableBase	可编辑基础类，提供了增删改接口

​								DapperEditBindableBase<TMainDbContext> 受Dapper数据库支持的编辑基础类

​								EfEditBindableBase<TMainDbContext>  Entity Framework支持的编辑基础类

**ViewModel基类的选择原则考虑以下几点：**是否带输入绑定？是否需要数据校验？是否增删改对象？是否支持数据库？

(1) 不需要输入绑定的ViewModel，应该从QBindableAppBase继承

(2) 不需要输入和编辑，但需要访问数据库的ViewModel，应该从DapperAppBindableBase<TMainDbContext> 或EfAppBindableBase<TMainDbContext>继承

(3) 需要数据校验，但不需要访问数据库，比如自己存文件或云存储的，应该从QValidatableBase 继承

(4) 需要数据校验，作为增删改操作对象，又不想用Dapper和Ef的，从QEditBindableBase继承

(5) 需要数据校验，绑定编辑输入，同时又要访问数据库的，从DapperEditBindableBase<TMainDbContext>或EfEditBindableBase<TMainDbContext>继承

(6) 只要求通知属性触发，追求极简效率，不用任何其他组件的，从最基础的QBindableBase 继承

### 3.3.2 核心类QBindableAppBase

QBindableAppBase作为ViewModel的核心基类，提供了大量组件，使得ViewModel中可以直接使用，大大提升开发效率，下面分别介绍：

``` c#
public IFormat Format { get; }  //为Xaml绑定提供格式化和转换服务
protected IServiceProvider ServiceProvider { get; }  //IOC容器的Provider服务
protected ILocalization Localization { get; }  //多语言服务
protected IMessageBox MsgBox { get; } //消息框服务
protected IMapper Mapper { get; } //对象映射服务
protected ILogger Logger { get; } //日志服务
protected ILocalStorage Ls { get; }  //LocalStorage服务
protected IMessenger Messenger { get; }  //消息服务
protected IQConfiguration Configuration { get; } //配置服务

//触发器，用于Xaml中绑定触发
public QBindableAppBase Triggers(params object[] propertyNames);
//获取LocalStorage
protected virtual TValue GetLs<TValue>([CallerMemberName] string propertyName = null);
//获取LocalStorage或者返回默认值
protected virtual TValue GetLsOrDefault<TValue>(TValue defaultValue, [CallerMemberName] string propertyName = null);
//设置LocalStorage
protected virtual void SetLs<TValue>(TValue value, [CallerMemberName] string propertyName = null);
```

## 3.4 LocalStorage

LocalStorage机制提高了开发的便利性，以json为格式，可见，可编辑，因此有必要特别予以介绍。详见LocalStorageDemo，下面举一个简单例子：

``` c#
public class MainWindowViewModel : QBindableAppBase
{
    public string Name
    {
        get => GetLs<string>(); //自动从LocalStorage中获取，如果不存在返回null
        set => SetLs(value);   //自动保存到LocalStorage
    }
}
<TextBox Text="{Binding Path=Name}" />
```

​        以上代码看似和普通代码没有什么区别，但你会神奇的发现，Name的值被持久化了，也就是说界面修改的Name自动存盘，下次程序启动时，Name自动加载为上次的值。这里有开发人员可能会有疑问，如果高频率修改Name的值，是不是每次都要写硬盘，存在效率问题？这个问题框架也有所考虑，如果2秒内没有新的更改，才会写入硬盘，否则只是修改内存，不存在效率问题。

## 3.5 配置系统

WPF应用中，有些设置参数和选项，需要写入配置文件持久化。实现配置有各种各样的方式，Quick框架提供了一种便利方式

``` c#
//只需要定义一个类，标记QConfigurationSection，并设置一个不重复的名称，框架将自动维护它的读取和存储
[QConfigurationSection("app")]
public class AppConfig
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

/*   配置默认存储于程序安装目录下的appsettings.json里了，如果要设置初始值，可以在工程目录下的
appsettings.json里设置它，并且将文件生成动作设置为：如果较新则复制 */

{
  "app": {
    "UserName": "liming",
    "Password" :  "123456"
  }
}

//使用方法，上面的AppConfig类已经被全局自动注入了，因此可以在任何地方使用它
public class LoginViewModel : QBindableAppBase
{
    public void DoLogin()
    {
        AppConfig config = ServiceProvider.GetService<AppConfig>();
        
    }
}
```

需要注意的是，如果程序崩溃，没有正常退出，配置将丢失，如果需要立即将配置存盘，需要注入IQConfiguration服务, 调用其Save方法

``` c#
public class LoginViewModel : QBindableAppBase
{
    public void DoLogin()
    {
        AppConfig config = ServiceProvider.GetService<AppConfig>();
        config.Name = "new user";
        config.Password = "password";
        Configuration.Save();
    }
}
```

## 3.6 WPF应用程序增强

​        针对开发中的一些痛点：比如应用单一实例启动后，再启动第二个时，如何激活第一个。Quick框架进行了高效实现(不用socket，不用管道，不用窗口消息)，减轻了开发人员负担。

``` c#
 public partial class App : QWpfApplication
{
    /// <summary>
    /// 以下属性也可以直接在Xaml里设置，这里是为了添加注释
    /// </summary>
    public App()
    {
        //应用程序单例，当设置该项时，在启动第二个实例时，将自动激活已启动的，阻止第二个启动，
        //需要注意的是，不同应用应该使用不同的Id
        SingleInstanceAppId = "C620B9C4-92FF-4980-B536-6C7A52135761";

        //设置该项后，在应用退出时，将采用结束进程的方式执行快速关闭，加速程序关闭的速度，默认为false
        QuickShutdown = true;

        //设置该项后，当程序出现异常时，将阻止程序崩溃，并记录异常日志，默认为true
        PreventShutdownWhenCrashed = true;
    }
    //当设置了SingleInstanceAppId，启动第二个实例时，第一个实例会调用以下方法，可以在这里写自定义代码
    protected override void OnStartupNextInstance()
    {

    }
 }
```

## 3.7 消息机制

消息机制作为提效解耦的利器，也是必不可少的，各框架都提供了自己的消息中间件。Quick框架中，消息机制的使用也非常简单。

``` c#
//首先定义一条消息
public class SearchFinishedMessage
{
    public SearchFinishedMessage(string keyword)
    {
        Keyword = keyword;
    }

    public string Keyword { get; }
}

//订阅消息
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _mainVM;
    public MainWindow(MainWindowViewModel mainVM)
    {
        InitializeComponent();
        _mainVM = mainVM;
        this.DataContext = _mainVM;

        Messenger.Default.Register<SearchFinishedMessage>(this, msg =>
        {
            tbxKeyword.Focus();
            tbxKeyword.SelectAll();
        });
    }
}

//发送消息
[SingletonDependency]
public class MainWindowViewModel : QBindableBase
{

    public string Keyword { get; set; }

    public void DoSearch()
    {
        if (Keyword.IsNullOrEmpty())
        {
            return;
        }
        //调用消息服务，发送消息
        Messenger.Default.Send(new SearchFinishedMessage(Keyword));
    }
}

```

## 3.8 消息框

消息框作为常用的功能，开发人员用起来也很繁琐，常常要写很多代码，有几个痛点：

（1） 参数多，尤其是OkCancel这种类型的，要指定一堆参数

（2） 系统消息框，不支持多语言

（3） 弹出的消息内容如果要支持多语言，也很麻烦

   (4）纠结能否在ViewModel中弹消息框

Quick框架解决了以上4个问题

``` c#
public class MainWindowViewModel : QValidatableBase
{
    public void BatchDelete()
	{	
        //直接提供QuestionOKCancel和QuestionYesNo方法来解决参数多的问题
    	if (MsgBox.QuestionOKCancel("确定要执行批量删除吗？") == MessageBoxResult.Cancel)
	    {
    	    return;
	    }
        //其中MsgBox.Question.BatchDelete为定义在多语言文件中的字符串资源的Key
        //注：当以@开头时，将从资源中搜索，以便支持多语言
        if (MsgBox.QuestionOKCancel("@MsgBox.Question.BatchDelete") == MessageBoxResult.Cancel)
	    {
    	    return;
	    }
    }
}
```

## 3.9 自动UI

自动UI机制是本框架的一大特色，这里简单予以介绍，详细情况请参考AutoUIDemo。

``` c#
//枚举定义
public enum OccupationType
{
    Student,
    Doctor,
    Teacher
}

//XAML代码如下
<QEditPanel>
	<QElement Name="Occupation" Width="100" />
</QEditPanel>

//ViewModel代码
public class EditPanelDemoViewModel : QValidatableBase
{
    [QEnumComboBox]
    public OccupationType Occupation { get; set; }
}
```

​       通过在ViewModel中为枚举标记QEnumComboBox特性，Xaml中的QElement将运行时自动渲染出ComboBox效果，并自动绑定，这个机制又节省了大量代码

## 3.10 皮肤的集成

### 3.10.1 Quick框架提供的皮肤

Quick框架本身提供一套可选的UI皮肤样式，包含黑白两种风格，开发者如果不满意有两个选择，一个是直接用Quick皮肤的源码进行重写，实现完全自定义的皮肤；相比于其他第三方框架的复杂代码，Quick的皮肤源码要简单的多。第二个选择是使用第三方的皮肤，比如Material Design或Handy Control等UI。

Quick框架皮肤集成(参见QuickThemeDemo工程)，在app.xaml里添加如下代码

黑色风格：

``` xaml
<Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf;component/Themes/AutoUI.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf.Themes;component/Themes/ColorBlack.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf.Themes;component/Themes/ThemeConcise.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

白色风格：

``` xaml
<Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf;component/Themes/AutoUI.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf.Themes;component/Themes/ColorBlue.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf.Themes;component/Themes/ThemeConcise.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

这里需要说明的是引用AutoUI.xaml是为了使用Quick框架的自动UI和一些扩展控件，不需要使用的情况下可以不引入。

另外，Quick框架提供了一个轻量的样式呈现默认界面，在不使用任何皮肤的情况下，建议引入该样式文件，获得更好的界面呈现

``` xaml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf;component/Themes/Lite.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    <Application.Resources>
```

### 3.10.2 使用其他框架提供的皮肤

以Material Design为例，使用方法跟不用Quick框架时没有区别，如下所示：

``` xaml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Quick.Wpf;component/Themes/AutoUI.xaml"/>
                <materialDesign:BundledTheme BaseTheme="Light" PrimaryColor="DeepPurple" SecondaryColor="Lime" />
                <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

## 3.11 布局

Quick框架提供了一个强大的布局面板SpaceStackPanel，兼具Grid的部分功能和StackPanel的功能，可以很容易创建可伸缩，适应分辨率的布局，减少大量布局代码，可以说“一板走天下“，详细例子可参见LayoutDemo工程。

微信布局的例子：

``` xaml
<Window x:Class="LayoutDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:LayoutDemo"
        xmlns:qk="http://quick.cvbox.cn/2020"
        xmlns:sys="clr-namespace:System;assembly=mscorlib"
        mc:Ignorable="d" WindowStartupLocation="CenterScreen" 
        Title="QuickFramework - 布局演示，模拟微信布局" Height="720" Width="900">
    <Window.Resources>
        <x:Array Type="sys:String" x:Key="dataSource">
            <sys:String>zhangsan</sys:String>
            <sys:String>lisi</sys:String>
            <sys:String>wangwu</sys:String>
        </x:Array>
    </Window.Resources>
    <SpaceStackPanel Orientation="Horizontal">
        <!--左-->
        <SpaceStackPanel Background="#2e2e2e" Width="50">
            <!--上-->
            <SpaceStackPanel Width="30" Margin="0,12,0,0" Spacing="12">
                <Button Height="30" />
                <Button Height="30" />
                <Button Height="30" />
                <Button Height="30" />
                <Button Height="30" />
            </SpaceStackPanel>

            <!--中，无内容，设置站位-->
            <SimplePanel SpaceStackPanel.Weight="*"  />

            <!--下-->
            <SpaceStackPanel Width="30" Margin="0,0,0,14">
                <Button Height="30" />
                <Button Height="30" />
                <Button Height="30" />
            </SpaceStackPanel>
        </SpaceStackPanel>

        <!--中-->
        <SpaceStackPanel Width="220" Margin="0,14">
            <!--搜索框-->
            <SpaceStackPanel Orientation="Horizontal" Height="28">
                <TextBox SpaceStackPanel.Weight="*" />
                <Button Content="+" Width="32" />
            </SpaceStackPanel>
            <!--会话列表，占满-->
            <SpaceStackPanel SpaceStackPanel.Weight="*">
                <ItemsControl ItemsSource="{StaticResource dataSource}">
                    <ItemsControl.ItemTemplate>
                        <DataTemplate>
                            <!--有跨行跨列需求的局部布局使用Grid-->
                            <Grid Height="46" Margin="0,0,0,14">
                                <Grid.RowDefinitions>
                                    <RowDefinition Height="*" />
                                    <RowDefinition Height="*" />
                                </Grid.RowDefinitions>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="46" />
                                    <ColumnDefinition Width="*" />
                                    <ColumnDefinition Width="auto" />
                                </Grid.ColumnDefinitions>
                                <Rectangle Fill="Gray" Grid.RowSpan="2" />
                                <TextBlock Text="文件传输助手" VerticalAlignment="Center" Grid.Column="1" Margin="10,0,0,0" />
                                <TextBlock Text="[图片]" Grid.Row="1" VerticalAlignment="Center" Grid.Column="1" Margin="10,0,0,0" Foreground="Gray"  />
                                <TextBlock Text="10:12" Grid.Column="2" VerticalAlignment="Center" Margin="10,0,0,0" Foreground="Gray"  />
                            </Grid>
                        </DataTemplate>
                    </ItemsControl.ItemTemplate>
                </ItemsControl>
            </SpaceStackPanel>
        </SpaceStackPanel>

        <!--右，可伸缩的部分，设置SpaceStackPanel.Weight="*"，因为有可调区域，用Grid才能使用GridSplitter-->
        <Grid SpaceStackPanel.Weight="*" Margin="10">
            <Grid.RowDefinitions>
                <RowDefinition Height="auto"  />
                <RowDefinition Height="*" MinHeight="100" />
                <RowDefinition Height="8" />
                <RowDefinition Height="300" MinHeight="140" />
            </Grid.RowDefinitions>
            <!--标题-->
            <TextBlock Text="文件传输助手" FontSize="18" Margin="0,0,0,10" />
            
            <!--消息显示-->
            <TextBox Grid.Row="1">

            </TextBox>
            
            <!--分隔条-->
            <GridSplitter Grid.Row="2" Background="Transparent" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" />
            
            <!--输入区域-->
            <SpaceStackPanel Grid.Row="3" Spacing="8" >
                <!--工具区-->
                <SpaceStackPanel Orientation="Horizontal" Height="30">
                    <!--工具区左-->
                    <SpaceStackPanel Orientation="Horizontal">
                        <Button Width="30" />
                        <Button Width="30" />
                        <Button Width="30" />
                        <Button Width="30" />
                    </SpaceStackPanel>
                    
                    <!--占位-->
                    <SimplePanel SpaceStackPanel.Weight="*" />

                    <!--工具区右-->
                    <SpaceStackPanel Orientation="Horizontal">
                        <Button Width="30" />
                        <Button Width="30" />
                    </SpaceStackPanel>
                </SpaceStackPanel>
                <!--输入区，占满-->
                <TextBox SpaceStackPanel.Weight="*" />
                <!--发送按钮-->
                <Button Content="发送(S)" Height="30" HorizontalAlignment="Right" Width="80"/>
            </SpaceStackPanel>
        </Grid>
    </SpaceStackPanel>
</Window>

```



