using System;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBookAuthor : CellBookInput
    {
        readonly IDevice _device;
        readonly IMvxNavigationService _navigation;

        #region Properties

        public string LblAuthor => "Author";

        ApiAuthor _item;
        public ApiAuthor Item
        {
            get => _item;
            set
            {
                if (SetProperty(ref _item, value))
                    UpdateValidation?.Invoke();
            }
        }

        public override bool IsValid => Item != null;

        public override EBookInputType InputType => EBookInputType.Author;

        #endregion

        #region Commands

        IMvxCommand _commandSelectItem;
        public IMvxCommand CommandSelectItem => _commandSelectItem ??= new MvxCommand(() => HandleSelectItem().Forget());

        #endregion

        #region Constructor

        public CellBookAuthor(IMvxNavigationService navigation, IDevice device, Action updateValidation, bool isEdit = false)
        {
            _device = device;
            _navigation = navigation;
            UpdateValidation = updateValidation;
            IsEdit = isEdit;
        }

        public CellBookAuthor(ApiAuthor selectedAuthor, IMvxNavigationService navigation, IDevice device, Action updateValidation, bool isEdit = false)
            : this(navigation, device, updateValidation, isEdit)
        {
            Item = selectedAuthor;
        }

        #endregion

        #region Private

        async Task HandleSelectItem()
        {
            _device.HideKeyboard();

            var result = await _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, ApiAuthor>(new AuthorSelectParameter(Item));
            if (result != null)
                Item = result;
        }

        #endregion
    }
}
