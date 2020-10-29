using System;
using System.ComponentModel;
using AndroidX.RecyclerView.Widget;

namespace ThePage.Droid
{
    public class OnScrollListener : RecyclerView.OnScrollListener, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        bool _bottomReached;
        public bool BottomReached
        {
            get => _bottomReached;
            set
            {
                if (value != _bottomReached)
                {
                    _bottomReached = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BottomReached)));
                }
            }
        }

        #endregion

        #region Public

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            recyclerView.Post(() =>
            {
                BottomReached = !recyclerView.CanScrollVertically(1);
            });
        }

        #endregion
    }
}
