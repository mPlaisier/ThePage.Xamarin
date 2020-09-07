using System.Collections.Generic;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    //TODO check if still required
    public class CellBook : MvxNotifyPropertyChanged, ICellBase
    {
        #region Properties

        public Book Book { get; set; }

        public Author Author { get; set; }

        public List<Genre> Genres { get; set; }

        #endregion

        #region Constructor

        public CellBook()
        {

        }

        public CellBook(Book book, Author author, List<Genre> genres)
        {
            Book = book;
            Author = author;
            Genres = genres;
        }

        #endregion
    }
}