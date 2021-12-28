namespace Quick
{
    public class EnumItemViewModel<T> : QBindableBase
    {
        public EnumItemViewModel(T enumVal)
        {
            EnumValue = enumVal;
        }
        public T EnumValue { get; set; }
        public string EnumDesc { get; set; }
    }

    public class EnumItemViewModel : EnumItemViewModel<object>
    {
        public EnumItemViewModel(object enumVal) : base(enumVal)
        {

        }
    }
}
