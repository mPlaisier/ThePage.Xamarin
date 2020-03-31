using System;
using MvvmCross.Commands;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBookGenreItem : ICellBook
    {
        Action<CellBookGenreItem> _removeGenre;

        #region Properties

        public Genre Genre { get; }

        #endregion

        #region Commands

        IMvxCommand _DeleteCommand;
        public IMvxCommand DeleteCommand => _DeleteCommand = _DeleteCommand ?? new MvxCommand(() => _removeGenre?.Invoke(this));

        #endregion

        #region Constructor

        public CellBookGenreItem(Genre genre, Action<CellBookGenreItem> removeGenre)
        {
            Genre = genre;
            _removeGenre = removeGenre;
        }

        #endregion
    }

    public class CellBookAddGenre : ICellBook
    {
        Action _addGenre;
        #region Properties

        public string Label => "Voeg genre toe";

        IMvxCommand _addGenreCommand;
        public IMvxCommand AddGenreCommand => _addGenreCommand = _addGenreCommand ?? new MvxCommand(() => _addGenre?.Invoke());

        #endregion

        #region Constructor

        public CellBookAddGenre(Action addGenre)
        {
            _addGenre = addGenre;
        }

        #endregion
    }
}
