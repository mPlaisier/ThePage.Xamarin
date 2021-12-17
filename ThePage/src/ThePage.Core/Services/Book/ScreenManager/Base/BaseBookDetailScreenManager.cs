using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.Cells;
using static ThePage.Core.Enums;

namespace ThePage.Core
{
    public abstract class BaseBookDetailScreenManager : MvxNotifyPropertyChanged, IBaseBookDetailScreenManager
    {
        protected readonly IMvxNavigationService _navigation;
        protected readonly IUserInteraction _userInteraction;
        protected readonly IDevice _device;
        protected readonly IGoogleBooksService _googleBooksService;
        protected readonly IAuthorService _authorService;
        protected readonly IBookService _bookService;

        #region Properties

        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            internal set
            {
                if (value.Equals(_isLoading))
                    return;

                _isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        public MvxObservableCollection<ICellBook> Items { get; internal set; }
            = new MvxObservableCollection<ICellBook>();

        public BookDetail BookDetail { get; internal set; }

        #endregion

        #region Constructor

        protected BaseBookDetailScreenManager(IMvxNavigationService navigationService,
                                              IUserInteraction userInteraction,
                                              IDevice device,
                                              IGoogleBooksService googleBooksService,
                                              IAuthorService authorService,
                                              IBookService bookService)
        {
            _navigation = navigationService;
            _userInteraction = userInteraction;
            _device = device;
            _googleBooksService = googleBooksService;
            _authorService = authorService;
            _bookService = bookService;
        }

        #endregion

        #region public abstract

        public virtual void CreateCellBooks(BookDetail bookDetail, bool isEdit)
        {
            var items = new MvxObservableCollection<ICellBook>
            {
                new CellBasicBook.Builder(bookDetail.Title, bookDetail.Author, bookDetail.Images, UpdateValidation, SearchForBookTitle, SelectAuthor).IsEdit(isEdit).Build(),
                new CellBookTitle("Genres")
            };

            foreach (var item in bookDetail?.Genres)
            {
                items.Add(new CellBookGenreItem(item, RemoveGenre, isEdit));
            }

            if (isEdit)
                items.Add(new CellBookAddGenre(AddGenre));

            items.Add(new CellBookNumberTextView.NumberTextViewBuilder("Pages", EBookInputType.Pages, UpdateValidation)
                                                .IsEdit(isEdit).SetValue(bookDetail.Pages.ToString()).NotRequired()
                                                .Build());
            items.Add(new CellBookNumberTextView.NumberTextViewBuilder("ISBN", EBookInputType.ISBN, UpdateValidation)
                                                .IsEdit(isEdit).SetValue(bookDetail.ISBN).NotRequired()
                                                .AllowSearch(SearchForBookIsbn)
                                                .Build());
            items.Add(new CellBookSwitch("Do you own this book?", bookDetail.Owned, EBookInputType.Owned, UpdateValidation, isEdit));
            items.Add(new CellBookSwitch("Have you read this book?", bookDetail.Read, EBookInputType.Read, UpdateValidation, isEdit));

            Items.ReplaceWith(items);
        }

        public virtual Task DeleteBook()
        {
            return Task.FromResult(0);
        }

        public abstract Task SaveBook();

        public abstract Task FetchData();

        #endregion

        #region Protected

        protected void UpdateValidation()
        {
            if (Items.IsNullOrEmpty())
                return;

            var lstInput = Items.OfType<CellBaseBookInput>();
            var isValid = lstInput.All(x => x.IsValid);

            Items.ForEachType<ICellBook, CellBookButton>(x => x.IsValid = isValid);
            RaisePropertyChanged(nameof(Items));
        }

        protected virtual async Task AddGenre()
        {
            var selectedGenres = Items.OfType<CellBookGenreItem>().Select(i => i.Genre).ToList();
            var genres = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, List<Genre>>(
                                                    new SelectedGenreParameters(selectedGenres));

            if (genres.IsNotNull())
            {
                //Remove all old genres:
                Items.RemoveItems(Items.OfType<CellBookGenreItem>().ToList());

                var genreItems = new List<CellBookGenreItem>();
                genres.ForEach(x => genreItems.Add(new CellBookGenreItem(x, RemoveGenre, true)));

                var index = Items.FindIndex(x => x is CellBookAddGenre);
                Items.InsertRange(index, genreItems);

                await RaisePropertyChanged(nameof(Items));
            }
        }

        protected void RemoveGenre(CellBookGenreItem obj)
        {
            Items.Remove(obj);
            RaisePropertyChanged(nameof(Items));
        }

        protected async Task SearchForBookTitle(string title)
        {
            _device.HideKeyboard();
            IsLoading = true;

            var result = await _googleBooksService.SearchBookByTitle(title);
            await HandleSearchResults(result).ConfigureAwait(false);

            IsLoading = false;
        }

        protected async Task HandleSearchResults(GoogleBooksResult result)
        {
            if (result == null)
                return;

            if (result.Books.IsNotNullAndHasItems())
            {
                var book = await _navigation.Navigate<BookSearchViewModel, GoogleBooksResult, GoogleBook>(result);

                if (book != null)
                {
                    var title = book.VolumeInfo.Title;
                    var author = await SelectOrCreateAuthor(new Author(book.VolumeInfo.Authors.First())).ConfigureAwait(false);

                    var pages = book.VolumeInfo.PageCount;
                    var isbn = book.VolumeInfo.IndustryIdentifiers.First().Identifier;

                    var bookDetail = new BookDetail
                    {
                        Title = title,
                        Author = author,
                        Pages = pages,
                        ISBN = isbn,
                        Images = ImageBusinessLogic.MapImages(book.VolumeInfo.ImageLinks)
                    };

                    CreateCellBooks(bookDetail, true);
                    UpdateValidation();
                }
            }
            else
            {
                _userInteraction.Alert("No books found");
            }
        }

        protected async Task SearchForBookIsbn(string isbn)
        {
            _device.HideKeyboard();
            IsLoading = true;

            var result = await _googleBooksService.SearchBookByISBN(isbn);
            await HandleSearchResults(result);

            IsLoading = false;
        }

        protected async Task<Author> SelectOrCreateAuthor(Author author)
        {
            return await SelectOrCreateAuthor(author, null).ConfigureAwait(false);
        }

        protected async Task<Author> SelectOrCreateAuthor(Author author, string olKey)
        {
            Author newAuthor = null;

            var searchAuthors = await _authorService.Search(author.Name);
            if (searchAuthors.Count() == 1)
                newAuthor = searchAuthors.First();
            else
            {
                var userChoice = await _userInteraction.ConfirmThreeButtonsAsync($"{author.Name} is not found in your author list. Would you like to add it?",
                                                                             neutral: "Choose from list");

                if (userChoice == ConfirmThreeButtonsResponse.Positive)
                {
                    newAuthor = await _navigation.Navigate<AddAuthorViewModel, Author, Author>(author);
                }
                //Select author from list
                else if (userChoice == ConfirmThreeButtonsResponse.Neutral)
                {
                    newAuthor = await _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, Author>(new AuthorSelectParameter());

                    if (olKey.IsNotNull())
                    {
                        newAuthor.Olkey = olKey;
                        newAuthor = await _authorService.UpdateAuthor(newAuthor);
                    }
                }
            }

            return newAuthor;
        }

        protected Task<Author> SelectAuthor(Author author)
        {
            _device.HideKeyboard();

            return _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, Author>(new AuthorSelectParameter(author));
        }

        #endregion
    }
}