using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Quick
{
    public abstract class QBindableBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                NotifyPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
        protected virtual bool SetObjectPropertyValue<T>(object storageObj, string storagePropertyName, T value, [CallerMemberName] string propertyName = null)
        {
            Type srcType = storageObj.GetType();
            PropertyInfo pInfo = srcType.GetProperty(storagePropertyName);
            object oldVal = pInfo.GetValue(storageObj);
            if (!object.Equals(oldVal, value))
            {
                pInfo.SetValue(storageObj, value);
                NotifyPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected virtual bool SetObjectPropertyValue<T>(object storageObj, T value, [CallerMemberName] string propertyName = null)
        {
            return SetObjectPropertyValue(storageObj, propertyName, value, propertyName);
        }

        //protected virtual T GetObjectPropertyValue<T>(object storageObj, [CallerMemberName] string propertyName = null)
        //{
        //    Type srcType = storageObj.GetType();
        //    PropertyInfo pInfo = srcType.GetProperty(propertyName);
        //    return (T)pInfo.GetValue(storageObj);
        //}
    }
}

