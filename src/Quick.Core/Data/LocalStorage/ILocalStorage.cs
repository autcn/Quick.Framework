namespace Quick
{
    public interface ILocalStorage
    {
        object this[string key] { get; set; }
        bool ContainsKey(string key);
        bool Remove(string key);

        TValue GetValue<TValue>(string key);

        TValue GetValueOrDefault<TValue>(string key);
        TValue GetValueOrDefault<TValue>(string key, TValue defaultValue);
        bool TryGetValue<TValue>(string key, out TValue val);

        object GetValueOrDefault(string key);
        object GetValueOrDefault(string key, object defaultValue);
        bool TryGetValue(string key, out object val);

        void Save();
        void Clear();
    }
}
