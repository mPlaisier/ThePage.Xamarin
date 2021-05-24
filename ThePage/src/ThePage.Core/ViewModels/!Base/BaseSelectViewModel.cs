using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace ThePage.Core
{
    /// <summary>
    /// Interface for a viewmodel where user can select multiple items of <typeparamref name="TSelectObject"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSelectObject"></typeparam>
    public interface IBaseSelectSingleItemViewModel<TResult, TSelectObject>
        where TSelectObject : ICellBaseSelect<TResult>
    {
        MvxObservableCollection<TSelectObject> Items { get; set; }

        TResult SelectedItem { get; }
    }

    /// <summary>
    /// Interface for a viewmodel where user can select multiple items of <typeparamref name="TSelectResultObject"/>.
    /// </summary>
    /// <typeparam name="TSelectObject"></typeparam>
    /// <typeparam name="TSelectResultObject"></typeparam>
    public interface IBaseSelectMultipleItemsViewModel<TSelectObject, TSelectResultObject>
         where TSelectObject : ICellBaseSelect<TSelectResultObject>
    {
        MvxObservableCollection<TSelectObject> Items { get; set; }

        List<TSelectResultObject> SelectedItems { get; }
    }
}