using System;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using ThePage.Core;

namespace ThePage.Droid
{
    public class BookTemplateSelector : MvxTemplateSelector<ICell>
    {
        #region Properties

        public override int GetItemLayoutId(int fromViewType) => fromViewType;

        #endregion

        #region Protected

        protected override int SelectItemViewType(ICell forItemObject)
        {
            switch (forItemObject)
            {
                case CellBookTitle c:
                    return Resource.Layout.cell_book_title;
                case CellBookNumberTextView c:
                    return Resource.Layout.cell_book_numbertextview;
                case CellBookTextView c:
                    return Resource.Layout.cell_book_textview;
                case CellBookAuthor c:
                    return Resource.Layout.cell_book_author;
                case CellBookButton c:
                    return Resource.Layout.cell_book_button;
                case CellBookAddGenre c:
                    return Resource.Layout.cell_book_addgenre_item;
                case CellBookGenreItem c:
                    return Resource.Layout.cell_book_genre_item;
                case CellBookSwitch c:
                    return Resource.Layout.cell_book_switch;
                default:
                    throw new NotSupportedException("Unknown cell type");
            }
        }

        #endregion
    }
}
