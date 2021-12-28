using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Quick
{
    public abstract class QEditBindableBase : QValidatableBase, IEditableObject
    {
        [NotMapped]
        public virtual bool IsEditMode { get; set; }

        public virtual Task<int> DeleteAsync()
        {
            return Task.FromResult(1);
        }

        public virtual Task<int> SubmitAsync()
        {
            return Task.FromResult(1);
        }
    }
}

