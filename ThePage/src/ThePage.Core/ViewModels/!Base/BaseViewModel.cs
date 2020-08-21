using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;

namespace ThePage.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public abstract string LblTitle { get; }

        bool _isLoading;
        public virtual bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
    }
}
