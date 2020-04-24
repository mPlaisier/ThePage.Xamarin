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

        public CellBook Book { get; }

        #endregion

        #region Constructor

        public BookDetailParameter(CellBook book)
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

        #region Properties

        public MvxObservableCollection<ICellBook> Items { get; set; }

        public override string Title => BookCell.Book != null ? BookCell.Book.Title : "Book detail";

        public CellBook BookCell { get; internal set; }

        public bool IsEditing { get; set; }

        #endregion

        #region Commands

        //EditBookCommand
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
            BookCell = parameter.Book;
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

            var result = await _thePageService.UpdateBook(BookCell.Book);

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

                var result = await _thePageService.DeleteBook(BookCell.Book);

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

            _allAuthors = await _thePageService.GetAllAuthors();
            _allGenres = await _thePageService.GetAllGenres();

            CreateCellBooks();

            IsLoading = false;
            UpdateValidation();
        }

        async Task AddGenreAction()
        {
            if (!IsEditing)
                return;

            var selectedGenres = Items.OfType<CellBookGenreItem>().Select(i => i.Genre).ToList();
            var genre = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, Genre>(new SelectedGenreParameters(_allGenres, selectedGenres));

            if (genre != null)
            {
                var genreItem = new CellBookGenreItem(genre, RemoveGenre);

                var index = Items.FindIndex(x => x is CellBookAddGenre);
                Items.Insert(index, genreItem);
            }
        }

        void CreateCellBooks()
        {
            Items = new MvxObservableCollection<ICellBook>
            {
                new CellBookTextView("Title",BookCell.Book.Title, EBookInputType.Title,UpdateValidation),
                new CellBookAuthor(BookCell.Author, _device,_allAuthors, UpdateValidation),
                new CellBookTitle("Genres")
            };

            foreach (var item in BookCell.Genres)
            {
                Items.Add(new CellBookGenreItem(item, RemoveGenre));
            }

            Items.Add(new CellBookNumberTextView("Pages", BookCell.Book.Pages.ToString(), EBookInputType.Pages, UpdateValidation, true));
            Items.Add(new CellBookNumberTextView("ISBN", BookCell.Book.ISBN, EBookInputType.ISBN, UpdateValidation, false));
            Items.Add(new CellBookSwitch("Do you own this book?", BookCell.Book.Owned, EBookInputType.Owned, UpdateValidation));
            Items.Add(new CellBookSwitch("Have you read this book?", BookCell.Book.Read, EBookInputType.Read, UpdateValidation));
            Items.Add(new CellBookButton("Delete Book", DeleteBook, false));
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.OfType<CellBookInput>().ToList();
            var isValid = lstInput.Where(x => x.IsValid == false).Count() == 0;

            foreach (var item in Items.OfType<CellBookButton>())
                item.IsValid = isValid;
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
            var updatedBook = BookBusinessLogic.CreateBookFromInput(Items, BookCell.Book.Id);
            var updatedGenres = GenreBusinessLogic.GetGenresFromString(updatedBook.Genres, _allGenres).ToList();
            var updatedAuthor = AuthorBusinessLogic.GetAuthorFromString(updatedBook.Author, _allAuthors);

            BookCell.Book.Title = updatedBook.Title;
            BookCell.Book.Author = updatedBook.Author;
            BookCell.Book.Genres = updatedBook.Genres;
            BookCell.Book.ISBN = updatedBook.ISBN;
            BookCell.Book.Owned = updatedBook.Owned;
            BookCell.Book.Read = updatedBook.Read;
            BookCell.Book.Pages = updatedBook.Pages;

            BookCell.Author = updatedAuthor;
            BookCell.Genres = updatedGenres;
        }

        #endregion
    }
}