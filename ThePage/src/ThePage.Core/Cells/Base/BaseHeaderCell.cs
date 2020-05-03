using System;
using MvvmCross.ViewModels;

namespace ThePage.Core
{
    public abstract class BaseHeaderCell : MvxNotifyPropertyChanged
    {
        public virtual string Title { get; protected set; }

        public bool IsOpen { get; set; }

        public virtual string OpenIcon => "ic_chevron_down";

        public virtual string CloseIcon => "ic_chevron_up";
    }
}
