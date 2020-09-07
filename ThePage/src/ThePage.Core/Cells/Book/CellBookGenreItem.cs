using System;
using MvvmCross.Commands;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBookGenreItem : CellBookInput, ICellBook
    {
        Action<CellBookGenreItem> _removeGenre;

        #region Properties

        public ApiGenre Genre { get; }

        #endregion

        #region Commands

        IMvxCommand _DeleteCommand;
        public IMvxCommand DeleteCommand => _DeleteCommand = _DeleteCommand ?? new MvxCommand(() => _removeGenre?.Invoke(this));

        public override bool IsValid => true;

        public override EBookInputType InputType => EBookInputType.Genre;

        #endregion

        #region Constructor

        public CellBookGenreItem(ApiGenre genre, Action<CellBookGenreItem> removeGenre, bool isEdit = false)
        {
            Genre = genre;
            _removeGenre = removeGenre;
            IsEdit = isEdit;
        }

        #endregion
    }

    public class CellBookAddGenre : ICellBook
    {
        Action _addGenre;
        #region Properties

        public string Label => "Add Genre";

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