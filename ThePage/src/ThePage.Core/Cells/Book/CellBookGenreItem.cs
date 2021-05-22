using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBookGenreItem : CellBookInput, ICellBook
    {
        readonly Action<CellBookGenreItem> _removeGenre;

        #region Properties

        public ApiGenre Genre { get; }

        #endregion

        #region Commands

        IMvxCommand _DeleteCommand;
        public IMvxCommand DeleteCommand => _DeleteCommand ??= new MvxCommand(() => _removeGenre?.Invoke(this));

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

    public class CellBookAddGenre : BaseCellClickableText, ICellBook
    {
        #region Constructor

        public CellBookAddGenre(Func<Task> action)
            : base("Add Genre", action)
        {
        }

        #endregion
    }
}