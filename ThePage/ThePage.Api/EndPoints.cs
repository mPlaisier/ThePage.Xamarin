using System;
namespace ThePage.Api
{
    static class EndPoints
    {
        //Book
        public const string GetBooks = "books";
        public const string GetBook = "books/";
        public const string AddBook = "books";

        public const string PatchBook = "books/";
        public const string DeleteBook = "books/";

        //Author
        public const string GetAuthors = "authors";
        public const string GetAuthor = "authors/";
        public const string AddAuthor = "authors";

        public const string PatchAuthor = "authors/";
        public const string DeleteAuthor = "authors/";
    }
}
