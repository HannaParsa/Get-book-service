namespace taaghche.Services
{
    public interface IInMemoryCacheService
    {
        object GetValue(string key);
        void SetValue(string key, object value);
        void Remove(string key);
    }
}
