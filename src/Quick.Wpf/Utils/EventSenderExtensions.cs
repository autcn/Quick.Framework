using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System
{
    public static class EventSenderExtensions
    {
        /// <summary>
        /// 获取路由事件的发送者的DataContext
        /// </summary>
        /// <typeparam name="TDataContext">DataContext类型</typeparam>
        /// <param name="eventSender">事件的发送者，必须继承自FrameworkElement</param>
        /// <returns></returns>
        public static TDataContext GetDataContext<TDataContext>(this object eventSender)
        {
            return (TDataContext)((eventSender as FrameworkElement).DataContext);
        }
    }
}
