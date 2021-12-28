using System;
using System.Globalization;

namespace Quick
{
    public interface ILocalization
    {
        CultureInfo CurrentCulture { get; }
        void RegisterCultureChangedHandler(object owner, Action<ILocalization> action);
        void UnRegisterCultureChangedHandler(object owner);
        void SetCulture(string cultureName);
        object GetResource(string key);
        bool TryGetResource(string key, out object val);
        string GetResString(string key);
        string Format(string key, params object[] args);
        string this[string key] { get; }
        string ConvertStrongText(string strongText);
        bool TryGetResString(string key, out string str);
    }
}
