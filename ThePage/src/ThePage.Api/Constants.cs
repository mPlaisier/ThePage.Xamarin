namespace ThePage.Api
{
    public static class Constants
    {
        public const int GenreExpirationTimeInMinutes = 60;
        public const int AuthorExpirationTimeInMinutes = 60;
        public const int BookExpirationTimeInMinutes = 60;

#if DEBUG
        public const string ThePage_Api_Url = "https://thepageapi-stg.herokuapp.com/api";
#else
        public const string ThePage_Api_Url = "https://thepageapi.herokuapp.com/api";
#endif

        public const string OpenLibrary_Api_Url = "http://openlibrary.org/api";
    }
}