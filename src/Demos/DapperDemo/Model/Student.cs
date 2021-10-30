using Quick;
using System;

namespace DapperDemo.Model
{
    public class Student : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Province { get; set; }
        public int? Age { get; set; }
        public double? Weight { get; set; }
        public bool IsForeigner { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImagePath { get; set; }
    }
}
