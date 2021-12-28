using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Quick
{
    public abstract class QValidatableBase : QBindableAppBase, IDataErrorInfo, IValidatableObject
    {
        #region Properties

        public string this[string columnName] => ValidateProperty(columnName);


        private string _error;

        [NotMapped]
        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        #endregion

        #region Private functions
        private string GetTitle(string propertyName)
        {
            QEditContext editContext = QEditContextCache.GetTypePropertyEditContext(this.GetType(), propertyName);
            string title = editContext == null ? null : editContext.GetAttr().Title;
            if (string.IsNullOrEmpty(title))
            {
                return propertyName;
            }
            return Localization.ConvertStrongText(title);
        }

        #endregion

        #region Public functions

        public virtual string ValidateProperty(string propertyName)
        {
            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            if (pi == null)
            {
                return null;
            }
            string title = GetTitle(pi.Name);
            if (pi.IsDefined(typeof(ValidationAttribute)))
            {
                List<ValidationResult> validationResults = new List<ValidationResult>();
                ValidationContext context = new ValidationContext(this);
                context.MemberName = pi.Name;
                context.DisplayName = title;
                if (!Validator.TryValidateProperty(pi.GetValue(this), context, validationResults))
                {
                    string innerMsg = validationResults[0].ErrorMessage;//可能为资源字符串
                    innerMsg = Localization.ConvertStrongText(innerMsg);
                    try
                    {
                        return string.Format(innerMsg, title);
                    }
                    catch { return innerMsg; }
                }
            }

            return null;
        }

        public virtual string Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string error = null;
            foreach (PropertyInfo pi in properties)
            {
                if (pi.DeclaringType == typeof(QValidatableBase)
                 || pi.DeclaringType == typeof(QBindableAppBase)
                 || pi.DeclaringType == typeof(QEditBindableBase))
                {
                    continue;
                }
                string pError = ValidateProperty(pi.Name);
                if (pError != null)
                {
                    if (error == null)
                    {
                        error = "";
                    }
                    if (error != "")
                    {
                        error += "\r\n";
                    }
                    error += pError;
                }
            }
            Error = error;
            return error;
        }

        #endregion
    }
}

