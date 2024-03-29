using System;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.Cells;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookSearchViewModel : BaseListViewModel<GoogleBooksResult, GoogleBook>
    {
        readonly IMvxNavigationService _navigationService;
        readonly IGoogleBooksService _googleBooksService;
        readonly IUserInteraction _userInteraction;

        #region Properties

        public override string LblTitle => "Found books";

        public MvxObservableCollection<CellGoogleBook> Items { get; set; }

        #endregion

        #region Commands

        IMvxCommand<CellGoogleBook> _itemClickCommand;
        public IMvxCommand<CellGoogleBook> ItemClickCommand => _itemClickCommand ??= new MvxCommand<CellGoogleBook>((item) =>
        {
            _navigationService.Close(this, item.Book);
        });

        #endregion

        #region Constructor

        public BookSearchViewModel(IMvxNavigationService navigationService, IGoogleBooksService googleBooksService, IUserInteraction userInteraction)
        {
            _navigationService = navigationService;
            _googleBooksService = googleBooksService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region lifeCycle

        public override void Prepare(GoogleBooksResult parameter)
        {
            Items = new MvxObservableCollection<CellGoogleBook>();

            parameter.Books.ForEach(b => Items.Add(new CellGoogleBook(b)));
        }

        #endregion

        public override async Task Search(string input)
        {
            var result = await _googleBooksService.SearchBookByTitle(input);
            if (result == null)
                return;

            if (result.Books.IsNotNullAndHasItems())
            {
                var books = result.Books.Select(b => new CellGoogleBook(b));
                Items.ReplaceWith(books);
            }
            else
            {
                Items.Clear();
                _userInteraction.Alert("No books found");
            }

        }

        public override Task LoadNextPage()
            => throw new NotImplementedException();
    }
}
