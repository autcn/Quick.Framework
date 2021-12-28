using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick
{
    public interface IEditableControl
    {
        void Insert();
        void Update();
        void Delete();
    }
}
