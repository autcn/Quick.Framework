using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Quick
{
    public class Toast : ContentControl, IDisposable
    {
        public Toast()
        {
            s_toasts.Add(this);
        }

        public string Token { get; set; }

        private static List<Toast> s_toasts = new List<Toast>();

        public static void Show(object content, int duration, string token)
        {
            Toast toastControl = s_toasts.FirstOrDefault(p => p.Token == token);

            if (toastControl != null)
            {
                toastControl.Content = content;
                DoubleAnimation ani = new DoubleAnimation();
                ani.From = 1.0;
                ani.To = 0;
                ani.FillBehavior = FillBehavior.HoldEnd;
                ani.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseIn };
                ani.Duration = TimeSpan.FromMilliseconds(duration);
                toastControl.BeginAnimation(ContentControl.OpacityProperty, ani);
            }
        }

        public static void Show(object content, string token)
        {
            Show(content, 1500, token);
        }
        public static void Show(object content, int duration)
        {
            Show(content, duration, null);
        }

        public static void Show(object content)
        {
            Show(content, null);
        }

        public void Dispose()
        {
            s_toasts.Remove(this);
        }
    }
}
