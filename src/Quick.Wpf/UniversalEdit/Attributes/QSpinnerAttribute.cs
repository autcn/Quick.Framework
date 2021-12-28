namespace Quick
{
    public class QSpinnerAttribute : QEditAttribute
    {
        public int MaxNumber { get; set; } = int.MaxValue;
        public int MinNumber { get; set; } = 0;
    }
}
