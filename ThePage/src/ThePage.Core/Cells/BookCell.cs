using System.Collections.Generic;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class BookCell : MvxNotifyPropertyChanged, IBaseCell
    {
        #region Properties

        public string Id { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }

        public List<Genre> Genres { get; set; }

        #endregion

        #region Constructor

        public BookCell()
        {

        }

        public BookCell(string id, string title, Author author, List<Genre> genres)
        {
            Id = id;
            Title = title;
            Author = author;
            Genres = genres;
        }

        #endregion
    }
}