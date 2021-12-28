namespace Quick
{
    public class QDataGridAttribute : QEditAttribute
    {
        public bool DeleteWarning { get; set; } = true;
        public bool DeleteKeepSelection { get; set; } = false;
        public bool UseEditBar { get; set; } = false;
        public double MaxHeight { get; set; } = double.PositiveInfinity;
        public QEditBarEditMode EditBarEditMode { get; set; } = QEditBarEditMode.Add | QEditBarEditMode.Update | QEditBarEditMode.Delete;
    }
}
