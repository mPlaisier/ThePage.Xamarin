using System;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using ThePage.Core;

namespace ThePage.Droid
{
    public class MenuTemplateSelector : MvxTemplateSelector<CellMenu>
    {
        #region Properties

        public override int GetItemLayoutId(int fromViewType) => fromViewType;

        #endregion

        #region Protected

        protected override int SelectItemViewType(CellMenu forItemObject)
        {
            switch (forItemObject)
            {
                case CellMenuItem c:
                    return Resource.Layout.cell_menu_item;
                case CellMenuHeader c:
                    return Resource.Layout.cell_menu_header;
                default:
                    throw new NotSupportedException("Unknown cell type");
            }
        }

        #endregion

    }
}
