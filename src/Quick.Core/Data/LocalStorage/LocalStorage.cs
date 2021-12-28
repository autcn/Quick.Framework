using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Quick
{
    public class LocalStorage : ILocalStorage, IInitializable
    {
        public const string LocalStorageConfigSectionName = "localStorage";
        private string _filePath;
        private readonly IQConfiguration _qConfiguration;
        private JObject _storage;
        private Task _saveTask;
        private long _lastUpdateStamp = 0;
        private bool _storageInConfig = false;

        public LocalStorage(IQConfiguration configuration, string filePth)
        {
            _qConfiguration = configuration;
            _filePath = filePth;
            _storageInConfig = _filePath.IsNullOrEmpty();
        }

        public void Initialize()
        {
            if (_storageInConfig)
            {
                _storage = _qConfiguration.GetOrCreateConfig<JObject>(LocalStorageConfigSectionName);
            }
            else
            {
                if (File.Exists(_filePath))
                {
                    string strJson = File.ReadAllText(_filePath, Encoding.UTF8);
                    _storage = JsonConvert.DeserializeObject<JObject>(strJson);
                }
                else
                {
                    _storage = new JObject();
                }
            }
        }

        public object this[string key]
        {
            get => (_storage[key] as JValue).Value;
            set
            {
                if (!_storage.ContainsKey(key))
                {
                    JValue jValue = new JValue(value);
                    _storage[key] = jValue;
                }
                else
                {
                    (_storage[key] as JValue).Value = value;
                }
                SaveAsync();
            }
        }

        public bool ContainsKey(string key) => _storage.ContainsKey(key);

        public bool Remove(string key)
        {
            bool exist = _storage.Remove(key);
            SaveAsync();
            return exist;
        }

        public bool TryGetValue<TValue>(string key, out TValue val)
        {
            if (_storage.TryGetValue(key, out JToken item))
            {
                val = (item as JValue).Value<TValue>();
                return true;
            }
            val = default(TValue);
            return false;
        }

        public TValue GetValue<TValue>(string key)
        {
            return (_storage[key] as JValue).Value<TValue>();
        }

        public TValue GetValueOrDefault<TValue>(string key)
        {
            return GetValueOrDefault(key, default(TValue));
        }

        public TValue GetValueOrDefault<TValue>(string key, TValue defaultValue)
        {
            if (_storage.TryGetValue(key, out JToken item))
            {
                return (item as JValue).Value<TValue>();
            }
            return defaultValue;
        }

        public object GetValueOrDefault(string key)
        {
            return GetValueOrDefault(key, null);
        }

        public object GetValueOrDefault(string key, object defaultValue)
        {
            if (_storage.TryGetValue(key, out JToken item))
            {
                return (item as JValue).Value;
            }
            return defaultValue;
        }

        public bool TryGetValue(string key, out object val)
        {
            if (_storage.TryGetValue(key, out JToken item))
            {
                val = (item as JValue).Value;
                return true;
            }
            val = null;
            return false;
        }

        public void Save()
        {
            if (_storageInConfig)
            {
                _qConfiguration.Save();
            }
            else
            {
                string strJson = JsonConvert.SerializeObject(_storage, Formatting.Indented);
                File.WriteAllText(_filePath, strJson, Encoding.UTF8);
            }
        }

        private void SaveAsync()
        {
            //记录最新的时间戳
            Interlocked.Exchange(ref _lastUpdateStamp, DateTimeOffset.Now.ToUnixTimeMilliseconds());
            if (_saveTask != null && _saveTask.Status == TaskStatus.Running)
            {
                return;
            }
            _saveTask = Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000);
                    long curTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    if (curTime - _lastUpdateStamp <= 1800)
                    {
                        continue;
                    }
                    Save();
                    _saveTask = null;
                    return;
                }
            });
        }

        public void Clear()
        {
            _storage.RemoveAll();
            SaveAsync();
        }
    }

}
