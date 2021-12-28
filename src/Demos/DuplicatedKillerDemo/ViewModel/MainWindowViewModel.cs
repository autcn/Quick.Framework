using Quick;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks.Schedulers;
using System.Threading;

namespace DuplicatedKillerDemo.ViewModel
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
        public MainWindowViewModel(AppConfig config)
        {
            _config = config;
            Results = new ObservableCollection<CompareItem>();
        }
        #region Private members
        private AppConfig _config;
        #endregion

        #region Bindable Properties

        public string Paths
        {
            get => _config.Paths;
            set
            {
                if (_config.Paths != value)
                {
                    _config.Paths = value;
                    NotifyPropertyChanged(nameof(Paths));
                }
            }
        }

        public bool IsSearching { get; set; }

        public bool AllowDelete { get; set; }

        public bool ShowLow { get; set; }

        public string DeleteFilter { get; set; }

        public ObservableCollection<CompareItem> Results { get; set; }

        #endregion


        #region Public functions

        private void AddItem(CompareItem item1)
        {
            QWpfApplication.Current.Dispatcher.BeginInvoke(new Action<CompareItem>(item =>
            {
                Results.Add(item);
            }), item1);
        }

        public async void Search()
        {
            LimitedConcurrencyLevelTaskScheduler scheduler = new LimitedConcurrencyLevelTaskScheduler(8);
            if (Paths.IsNullOrWhiteSpace())
            {
                MsgBox.Show("未输入路径！");
            }
            Results.Clear();
            StringReader reader = new StringReader(Paths);
            List<string> paths = new List<string>();
            while (true)
            {
                string strLine = reader.ReadLine();
                if (strLine == null)
                {
                    break;
                }
                if (!strLine.IsNullOrWhiteSpace())
                {
                    string path = strLine.Trim();
                    if (!Directory.Exists(path))
                    {
                        MsgBox.Show($"路径{path}不存在！");
                        return;
                    }
                    paths.Add(path);
                }
            }
            if (paths.IsNullOrEmpty())
            {
                MsgBox.Show("不存在有效路径！");
                return;
            }
            IsSearching = true;
            try
            {

                await Task.Run(() =>
                {
                    List<string> files = new List<string>();
                    foreach (string path in paths)
                    {
                        string[] tempFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                        files.AddRange(tempFiles);
                    }
                    Dictionary<long, SizeGroup> sizeDict = new Dictionary<long, SizeGroup>();
                    foreach (string file in files)
                    {
                        FileInfo info = new FileInfo(file);
                        if (info.Length == 0)
                        {
                            File.Delete(info.FullName);
                            continue;
                        }
                        SizeGroup group = null;
                        if (!sizeDict.TryGetValue(info.Length, out group))
                        {
                            group = new SizeGroup();
                            sizeDict.Add(info.Length, group);
                        }
                        group.Add(file, new FileInfoEx(info));
                    }
                    long md5MaxSize = 10 * 1024 * 1024;

                    int count = 0;
                    foreach (var sizePair in sizeDict)
                    {
                        if (sizePair.Value.Count == 1)
                        {
                            continue;
                        }
                        
                        long currentLen = sizePair.Key;
                        bool isBigFile = currentLen > md5MaxSize;
                        SizeGroup group = sizePair.Value;
                        List<KeyValuePair<string, FileInfoEx>> list = group.ToList();
                        count++;
                        if (count >= 100)
                        {
                            //return;
                        }
                        Task.Factory.StartNew(() =>
                        {
                            for (int i = 0; i < list.Count - 1; i++)
                            {
                                var leftPair = list[i];
                                string leftPath = leftPair.Key;

                                FileInfoEx left = leftPair.Value;
                                if (left.Md5 == null && !isBigFile)
                                {
                                    byte[] leftBytes = File.ReadAllBytes(leftPath);
                                    left.Md5 = leftBytes.ToMd5UpperString();
                                }

                                for (int j = i + 1; j < list.Count; j++)
                                {
                                    var rightPair = list[j];
                                    string rightPath = rightPair.Value.Info.FullName;
                                    if (leftPath != rightPath)
                                    {
                                        CompareItem sameItem = new CompareItem();
                                        FileInfoEx right = rightPair.Value;
                                        sameItem.LeftPath = leftPath;
                                        sameItem.RightPath = rightPath;
                                        sameItem.SizeDes = currentLen.ToString("N").Replace(".00", "");
                                        sameItem.Result = CompareResult.Mid;
                                        //如果小于阈值，则必须要计算Md5
                                        if (!isBigFile)
                                        {
                                            byte[] rightBytes = File.ReadAllBytes(rightPath);
                                            right.Md5 = rightBytes.ToMd5UpperString();
                                        }
                                        if (left.Md5 != null && right.Md5 != null && left.Md5 == right.Md5)
                                        {
                                            sameItem.Result = CompareResult.High;

                                            AddItem(sameItem);
                                            count++;
                                            continue;
                                        }
                                        if (Path.GetFileName(leftPath) == Path.GetFileName(rightPath))
                                        {
                                            sameItem.Result = CompareResult.Mid;
                                        }
                                        else
                                        {
                                            if (isBigFile)
                                                sameItem.Result = CompareResult.Low;
                                            else
                                            {
                                                if (!ShowLow)
                                                {
                                                    continue;
                                                }
                                                sameItem.Result = CompareResult.Low;
                                            }
                                        }
                                        AddItem(sameItem);
                                        count++;
                                    }
                                }
                            }
                        }, CancellationToken.None, TaskCreationOptions.None, scheduler);
                    }
                });
            }
            finally
            {
                IsSearching = false;
            }
        }

        public void Delete(CompareItem compareItem, bool isLeft)
        {
            if (AllowDelete)
            {
                try
                {
                    File.Delete(isLeft ? compareItem.LeftPath : compareItem.RightPath);
                    Results.Remove(compareItem);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
            else
            {
                MsgBox.Show("不允许删除！");
            }
        }
        public void OpenDir(CompareItem compareItem, bool isLeft)
        {
            string path = isLeft ? compareItem.LeftPath : compareItem.RightPath;
            string dir = Path.GetDirectoryName(path);
            WindowsApi.ShellOpenFile(dir);
        }

        public void OpenFile(CompareItem compareItem, bool isLeft)
        {
            string path = isLeft ? compareItem.LeftPath : compareItem.RightPath;
            WindowsApi.ShellOpenFile(path);
        }

        public void BatchDelete(bool isLeft)
        {
            if (MsgBox.QuestionOKCancel("确定要执行批量删除吗？") == System.Windows.MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (CompareItem compareItem in Results)
            {
                string path = isLeft ? compareItem.LeftPath : compareItem.RightPath;
                if (!DeleteFilter.IsNullOrEmpty())
                {
                    string filter = DeleteFilter.ToLower();
                    if (!path.ToLower().StartsWith(filter))
                    {
                        continue;
                    }
                }
                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }
                catch { }
            }
            MsgBox.Show("Delete finished!");
        }
        #endregion
    }

    public enum CompareResult
    {
        High,
        Mid,
        Low
    }



    public class SizeGroup : Dictionary<string, FileInfoEx>
    {
        public long Size { get; set; }
    }

    public class FileInfoEx
    {
        public FileInfoEx(FileInfo info)
        {
            Info = info;
        }
        public string Md5 { get; set; }
        public FileInfo Info { get; set; }
    }

    public class CompareItem : QBindableBase
    {
        public string LeftPath { get; set; }
        public string RightPath { get; set; }
        public string SizeDes { get; set; }
        public CompareResult Result { get; set; }
    }
}
