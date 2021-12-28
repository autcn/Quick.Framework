using System.ComponentModel.DataAnnotations;

namespace Quick
{
    public class QRequiredAttribute : RequiredAttribute
    {
        public QRequiredAttribute()
        {
            ErrorMessage = QLocalizationProperties.StQEditPropertyRequiredTip;
        }
    }
}
