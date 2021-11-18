namespace ThePage.Api
{
    public interface ILocalDatabaseService
    {
        T GetData<T>(EApiDataType dataType, string id = null, int? page = null) where T : class;
        void StoreData<T>(T data, EApiDataType dataType, string id = null, int? page = null) where T : class;
        void Clear(EApiDataType dataType, string id = null);
        void ClearAll();
    }
}