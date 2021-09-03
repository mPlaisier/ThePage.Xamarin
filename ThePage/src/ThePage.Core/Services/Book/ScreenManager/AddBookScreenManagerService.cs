using System;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.Cells;

namespace ThePage.Core
{
    [ThePageTypeService]
    public class AddBookScreenManagerService : BaseBookDetailScreenManager, IAddBookScreenManagerService
    {
        Action<string> _actionClose;

        OLObject _olBook;
        string _isbn;

        #region Constructor

        public AddBookScreenManagerService(IMvxNavigationService navigationService,
                                           IUserInteraction userInteraction,
                                           IDevice device,
                                           IGoogleBooksService googleBooksService,
                                           IBookService bookService,
                                           IAuthorService authorService)
            : base(navigationService, userInteraction, device, googleBooksService, authorService, bookService)
        {
        }

        #endregion

        #region Public

        public void Init(AddBookParameter parameter, Action<string> actionClose)
        {
            _isbn = parameter?.ISBN;
            _olBook = parameter?.Book;

            _actionClose = actionClose;
        }

        public override async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            if (_olBook != null)
                await CreateCellBooksFromOlData();
            else
                CreateCellBooks(new BookDetail { ISBN = _isbn }, true);

            IsLoading = false;
            UpdateValidation();
        }

        public override async Task SaveBook()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _bookService.AddBook(Items);
            if (result.IsNotNull())
                _actionClose?.Invoke(result.Id);
            else
                IsLoading = false;
        }

        public override void CreateCellBooks(BookDetail bookDetail, bool isEdit)
        {
            base.CreateCellBooks(bookDetail, isEdit);

            Items.Add(new CellBookButton("Add Book", SaveBook, Enums.EButtonType.Create));
        }

        #endregion

        #region Private

        async Task CreateCellBooksFromOlData()
        {
            var olAuthor = _olBook.Authors.First();
            var olkey = GetAuthorKey(olAuthor?.Url.ToString());

            Author author = null;

            var _authors = await _authorService.GetAuthors();
            if (_authors == null)
            {
                _userInteraction.Alert("Error retrieving data from Server", null, "Error");
                _actionClose?.Invoke(null);
            }
            else
            {
                author = _authors.FirstOrDefault(a => a.Olkey != null && a.Olkey.Equals(olkey));

                if (author == null)
                {
                    author = new Author
                    {
                        Name = olAuthor?.Name,
                        Olkey = olkey
                    };
                    var newAuthor = await SelectOrCreateAuthor(author, olkey);

                    if (newAuthor != null)
                        author = newAuthor;
                    else
                    {
                        _authors = await _authorService.GetAuthors();
                        author = _authors.FirstOrNull(a => a.Olkey != null && a.Olkey.Equals(olkey));
                    }
                }

                var bookDetail = new BookDetail
                {
                    Title = _olBook.Title,
                    Author = author,
                    Pages = _olBook.Pages,
                    ISBN = _isbn
                };

                CreateCellBooks(bookDetail, true);
            }

            static string GetAuthorKey(string key)
            {
                if (key != null)
                {
                    var split = key.Split('/');
                    return split[4];
                }
                return string.Empty;
            }
        }

        #endregion
    }
}
