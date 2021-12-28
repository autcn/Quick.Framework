
namespace Quick
{
    public class ObjectAccessor<T>
    {
        public T Value { get; set; }

        public ObjectAccessor()
        {

        }

        public ObjectAccessor(T obj)
        {
            Value = obj;
        }
    }
}