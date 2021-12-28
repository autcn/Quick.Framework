using Quick;

namespace System.Windows.Controls
{
    public class DataGridTextColumnEx : DataGridTextColumn
    {
        public DataGridTextColumnEx()
        {
            Style elementStyle = (Style)Application.Current.TryFindResource(StyleKeysProperties.QDataGridCellCenterStyleKey);
            if (elementStyle != null)
                this.ElementStyle = elementStyle;
            Style editStyle = (Style)Application.Current.TryFindResource(StyleKeysProperties.QDataGridCellEditCenterStyleKey);
            if (editStyle != null)
                this.EditingElementStyle = editStyle;

            //this.HeaderStyle = (Style)Application.Current.Resources["QDataGridColumnHeaderCenterStyle"];
        }
    }
}
