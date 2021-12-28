using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Quick
{
    public class Localization : ILocalization, IInitializable
    {
        private readonly LocalizationOptions _options;
        private List<LocalizationActionItem> _registers = new List<LocalizationActionItem>();
        private object _lockObj = new object();
        public Localization(LocalizationOptions options)
        {
            _options = options;
        }
        private CultureInfo _culture;
        public CultureInfo CurrentCulture => _culture;

        public string this[string key]
        {
            get => GetResString(key);
        }

        public void Initialize()
        {
            SetCulture(CultureInfo.CurrentCulture.Name);
        }
        public object GetResource(string key)
        {
            if(!Application.Current.Resources.Contains(key))
            {
                throw new QException($"The resource {key} is not found!");
            }
            return Application.Current.Resources[key];
        }

        public string GetResString(string key)
        {
            return (string)GetResource(key);
        }

        public void SetCulture(string cultureName)
        {
            CultureInfo info = CultureInfo.GetCultureInfo(cultureName);
            _culture = info;
            

            foreach (LocalizationOptionItem item in _options.Items)
            {
                string uriPath = $"pack://application:,,,/{item.AssemblyName};component/Localization/{cultureName}/{ConvertToXamlFileName(item.FileName)}";
                Uri uri = new Uri(uriPath, UriKind.Absolute);
                ResourceDictionary resDict = new ResourceDictionary();
                resDict.Source = uri;
                Application.Current.Resources.MergedDictionaries.Add(resDict);
            }



            //通知更新
            GC.Collect();
            //lock (_lockObj)
            //{
            //    for (int i = _registers.Count - 1; i >= 0; i--)
            //    {
            //        WeakAction<object> action = _registers[i].Item1;
            //        object param = _registers[i].Item2;
            //        if (action.IsAlive)
            //            action.Execute(this);
            //        else
            //        {
            //            action.MarkForDeletion();
            //            _registers.RemoveAt(i);
            //        }
            //    }
            //}
            RemoveDead();

            foreach (var item in _registers)
            {
                foreach (var action in item.Actions)
                {
                    action.Invoke(this);
                }
            }
        }

        private string ConvertToXamlFileName(string fileName)
        {
            if (fileName.ToLower().EndsWith(".xaml"))
            {
                return fileName;
            }
            return fileName + ".xaml";
        }

        public bool TryGetResource(string key, out object val)
        {
            if (!Application.Current.Resources.Contains(key))
            {
                val = null;
                return false;
            }
            val = Application.Current.Resources[key];
            return true;
        }

        public bool TryGetResString(string key, out string str)
        {
            bool result = TryGetResource(key, out object val);
            str = (string)val;
            return result;
        }

        public string ConvertStrongText(string strongText)
        {
            if (strongText == null)
            {
                return null;
            }
            if (strongText.Length <= 1)
            {
                return strongText;
            }
            if (strongText.StartsWith(QLocalizationProperties.ResourceKeyPrefixEscape))
            {
                return strongText.Substring(1);
            }
            if (strongText.StartsWith(QLocalizationProperties.ResourceKeyPrefix))
            {
                return GetResString(strongText.Substring(1));
            }
            return strongText;
        }

        public void RegisterCultureChangedHandler(object owner, Action<ILocalization> action)
        {
            if (action == null)
            {
                throw new Exception("The action can not be null.");
            }
            RemoveDead();
            lock (_lockObj)
            {
                LocalizationActionItem item = _registers.FirstOrDefault(p => p.Owner.Target == owner);
                if (item == null)
                {
                    item = new LocalizationActionItem();
                    item.Owner = new WeakReference(owner);
                    item.Actions = new List<Action<ILocalization>>();
                    _registers.Add(item);
                }
                item.Actions.Add(action);
            }
            action(this);
        }

        private void RemoveDead()
        {
            lock (_lockObj)
            {
                for (int i = _registers.Count - 1; i >= 0; i--)
                {
                    if (!_registers[i].Owner.IsAlive)
                    {
                        _registers.RemoveAt(i);
                    }
                }
            }
        }

        public void UnRegisterCultureChangedHandler(object owner)
        {
            lock (_lockObj)
            {
                _registers.RemoveAll(p => p.Owner.Target == owner);
            }
        }

        public string Format(string key, params object[] args)
        {
            string resString = GetResString(key);
            return string.Format(resString, args);
        }
    }

    public class LocalizationActionItem
    {
        public WeakReference Owner { get; set; }
        public List<Action<ILocalization>> Actions { get; set; }
    }
}
