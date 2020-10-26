using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PropertyChanged;
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

        [SuppressPropertyChangedWarnings]
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
    public abstract class BaseSelectSingleItemViewModel<TParameter, TResult, TSelectObject>
        : BaseSelectViewModel<TParameter, TResult, TSelectObject>
         where TSelectObject : ICellBaseSelect<TResult>
    {
        #region Properties

        [SuppressPropertyChangedWarnings]
        public abstract TResult SelectedItem { get; internal set; }

        #endregion
    }

    /// <summary>
    /// The Base VM to show a view and select multiple items of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSelectObject"></typeparam>
    public abstract class BaseSelectMultipleItemsViewModel<TParameter, TResult, TSelectObject, TSelectResultObject>
        : BaseViewModel<TParameter, TResult>
        where TResult : List<TSelectResultObject>
        where TSelectObject : ICellBaseSelect<TSelectResultObject>
    {
        #region Properties

        [SuppressPropertyChangedWarnings]
        public abstract List<TSelectObject> Items { get; set; }

        [SuppressPropertyChangedWarnings]
        public abstract TResult SelectedItems { get; internal set; }

        #endregion

        #region Commands

        public abstract IMvxCommand<TSelectObject> CommandSelectItem { get; }

        public virtual IMvxCommand CommandAddItem { get; }

        public abstract IMvxCommand CommandConfirm { get; }

        #endregion

        #region Public

        public abstract Task LoadData(string item = default);

        #endregion
    }
}