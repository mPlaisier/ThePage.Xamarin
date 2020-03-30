using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddBookViewModel : BaseViewModelResult<bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        List<Genre> _genres;
        List<Author> _authors;

        #region Properties

        public override string Title => "Add book";

        MvxObservableCollection<ICellBook> _items;
        public MvxObservableCollection<ICellBook> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        #endregion

        #region Commands

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(() => AddBook().Forget());

        #endregion

        #region Constructor

        public AddBookViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddBookViewModel)}");

            await base.Initialize();

            FetchData().Forget();
        }

        #endregion

        #region Private

        async Task AddBook()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();

            var title = Items.Where(t => t is CellBookTitle).OfType<CellBookTitle>().First().TxtTitle.Trim();
            var author = Items.Where(a => a is CellBookAuthor).OfType<CellBookAuthor>().First().SelectedAuthor;

            var book = new Book(title, author.Id, new List<string>());
            var result = await _thePageService.AddBook(book);

            if (result)
            {
                _userInteraction.ToastMessage("Book added");
                await _navigation.Close(this, true);
            }
            else
            {
                _userInteraction.Alert("Failure adding book");
                IsLoading = false;
            }
        }

        async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            _authors = await _thePageService.GetAllAuthors();
            _genres = await _thePageService.GetAllGenres();

            if (_authors == null || _genres == null)
            {
                _userInteraction.Alert("Error retrieving data from Server", null, "Error");
                await _navigation.Close(this, true);
            }

            CreateCellBooks();

            IsLoading = false;
        }

        void CreateCellBooks()
        {
            Items = new MvxObservableCollection<ICellBook>
            {
                new CellBookTitle(UpdateValidation),
                new CellBookAuthor(_device,_authors, UpdateValidation),
                new CellBookAddGenre(AddGenreAction),
                new CellBookButton(AddBook)
            };
        }

        void UpdateValidation()
        {
            var lstInput = Items.Where(x => x is CellBookInput).OfType<CellBookInput>().ToList();
            var btn = Items.Where(b => b is CellBookButton).OfType<CellBookButton>().First();

            btn.IsValid = lstInput.Where(x => x.IsValid == false).Count() == 0;
        }

        void AddGenreAction()
        {
            //TODO navigate to view with list of all genres
            //=>_genres

            //After await
            var genreItem = new CellBookGenreItem(new Genre("Genre naam"), RemoveGenre);

            var index = Items.FindIndex(x => x is CellBookAddGenre);
            Items.Insert(index, genreItem);
        }

        private void RemoveGenre(CellBookGenreItem obj)
        {
            Items.Remove(obj);
        }

        #endregion
    }
}
