using System;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using ThePage.Core;

namespace ThePage.Droid
{
    public class BookShelfTemplateSelector : MvxTemplateSelector<ICell>
    {
        #region Properties

        public override int GetItemLayoutId(int fromViewType) => fromViewType;

        #endregion

        #region Protected

        protected override int SelectItemViewType(ICell forItemObject)
        {
            return forItemObject switch
            {
                BaseCellTitle c => Resource.Layout.cell_base_title,
                BaseCellTextView c => Resource.Layout.cell_base_textview,
                BaseCellClickableText c => Resource.Layout.cell_base_clickabletext,
                BaseCellKeyValueListItem c => Resource.Layout.cell_base_keyvalue_listitem,
                _ => throw new NotSupportedException($"Unknown cell type: {forItemObject} in {nameof(BookShelfTemplateSelector)}"),
            };
        }

        #endregion
    }
}
