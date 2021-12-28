using Quick;
using System;

namespace MaterialThemeDemo.ViewModel
{
    public class StudentSimpleItem : QValidatableBase
    {
        [QEdit]
        public string Name { get; set; }

        [QEdit]
        public GenderType GenderCode { get; set; }

        [QEdit]
        public string PhoneNumber { get; set; }

        [QEdit]
        public int Age { get; set; }

        [QEdit]
        public DateTime DateOfBirth { get; set; }

        [QEdit]
        public double Height { get; set; }

        [QEdit]
        public double Weight { get; set; }

        [QEdit]
        public string Address { get; set; }
    }
}
