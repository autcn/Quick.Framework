using System;
using System.Windows;

namespace Quick
{
    public class QEditWindowOptions
    {
        public Type WindowType { get; set; } = typeof(Window);
        public double DefaultWidth { get; set; } = 500;
        public double DefaultHeight { get; set; } = 800;
    }
}
