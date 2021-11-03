using Quick;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SimpleMvvmDemo.ViewModel
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
        #region Private members
        private byte[] _imageData { get; set; }
        private const string DefaultImageUrl = "http://img1.doubanio.com/view/photo/l/public/p2343395499.jpg";
        private bool _hasLogin = false;
        #endregion

        #region Bindable Properties

        [Required(ErrorMessage = "下载链接不能为空！")]
        public string DownloadUrl { get; set; } = DefaultImageUrl;

        //在View中绑定该方法，演示使用QBinding来转换不同的颜色效果
        public DownloadResult DownloadResult { get; set; }

        public BitmapImage Image { get; set; }

        public string UserName { get; set; }

        public long DataSize { get; set; }

        public int UnitIndex { get; set; } = 0;

        public string Path { get; set; } = "asdf";
        #endregion

        #region Events
        public event EventHandler DownloadFinished;
        #endregion

        #region Public functions

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

        //采用了DevExpress.Mvvm提供的DXEvent，直接绑定该方法，其他用法查文档
        public async void Download()
        {
            string error = Validate();
            if (error == null)
            {
                //使用LoadingBox效果，必须在Xaml中放置LoadingBox控件
                using (var loading = new Loading("正在下载，请稍后..."))
                {
                    try
                    {
                        DownloadResult = DownloadResult.None;
                        HttpClient httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(DownloadUrl);
                        byte[] data = await response.Content.ReadAsByteArrayAsync();
                        DataSize = data.Length;
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = new MemoryStream(data);
                        image.EndInit();
                        Image = image;
                        _imageData = data;
                        DownloadResult = DownloadResult.Success;
                        //发送事件，模拟下载完毕，开启一个界面动画，见MainWindow代码
                        DownloadFinished?.Invoke(this, new EventArgs());
                    }
                    catch (Exception ex)
                    {
                        DownloadResult = DownloadResult.Failed;
                        MsgBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MsgBox.Show(error);
            }
        }

        //MainWindow中未直接绑定该方法，而是在MainWindow.cs中调用该方法，因为该方法执行完毕之后MainWindow.cs还需要移动输入焦点
        public void Clear()
        {
            DownloadUrl = null;
            DownloadResult = DownloadResult.None;
        }

        //演示DXEvent绑定
        public void RestoreDefaultUrl()
        {
            DownloadUrl = DefaultImageUrl;
        }

        public bool CanSaveImage()
        {
            if (!_hasLogin)
            {
                MsgBox.Show("要保存文件，必须先登陆！");
                return false;
            }

            if (_imageData == null)
            {
                MsgBox.Show("未下载图片，无法保存！");
                return false;
            }
            return true;
        }

        public void SaveImageToFile(string filePath)
        {
            if (_imageData != null)
            {
                File.WriteAllBytes(filePath, _imageData);
            }
        }

        //用于演示DXBinding
        public string GetStatusDes(DownloadResult result)
        {
            switch (result)
            {
                case DownloadResult.None:
                    return "未下载";
                case DownloadResult.Success:
                    return "下载成功";
                case DownloadResult.Failed:
                    return "下载失败";
            }
            return "";
        }

        public void SetLoginResult(LoginWindowViewModel loginWindowVM)
        {
            _hasLogin = loginWindowVM.LoginSuccess;
            UserName = loginWindowVM.UserName;
        }


        #endregion
    }

    public enum DownloadResult
    {
        None,
        Success,
        Failed
    }
}
