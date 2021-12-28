namespace Quick
{
    public class QAutoCompleteComboBoxAttribute : QComboBoxAttribute
    {
        public string FilterMemberPath { get; set; }
        public bool CanDropDown { get; set; }
    }
}
