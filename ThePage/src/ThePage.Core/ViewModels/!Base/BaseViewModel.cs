using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;

namespace ThePage.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public abstract string Title { get; }
    }
}
