using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;
using static ThePage.Core.CellBookInput;

namespace ThePage.Core
{
    public class BookDetailParameter
    {
        #region Properties

        public ApiBook Book { get; }

        #endregion

        #region Constructor

        public BookDetailParameter(ApiBook book)
        {
            Book = book;
        }

        #endregion
    }

    public class BookDetailViewModel : BaseViewModel<BookDetailParameter, bool>, INotifyPropertyChanged
    {
        List<Genre> _allGenres;
        List<Author> _allAuthors;

        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        ApiBook _book;

        #region Properties

        public MvxObservableCollection<ICellBook> Items { get; set; } = new MvxObservableCollection<ICellBook>();

        public override string Title => BookDetail != null ? BookDetail.Title : "Book detail";

        public ApiBookDetailResponse BookDetail { get; internal set; }

        public bool IsEditing { get; set; }

        #endregion

        #region Commands

        IMvxCommand _editbookCommand;
        public IMvxCommand EditBookCommand => _editbookCommand ??= new MvxCommand(ToggleEditValue);

        #endregion

        #region Constructor

        public BookDetailViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(BookDetailParameter parameter)
        {
            _book = parameter.Book;
        }

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookDetailViewModel)}");

            await base.Initialize();

            FetchData().Forget();
        }

        #endregion

        #region Private

        async Task UpdateBook()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;

            UpdateBookCellData();

            var result = await _thePageService.UpdateBook(BookDetail);

            if (result != null)
                _userInteraction.ToastMessage("Book updated");
            else
                _userInteraction.Alert("Failure updating book");

            ToggleEditValue();
            IsLoading = false;
        }

        async Task DeleteBook()
        {
            if (IsLoading)
                return;

            var answer = await _userInteraction.ConfirmAsync("Remove book?", "Confirm", "DELETE");
            if (answer)
            {
                IsLoading = true;

                var result = await _thePageService.DeleteBook(BookDetail);

                if (result)
                {
                    _userInteraction.ToastMessage("Book removed");
                    await _navigation.Close(this, true);
                }
                else
                {
                    _userInteraction.Alert("Failure removing book");
                    IsLoading = false;
                }

            }
        }

        async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            BookDetail = await _thePageService.GetBook(_book.Id);

            CreateCellBooks();

            IsLoading = false;
            UpdateValidation();
        }

        async Task AddGenreAction()
        {
            if (!IsEditing)
                return;

            var selectedGenres = Items.OfType<CellBookGenreItem>().Select(i => i.Genre).ToList();
            var genres = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, List<ApiGenre>>(new SelectedGenreParameters(selectedGenres));

            if (genres != null)
            {
                Items.RemoveItems(Items.OfType<CellBookGenreItem>().ToList());

                var genreItems = new List<CellBookGenreItem>();
                genres.ForEach(x => genreItems.Add(new CellBookGenreItem(x, RemoveGenre, true)));

                var index = Items.FindIndex(x => x is CellBookAddGenre);
                Items.InsertRange(index, genreItems);
            }
        }

        void CreateCellBooks()
        {
            Items = new MvxObservableCollection<ICellBook>
            {
                new CellBookTextView("Title",BookDetail.Title, EBookInputType.Title,UpdateValidation),
                new CellBookAuthor(BookDetail.Author,_navigation, _device, UpdateValidation),
                new CellBookTitle("Genres")
            };

            foreach (var item in BookDetail.Genres)
            {
                Items.Add(new CellBookGenreItem(item, RemoveGenre));
            }

            Items.Add(new CellBookNumberTextView("Pages", BookDetail.Pages.ToString(), EBookInputType.Pages, UpdateValidation, true));
            Items.Add(new CellBookNumberTextView("ISBN", BookDetail.ISBN, EBookInputType.ISBN, UpdateValidation, false));
            Items.Add(new CellBookSwitch("Do you own this book?", BookDetail.Owned, EBookInputType.Owned, UpdateValidation));
            Items.Add(new CellBookSwitch("Have you read this book?", BookDetail.Read, EBookInputType.Read, UpdateValidation));
            Items.Add(new CellBookButton("Delete Book", DeleteBook, false));
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.OfType<CellBookInput>().ToList();
            var isValid = lstInput.Where(x => x.IsValid == false).Count() == 0;

            Items.ForEachType<ICellBook, CellBookButton>(x => x.IsValid = isValid);

        }

        void RemoveGenre(CellBookGenreItem obj)
        {
            Items.Remove(obj);
        }

        void ToggleEditValue()
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;

            //Remove update/Delete button
            Items.Remove(Items.OfType<CellBookButton>().First());

            //Set all input or info fields visible
            foreach (var item in Items.OfType<CellBookInput>())
                item.IsEdit = IsEditing;

            if (IsEditing)
            {
                var index = Items.FindIndex(x => x is CellBookNumberTextView y && y.InputType == EBookInputType.Pages);
                Items.Insert(index, new CellBookAddGenre(() => AddGenreAction().Forget()));

                Items.Add(new CellBookButton("Update Book", UpdateBook));
                UpdateValidation();
            }
            else
            {
                Items.Remove(Items.OfType<CellBookAddGenre>().First());
                Items.Add(new CellBookButton("Delete Book", DeleteBook, false));
            }
        }

        void UpdateBookCellData()
        {
            var updatedBook = BookBusinessLogic.CreateBookFromInput(Items, BookDetail.Id);

            BookDetail.Title = updatedBook.Title;
            BookDetail.Author = updatedBook.Author;
            BookDetail.Genres = updatedBook.Genres;
            BookDetail.ISBN = updatedBook.ISBN;
            BookDetail.Owned = updatedBook.Owned;
            BookDetail.Read = updatedBook.Read;
            BookDetail.Pages = updatedBook.Pages;
        }

        #endregion
    }
}