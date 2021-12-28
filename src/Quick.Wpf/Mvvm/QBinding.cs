using Quick;

namespace System.Windows.Data
{
    public class QBinding : Binding
    {
        public QBinding(string path) : base(path)
        {
            Converter = UniversalConverter.Default;
        }
        public QBinding()
        {
            Converter = UniversalConverter.Default;
        }
    }
}
