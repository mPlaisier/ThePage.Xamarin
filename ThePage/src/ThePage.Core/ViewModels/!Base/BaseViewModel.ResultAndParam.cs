using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;

namespace ThePage.Core.ViewModels
{
    public abstract class BaseViewModel<TParameter, TResult> : BaseViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        public abstract void Prepare(TParameter parameter);
    }

    public abstract class BaseListViewModel<TParameter, TResult> : BaseListViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
