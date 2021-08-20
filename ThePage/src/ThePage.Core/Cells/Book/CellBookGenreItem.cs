using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using static ThePage.Core.Enums;

namespace ThePage.Core.Cells
{
    public class CellBookGenreItem : CellBaseBookInput, ICellBook
    {
        readonly Action<CellBookGenreItem> _removeGenre;

        #region Properties

        public Genre Genre { get; }

        #endregion

        #region Commands

        IMvxCommand _DeleteCommand;
        public IMvxCommand DeleteCommand => _DeleteCommand ??= new MvxCommand(() => _removeGenre?.Invoke(this));

        public override bool IsValid => true;

        #endregion

        #region Constructor

        public CellBookGenreItem(Genre genre, Action<CellBookGenreItem> removeGenre, bool isEdit = false)
            : base(EBookInputType.Genre)
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