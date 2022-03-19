using Android.Graphics;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace ThePage.Droid
{
    public class GridSpacingItemDecoration : RecyclerView.ItemDecoration
    {
        readonly int _spanCount;
        readonly int _spacing;
        readonly bool _includeEdge;

        public GridSpacingItemDecoration(int spanCount, int spacing, bool includeEdge)
        {
            _spanCount = spanCount;
            _spacing = spacing;
            _includeEdge = includeEdge;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            var position = parent.GetChildAdapterPosition(view);
            var column = position % _spanCount;

            if (_includeEdge)
            {
                outRect.Left = _spacing - column * _spacing / _spanCount;
                outRect.Right = (column + 1) * _spacing / _spanCount;

                if (position < _spanCount)
                {
                    outRect.Top = _spacing;
                }
                outRect.Bottom = _spacing;
            }
            else
            {
                outRect.Left = column * _spacing / _spanCount;
                outRect.Right = _spacing - (column + 1) * _spacing / _spanCount;
                if (position >= _spanCount)
                {
                    outRect.Top = _spacing;
                }
            }
        }
    }
}
