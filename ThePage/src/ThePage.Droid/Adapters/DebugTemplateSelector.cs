using System;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using ThePage.Core;

namespace ThePage.Droid
{
    public class DebugTemplateSelector : MvxTemplateSelector<CellDebug>
    {
        #region Properties

        public override int GetItemLayoutId(int fromViewType) => fromViewType;

        #endregion

        #region Protected

        protected override int SelectItemViewType(CellDebug forItemObject)
        {
            switch (forItemObject)
            {
                case CellDebugHeader c:
                    return Resource.Layout.cell_debug_header;
                case CellDebugItem c:
                    return Resource.Layout.cell_debug_item;
                default:
                    throw new NotSupportedException("Unknown cell type");
            }
        }

        #endregion

    }
}
