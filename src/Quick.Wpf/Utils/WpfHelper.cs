using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Linq;

namespace Quick
{
    public class WpfHelper
    {

        public static DependencyObject VisualTreeSearchDown(DependencyObject obj, Type type)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject objRes = VisualTreeHelper.GetChild(obj, i);
                if (objRes != null && objRes.GetType() == type)
                {
                    return objRes;
                }
                else
                {
                    DependencyObject child = VisualTreeSearchDown(objRes, type);
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        public static T VisualTreeSearchDown<T>(DependencyObject obj) where T : DependencyObject
        {
            return (T)VisualTreeSearchDown(obj, typeof(T));
        }
        public static List<T> VisualTreeSearchDownGroup<T>(DependencyObject root) where T : DependencyObject
        {
            return VisualTreeSearchDownGroup(root, typeof(T)).Cast<T>().ToList();
        }
        public static List<DependencyObject> VisualTreeSearchDownGroup(DependencyObject root, Type type)
        {
            List<DependencyObject> dependencyObjects = new List<DependencyObject>();
            InnerVisualTreeSearchDownGroup(root, type, ref dependencyObjects);
            return dependencyObjects;

            void InnerVisualTreeSearchDownGroup(DependencyObject innerRoot, Type innerSearchType, ref List<DependencyObject> group)
            {
                int childCount = VisualTreeHelper.GetChildrenCount(innerRoot);
                for (int i = 0; i < childCount; i++)
                {
                    DependencyObject objRes = VisualTreeHelper.GetChild(innerRoot, i);
                    if (objRes != null && objRes.GetType() == innerSearchType)
                    {
                        group.Add(objRes);
                    }

                    InnerVisualTreeSearchDownGroup(objRes, innerSearchType, ref group);
                }
            }
        }

        public static DependencyObject VisualTreeSearchUp(DependencyObject obj, Type searchType)
        {
            DependencyObject searchObj = VisualTreeHelper.GetParent(obj);
            if (searchObj == null)
            {
                return null;
            }
            else
            {
                if (searchObj.GetType() == searchType)
                {
                    return searchObj;
                }
                else
                {
                    return VisualTreeSearchUp(searchObj, searchType);
                }
            }
        }

        public static DependencyObject VisualTreeSearchUp<T>(DependencyObject obj) where T : DependencyObject
        {
            return (T)VisualTreeSearchUp(obj, typeof(T));
        }

        public static void DoEvents()
        {
            DoEvents(DispatcherPriority.Background);
        }

        public static void DoEvents(DispatcherPriority priority)
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.Invoke(priority,
                new DispatcherOperationCallback(delegate (object f)
                {
                    ((DispatcherFrame)f).Continue = false;

                    return null;
                }), frame);
            Dispatcher.PushFrame(frame);
        }

        public static BitmapImage GetBitmapImageFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(File.ReadAllBytes(filePath));
            bitmap.EndInit();
            return bitmap;
        }

        public static byte[] GetResourceFileContent(Uri uri)
        {
            StreamResourceInfo streamInfo = Application.GetResourceStream(uri);
            byte[] data = new byte[streamInfo.Stream.Length];
            streamInfo.Stream.Read(data, 0, data.Length);
            streamInfo.Stream.Close();
            return data;
        }
    }
}
