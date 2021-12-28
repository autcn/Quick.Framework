namespace Quick
{
    public class QFilePickerAttribute : QEditAttribute
    {
        public bool ShowNameOnly { get; set; } = false;
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }
        public bool IsFolderPicker { get; set; }
        public string OpenButtonText { get; set; } = "...";
    }
}
