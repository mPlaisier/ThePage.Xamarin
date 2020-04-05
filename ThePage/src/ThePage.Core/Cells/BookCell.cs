using System.Collections.Generic;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class BookCell : MvxNotifyPropertyChanged, IBaseCell
    {
        #region Properties

        public Book Book { get; set; }

        public Author Author { get; set; }

        public List<Genre> Genres { get; set; }

        #endregion

        #region Constructor

        public BookCell()
        {

        }

        public BookCell(Book book, Author author, List<Genre> genres)
        {
            Book = book;
            Author = author;
            Genres = genres;
        }

        #endregion
    }
}