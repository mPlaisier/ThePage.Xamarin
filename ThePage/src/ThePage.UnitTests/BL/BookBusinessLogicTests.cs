using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests
{
    public partial class BookBusinessLogicTests
    {
        [Fact]
        public void CheckMappingCompleteListBooksToCellBooks()
        {
            //Setup
            var books = JsonConvert.DeserializeObject<List<Book>>(BookDataComplete);
            var authors = BookFactory.GetCompleteAuthorList();
            var genres = BookFactory.GetCompleteGenreList();

            //Execute
            var bookCell = BookBusinessLogic.BooksToCellBooks(books, authors, genres);

            //Check
            Assert.NotNull(bookCell);
            Assert.NotEmpty(bookCell);
            Assert.Equal(bookCell.Count, books.Count);
        }

        [Fact]
        public void CheckMappingEmptyListBooksToBookCells()
        {
            //Setup
            var books = JsonConvert.DeserializeObject<List<Book>>(BookDataEmpty);
            var authors = BookFactory.GetCompleteAuthorList();
            var genres = BookFactory.GetCompleteGenreList();

            //Execute
            var bookCell = BookBusinessLogic.BooksToCellBooks(books, authors, genres);

            //Check
            Assert.NotNull(bookCell);
            Assert.Empty(bookCell);
        }

        [Fact]
        public void CheckMappingNullListBooksToBookCells()
        {
            //Setup
            List<Book> books = null;
            var authors = BookFactory.GetCompleteAuthorList();
            var genres = BookFactory.GetCompleteGenreList();

            //Execute
            var bookCells = BookBusinessLogic.BooksToCellBooks(books, authors, genres);

            //Check
            Assert.Null(bookCells);
        }

        [Fact]
        public void CheckMappingCompleteListBooksToCellBooksAuthorsAndGenresNull()
        {
            //Setup
            var books = BookFactory.GetCompleteBookList();
            List<Author> authors = null;
            List<Genre> genres = null;

            //Execute
            var bookCell = BookBusinessLogic.BooksToCellBooks(books, authors, genres);

            //Check
            Assert.NotNull(bookCell);
            Assert.NotEmpty(bookCell);
            Assert.Equal(bookCell.Count, books.Count);
        }

        [Fact]
        public void CheckMappingCompleteListBooksToCellBooksGenresNull()
        {
            //Setup
            var books = BookFactory.GetCompleteBookList();
            var authors = BookFactory.GetCompleteAuthorList();
            List<Genre> genres = null;

            //Execute
            var bookCell = BookBusinessLogic.BooksToCellBooks(books, authors, genres);

            //Check
            Assert.NotNull(bookCell);
            Assert.NotEmpty(bookCell);
            Assert.Equal(bookCell.Count, books.Count);
        }

        [Fact]
        public void CheckMappingBooksToBookCellsAllDataNull()
        {
            //Setup
            List<Book> books = null;
            List<Author> authors = null;
            List<Genre> genres = null;

            //Execute
            var bookCells = BookBusinessLogic.BooksToCellBooks(books, authors, genres);

            //Check
            Assert.Null(bookCells);
        }

        static class BookFactory
        {
            #region Public

            public static List<Book> GetCompleteBookList()
            {
                return JsonConvert.DeserializeObject<List<Book>>(BookDataComplete);
            }

            public static List<Author> GetCompleteAuthorList()
            {
                return JsonConvert.DeserializeObject<List<Author>>(AuthorBusinessLogicTests.AuthorDataComplete);
            }

            public static List<Genre> GetCompleteGenreList()
            {
                return JsonConvert.DeserializeObject<List<Genre>>(GenreBusinessLogicTests.GenreDataComplete);
            }

            #endregion
        }
    }
}