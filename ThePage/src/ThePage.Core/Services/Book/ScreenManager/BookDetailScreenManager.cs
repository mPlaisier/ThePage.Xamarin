using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.Cells;
using static ThePage.Core.Enums;

namespace ThePage.Core
{
    [ThePageTypeService]
    public class BookDetailScreenManagerService : BaseBookDetailScreenManager, IBookDetailScreenManagerService
    {
        readonly IBookService _bookService;

        Action _actionClose;
        Book _book;

        #region Properties

        public string Title => BookDetail?.Title;

        public BookDetail BookDetail { get; private set; }

        public bool IsEditing { get; private set; }

        #endregion

        #region Constructor

        public BookDetailScreenManagerService(IMvxNavigationService navigationService,
                                           IUserInteraction userInteraction,
                                           IDevice device,
                                           IBookService bookService)
            : base(navigationService, userInteraction, device)
        {
            _bookService = bookService;
        }

        #endregion

        #region Public

        public void Init(BookDetailParameter parameter, Action close)
        {
            _book = parameter.Book;
            _actionClose = close;
        }

        public override async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            BookDetail = await _bookService.FetchBook(_book.Id);

            CreateCellBooks(BookDetail);

            IsLoading = false;
            UpdateValidation();
        }

        public override async Task SaveBook()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;

            var request = UpdateBookCellData();

            if (request != null)
            {
                await _bookService.UpdateBook(request);
            }

            ToggleEditValue();
            IsLoading = false;
        }

        public override void CreateCellBooks(BookDetail bookDetail)
        {
            var items = new List<ICellBook>
            {
                new CellBookTextView("Title",bookDetail.Title, EBookInputType.Title, UpdateValidation),
                new CellBookAuthor(bookDetail.Author, _navigation, _device, UpdateValidation),
                new CellBookTitle("Genres")
            };

            foreach (var item in bookDetail?.Genres)
            {
                items.Add(new CellBookGenreItem(item, RemoveGenre));
            }

            items.Add(new CellBookNumberTextView("Pages", bookDetail.Pages.ToString(), EBookInputType.Pages, UpdateValidation, false));
            items.Add(new CellBookNumberTextView("ISBN", bookDetail.ISBN, EBookInputType.ISBN, UpdateValidation, false));
            items.Add(new CellBookSwitch("Do you own this book?", bookDetail.Owned, EBookInputType.Owned, UpdateValidation));
            items.Add(new CellBookSwitch("Have you read this book?", bookDetail.Read, EBookInputType.Read, UpdateValidation));
            items.Add(new CellBookButton("Delete Book", DeleteBook, false));

            Items.ReplaceWith(items);
        }

        public void ToggleEditValue()
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;

            //Remove update/Delete button
            Items.Remove(Items.OfType<CellBookButton>().First());

            //Set all input or info fields visible
            foreach (var item in Items.OfType<CellBaseBookInput>())
                item.IsEdit = IsEditing;

            if (IsEditing)
            {
                var index = Items.FindIndex(x => x is CellBookNumberTextView y && y.InputType == EBookInputType.Pages);
                Items.Insert(index, new CellBookAddGenre(AddGenre));

                Items.Add(new CellBookButton("Update Book", SaveBook));
                UpdateValidation();
            }
            else
            {
                Items.Remove(Items.OfType<CellBookAddGenre>().First());
                Items.Add(new CellBookButton("Delete Book", DeleteBook, false));
            }
        }

        #endregion

        #region Protected

        protected override Task AddGenre()
        {
            return !IsEditing
                    ? Task.FromResult(0)
                    : base.AddGenre();
        }

        #endregion

        #region Private

        async Task DeleteBook()
        {
            if (IsLoading)
                return;

            var answer = await _userInteraction.ConfirmAsync("Remove book?", "Confirm", "DELETE");
            if (answer)
            {
                IsLoading = true;

                var result = await _bookService.DeleteBook(BookDetail.Id);
                if (result)
                    _actionClose?.Invoke();
                else
                    IsLoading = false;
            }
        }

        ApiBookDetailRequest UpdateBookCellData()
        {
            //Create update object
            var (updatedBook, author, genres) = BookBusinessLogic.CreateBookDetailRequestFromInput(Items, BookDetail.Id, BookDetail);

            if (updatedBook == null)
                return null;

            if (updatedBook.Title != null)
            {
                BookDetail.Title = updatedBook.Title;
                _book.Title = updatedBook.Title;
                RaisePropertyChanged(nameof(Title));
            }

            BookDetail.Author = author;
            _book.Author = author;

            if (genres == null)
                BookDetail.Genres = new List<Genre>();

            BookDetail.ISBN = updatedBook.ISBN;

            if (updatedBook.Owned.HasValue)
                BookDetail.Owned = updatedBook.Owned.Value;

            if (updatedBook.Read.HasValue)
                BookDetail.Read = updatedBook.Read.Value;

            if (updatedBook.Pages.HasValue)
                BookDetail.Pages = updatedBook.Pages.Value;

            return updatedBook;
        }

        #endregion
    }
}
