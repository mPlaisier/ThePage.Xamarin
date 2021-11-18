using System;
using System.Linq;
using MonkeyCache.LiteDB;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class LocalDatabaseService : ILocalDatabaseService
    {
        bool isInitialized = false;

        const int LOCAL_DATA_DEFAULT_TIMOUT_MINUTES = 60;
        const int LOCAL_DATA_TOKENS_TIMOUT_DAYS = 30;

        const string TOKENS_KEY = nameof(TOKENS_KEY);

        const string BOOKS_KEY = nameof(BOOKS_KEY);
        const string BOOKS_DETAIL_KEY = nameof(BOOKS_DETAIL_KEY);

        const string BOOKSHELVES_KEY = nameof(BOOKSHELVES_KEY);
        const string BOOKSHELVES_DETAIL_KEY = nameof(BOOKSHELVES_DETAIL_KEY);

        const string AUTHORS_KEY = nameof(AUTHORS_KEY);
        const string AUTHORS_DETAIL_KEY = nameof(AUTHORS_DETAIL_KEY);

        const string GENRES_KEY = nameof(GENRES_KEY);
        const string GENRES_DETAIL_KEY = nameof(GENRES_DETAIL_KEY);

        #region Public

        public void StoreData<T>(T data, EApiDataType dataType, string id = null, int? page = null) where T : class
        {
            InitDatabase();

            var key = GetKey(dataType, id, page);
            Barrel.Current.Add(key, data, GetLocalTimout(dataType));
        }

        public T GetData<T>(EApiDataType dataType, string id = null, int? page = null) where T : class
        {
            InitDatabase();

            var key = GetKey(dataType, id, page);

            return Barrel.Current.Exists(key) && !Barrel.Current.IsExpired(key)
                ? Barrel.Current.Get<T>(key)
                : null; //TODO or throw custom exception? LocalDataNotFoundException
        }

        public void Clear(EApiDataType dataType, string id)
        {
            InitDatabase();

            var key = GetKey(dataType, id);

            //For list multiple keys can exists depending on the page
            //Find them all and clear them
            var barrelKeys = Barrel.Current.GetKeys();
            var dataKeys = barrelKeys.Where(x => x.Contains(key)).ToArray();

            if (dataKeys.Count() != 0)
                Barrel.Current.Empty(dataKeys);
        }

        public void ClearAll()
        {
            InitDatabase();

            Barrel.Current.EmptyAll();
        }

        #endregion

        #region Private

        string GetKey(EApiDataType dataType, string id = null, int? page = null)
        {
            var key = dataType switch
            {
                //Tokens
                EApiDataType.Tokens => TOKENS_KEY,

                //Books
                EApiDataType.BookList => BOOKS_KEY + (page ?? 1),
                EApiDataType.BookDetail => BOOKS_DETAIL_KEY + id,

                //BookShelves
                EApiDataType.BookShelfList => BOOKSHELVES_KEY + (page ?? 1),
                EApiDataType.BookShelfDetail => BOOKSHELVES_DETAIL_KEY + id,

                //Authors
                EApiDataType.AuthorList => AUTHORS_KEY + (page ?? 1),
                EApiDataType.AuthorDetail => AUTHORS_DETAIL_KEY + id,

                //Genre
                EApiDataType.GenreList => GENRES_KEY + (page ?? 1),
                EApiDataType.GenreDetail => GENRES_DETAIL_KEY + id,
                _ => throw new ArgumentException($"Please add {dataType} to the keys."),
            };

            return key;
        }

        TimeSpan GetLocalTimout(EApiDataType dataType)
        {
            var timeSpan = dataType switch
            {
                EApiDataType.Tokens => TimeSpan.FromDays(LOCAL_DATA_TOKENS_TIMOUT_DAYS),
                //Default
                _ => TimeSpan.FromMinutes(LOCAL_DATA_DEFAULT_TIMOUT_MINUTES)
            };

            return timeSpan;
        }

        void InitDatabase()
        {
            if (!isInitialized)
            {
                isInitialized = true;

                Barrel.ApplicationId = "thepageapplication";
                Barrel.EncryptionKey = "encryptionKey";

                //LiteDB Upgrade (4 -> 5) | NuGet 1.3 -> 1.5
                Barrel.Upgrade = true;
            }
        }

        #endregion
    }

    public enum EApiDataType
    {
        //Auth
        Tokens,

        //Books
        BookList,
        BookDetail,

        //BookShelves
        BookShelfList,
        BookShelfDetail,

        //Authors
        AuthorList,
        AuthorDetail,

        //Genres
        GenreList,
        GenreDetail
    }

    public class LocalDataNotFoundException : Exception
    {

    }
}
