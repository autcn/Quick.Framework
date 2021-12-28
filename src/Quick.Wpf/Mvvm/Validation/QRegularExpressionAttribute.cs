using System.ComponentModel.DataAnnotations;

namespace Quick
{
    public class QRegularExpressionAttribute : RegularExpressionAttribute
    {
        public QRegularExpressionAttribute(string pattern) : base(pattern)
        {
            ErrorMessage = QLocalizationProperties.StQEditPropertyInvalidTip;
        }
    }
}
