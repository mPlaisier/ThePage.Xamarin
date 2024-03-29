using System;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using static ThePage.Core.Enums;

namespace ThePage.Core.Cells
{
    public class CellBookAuthor : CellBaseBookInputValidation
    {
        readonly IDevice _device;
        readonly IMvxNavigationService _navigation;

        #region Properties

        public string LblAuthor => "Author";

        Author _item;
        public Author Item
        {
            get => _item;
            set
            {
                if (SetProperty(ref _item, value))
                    UpdateValidation?.Invoke();
            }
        }

        public override bool IsValid => Item != null;

        #endregion

        #region Commands

        IMvxCommand _commandSelectItem;
        public IMvxCommand CommandSelectItem => _commandSelectItem ??= new MvxCommand(() => HandleSelectItem().Forget());

        #endregion

        #region Constructor

        public CellBookAuthor(IMvxNavigationService navigation, IDevice device, Action updateValidation, bool isEdit = false)
            : base(updateValidation, EBookInputType.Author)
        {
            _device = device;
            _navigation = navigation;
            IsEdit = isEdit;
        }

        public CellBookAuthor(Author selectedAuthor, IMvxNavigationService navigation, IDevice device, Action updateValidation, bool isEdit = false)
            : this(navigation, device, updateValidation, isEdit)
        {
            _item = selectedAuthor;
        }

        #endregion

        #region Private

        async Task HandleSelectItem()
        {
            _device.HideKeyboard();

            var result = await _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, Author>(new AuthorSelectParameter(Item));
            if (result != null)
                Item = result;
        }

        #endregion
    }
}
