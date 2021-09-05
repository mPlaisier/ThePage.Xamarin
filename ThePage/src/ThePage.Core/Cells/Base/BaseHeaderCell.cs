using MvvmCross.ViewModels;

namespace ThePage.Core
{
    public abstract class BaseHeaderCell : MvxNotifyPropertyChanged
    {
        public virtual string Title { get; protected set; }

        public bool IsOpen { get; set; }

        public virtual string OpenIcon => Constants.ICON_DOWN;

        public virtual string CloseIcon => Constants.ICON_UP;
    }
}
