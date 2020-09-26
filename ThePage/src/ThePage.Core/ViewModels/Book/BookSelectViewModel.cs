using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;

namespace ThePage.Core
{
    public class BookSelectViewModel : BaseSelectMultipleItemsViewModel<List<ApiBook>, List<ApiBook>, CellBookSelect, ApiBook>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        public override string LblTitle => "Select Book";

        public override List<CellBookSelect> Items { get; set; }

        public override List<ApiBook> SelectedItems { get; internal set; }

        #endregion

        #region Commandds

        IMvxCommand<CellBookSelect> _commandSelectItem;
        public override IMvxCommand<CellBookSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellBookSelect>(HandleBookClick);

        IMvxCommand _commandAddItem;
        public override IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddBookViewModel, bool>();
            if (result)
                await LoadData();
        });

        IMvxCommand _commandConfirm;
        public override IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public BookSelectViewModel(IMvxNavigationService navigationService, IThePageService thePageService)
        {
            _navigation = navigationService;
            _thePageService = thePageService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(List<ApiBook> parameter)
        {
            SelectedItems = parameter ?? new List<ApiBook>();
        }

        public override Task Initialize()
        {
            LoadData().Forget();

            return base.Initialize();
        }

        #endregion

        #region Public

        public override async Task LoadData()
        {
            IsLoading = true;

            var books = await _thePageService.GetAllBooks();

            Items = new List<CellBookSelect>();
            books.Docs.ForEach(x => Items.Add(new CellBookSelect(x, SelectedItems.Contains(x))));

            IsLoading = false;
        }

        #endregion

        #region Private

        void HandleBookClick(CellBookSelect cellBook)
        {
            if (cellBook.IsSelected)
            {
                SelectedItems.Remove(cellBook.Item);
                cellBook.IsSelected = false;
            }
            else
            {
                SelectedItems.Add(cellBook.Item);
                cellBook.IsSelected = true;
            }
        }

        void HandleConfirm()
        {
            _navigation.Close(this, SelectedItems);
        }

        #endregion
    }
}