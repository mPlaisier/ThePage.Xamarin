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
            return forItemObject switch
            {
                CellBasicBook _ => Resource.Layout.cell_basic_book,
                CellBookTitle _ => Resource.Layout.cell_book_title,
                CellBookNumberTextView _ => Resource.Layout.cell_book_numbertextview,
                CellBookTextView _ => Resource.Layout.cell_book_textview,
                CellBookAuthor _ => Resource.Layout.cell_book_author,
                CellBookButton _ => Resource.Layout.cell_book_button,
                CellBookAddGenre _ => Resource.Layout.cell_book_addgenre_item,
                CellBookGenreItem _ => Resource.Layout.cell_book_genre_item,
                CellBookSwitch _ => Resource.Layout.cell_book_switch,
                _ => throw new NotSupportedException($"Unknown cell type {forItemObject.GetType()}"),
            };
        }

        #endregion
    }
}
