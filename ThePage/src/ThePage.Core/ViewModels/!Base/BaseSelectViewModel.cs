using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    /// <summary>
    /// The Base VM to show a view and select <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseSelectViewModel<TParameter, TResult, TObject>
        : BaseViewModel<TParameter, TResult>
        where TObject : ICellBaseSelect<TResult>
    {
        #region Properties

        public abstract List<TObject> Items { get; set; }

        #endregion

        #region Commands

        public abstract IMvxCommand<TObject> CommandSelectItem { get; }

        public virtual IMvxCommand CommandAddItem { get; }

        #endregion

        #region Public

        public abstract Task LoadData();

        #endregion
    }

    /// <summary>
    /// The Base VM to show a view and select a single item of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseSelectSingleItemViewModel<TParameter, TResult, TObject>
        : BaseSelectViewModel<TParameter, TResult, TObject>
         where TObject : ICellBaseSelect<TResult>
    {
        #region Properties

        public abstract TResult SelectedItem { get; internal set; }

        #endregion
    }

    /// <summary>
    /// The Base VM to show a view and select multiple items of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TObject"></typeparam>
    public abstract class BaseSelectMultipleItemsViewModel<TParameter, TResult, TObject, TResultObject>
        : BaseViewModel<TParameter, TResult>
        where TResult : List<TResultObject>
        where TObject : ICellBaseSelect<TResultObject>
    {
        #region Properties

        public abstract List<TObject> Items { get; set; }

        public abstract TResult SelectedItems { get; internal set; }

        #endregion

        #region Commands

        public abstract IMvxCommand<TObject> CommandSelectItem { get; }

        public virtual IMvxCommand CommandAddItem { get; }

        public abstract IMvxCommand CommandConfirm { get; }

        #endregion

        #region Public

        public abstract Task LoadData();

        #endregion
    }
}