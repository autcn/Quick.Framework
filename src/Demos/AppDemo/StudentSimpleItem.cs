using Quick;
using System;

namespace AppDemo
{
    public class StudentSimpleItem
    {
        public string Name { get; set; } = "张三";

        public string PhoneNumber { get; set; } = "13912343456";
        public int Age { get; set; } = 27;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public double Height { get; set; } = 170.5;

        public double Weight { get; set; } = 64.3;
        public string Address { get; set; } = "北京市海淀区上地7街";
    }
}
