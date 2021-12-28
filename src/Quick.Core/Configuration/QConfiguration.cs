using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Quick
{
    public class QConfiguration : IQConfiguration
    {
        private string _configFilePath;
        private Encoding _encoding;
        private JObject _jObject;
        private Dictionary<string, object> _objCache;
        public QConfiguration(string strConfigFilePath, Encoding encoding)
        {
            _encoding = encoding;
            _objCache = new Dictionary<string, object>();
            _configFilePath = strConfigFilePath;
            if (File.Exists(strConfigFilePath))
            {
                _jObject = JObject.Parse(File.ReadAllText(_configFilePath, encoding));
            }
            else
            {
                _jObject = new JObject();
            }

        }
        public QConfiguration(string strConfigFilePath) : this(strConfigFilePath, Encoding.UTF8) { }

        public void Save()
        {
            foreach (var pair in _objCache)
            {
                _jObject[pair.Key] = JToken.FromObject(pair.Value);
            }
            string strJson = JsonConvert.SerializeObject(_jObject, Formatting.Indented);
            File.WriteAllText(_configFilePath, strJson, _encoding);
        }

        public void SaveConfig(string sectionName, object configObj)
        {
            _jObject[sectionName] = JToken.FromObject(configObj);
            _objCache[sectionName] = configObj;
        }

        public T GetConfig<T>(string sectionName) where T : class
        {
            return (T)GetConfig(typeof(T), sectionName);
        }

        public T GetOrCreateConfig<T>(string sectionName) where T : class
        {
            return (T)GetOrCreateConfig(typeof(T), sectionName);
        }

        public object GetConfig(Type type, string sectionName)
        {
            if (_objCache.ContainsKey(sectionName))
            {
                return _objCache[sectionName];
            }
            object obj = _jObject[sectionName].ToObject(type);
            _objCache[sectionName] = obj;
            return obj;
        }

        public object GetOrCreateConfig(Type type, string sectionName)
        {
            if (_objCache.ContainsKey(sectionName))
            {
                return _objCache[sectionName];
            }
            if (!_jObject.ContainsKey(sectionName))
            {
                object obj = Activator.CreateInstance(type);
                SaveConfig(sectionName, obj);
                return obj;
            }
            else
            {
                object obj = _jObject[sectionName].ToObject(type);
                _objCache[sectionName] = obj;
                return obj;
            }
        }
    }
}
