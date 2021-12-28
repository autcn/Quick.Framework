using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick
{
    public interface IEditableObject
    {
        Task<int> SubmitAsync();
        Task<int> DeleteAsync();
        bool IsEditMode { get; set; }
    }
}
