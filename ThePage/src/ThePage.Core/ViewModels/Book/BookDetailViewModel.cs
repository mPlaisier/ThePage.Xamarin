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

        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public MvxObservableCollection<ICellBook> Items { get; set; }

        public override string Title => "Book Detail";

        public CellBook BookCell { get; internal set; }

        public List<Author> Authors { get; set; }

        Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                SetProperty(ref _selectedAuthor, value);
                _device.HideKeyboard();
            }

        }

        bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        #endregion

        #region Commands

        //EditBookCommand
        IMvxCommand _editbookCommand;
        public IMvxCommand EditBookCommand => _editbookCommand ??= new MvxCommand(() =>
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;

            //Remove update/Delete button
            Items.Remove(Items.OfType<CellBookButton>().First());

            //Set all input or info fields visible
            foreach (var item in Items.OfType<CellBookInput>())
            {
                item.IsEdit = IsEditing;
            }

            if (IsEditing)
            {
                SelectedAuthor = BookCell.Author != null ? Authors.FirstOrDefault(a => a.Id == BookCell.Author.Id) : Authors[0];
                Items.Add(new CellBookButton("Update Book", UpdateBook));
                UpdateValidation();
            }
            else
            {
                Items.Add(new CellBookButton("Delete Book", DeleteBook, false));
            }
        });

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
            //if (IsLoading)
            //    return;

            //_device.HideKeyboard();
            //IsLoading = true;

            ////Get data for Book
            //TxtTitle = TxtTitle.Trim();
            //BookCell.Book.Title = TxtTitle;
            //BookCell.Book.Author = SelectedAuthor.Id;
            //BookCell.Book.Genres = Genres.GetIdStrings();

            //var result = await _thePageService.UpdateBook(BookCell.Book);

            //if (result != null)
            //{
            //    //Update view
            //    BookCell.Genres = Genres.ToList();
            //    BookCell.Author = SelectedAuthor;

            //    _userInteraction.ToastMessage("Book updated");
            //    BookCell = BookBusinessLogic.BookToCellBook(result, Authors, _allGenres.ToList());
            //    SelectedAuthor = Authors.FirstOrDefault(a => a.Id == BookCell.Author.Id);
            //}
            //else
            //    _userInteraction.Alert("Failure updating book");

            //IsEditing = false;
            //IsLoading = false;
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

            Authors = await _thePageService.GetAllAuthors();
            _allGenres = await _thePageService.GetAllGenres();

            SelectedAuthor = Authors.FirstOrDefault(a => a.Id == BookCell.Author?.Id);

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
                new CellBookAuthor(SelectedAuthor, _device,Authors, UpdateValidation),
                new CellBookTitle("Genres")
            };

            foreach (var item in BookCell.Genres)
            {
                Items.Add(new CellBookGenreItem(item, RemoveGenre));
            }

            Items.Add(new CellBookAddGenre(() => AddGenreAction().Forget()));
            Items.Add(new CellBookNumberTextView("Pages", BookCell.Book.Pages.ToString(), EBookInputType.Pages, UpdateValidation, false));
            Items.Add(new CellBookTextView("ISBN", BookCell.Book.ISBN, EBookInputType.ISBN, UpdateValidation, false));
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

        #endregion
    }
}