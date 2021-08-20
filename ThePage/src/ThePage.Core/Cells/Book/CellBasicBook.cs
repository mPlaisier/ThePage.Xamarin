using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using static ThePage.Core.Enums;

namespace ThePage.Core.Cells
{
    public class CellBasicBook : CellBaseBookInputValidation
    {
        readonly Func<string, Task> _searchFunc;
        readonly Func<Author, Task<Author>> _funcSelectAuthor;

        #region Properties

        string _txtInput;
        public string TxtTitle
        {
            get => _txtInput;
            set
            {
                if (SetProperty(ref _txtInput, value))
                    UpdateValidation?.Invoke();
            }
        }

        Author _author;
        public Author Author
        {
            get => _author;
            set
            {
                if (SetProperty(ref _author, value))
                    UpdateValidation?.Invoke();
            }
        }

        public string ImageUri => Images?.GetImageUrl();

        public override bool IsValid => Author != null && !string.IsNullOrWhiteSpace(TxtTitle);

        public ImageLinks Images { get; }

        #endregion

        #region Commands

        IMvxAsyncCommand _commandSearchClick;
        public IMvxAsyncCommand CommandSearchClick => _commandSearchClick ??= new MvxAsyncCommand(HandleSearch);

        IMvxAsyncCommand _commandSelectAuthor;
        public IMvxAsyncCommand CommandSelectAuthor => _commandSelectAuthor ??= new MvxAsyncCommand(HandleSelectAuthor);

        #endregion

        #region Constructor

        public CellBasicBook(string title,
                             Author author,
                             ImageLinks images,
                             Action updateValidation,
                             Func<string, Task> searchFunc,
                             Func<Author, Task<Author>> funcSelectAuthor)
                : base(updateValidation, EBookInputType.BasicInfo)
        {
            _txtInput = title;
            _author = author;
            Images = images;

            _searchFunc = searchFunc;
            _funcSelectAuthor = funcSelectAuthor;
        }

        #endregion

        #region Private

        protected virtual async Task HandleSearch()
        {
            if (!string.IsNullOrWhiteSpace(TxtTitle))
                await _searchFunc(TxtTitle);
        }

        async Task HandleSelectAuthor()
        {
            var result = await _funcSelectAuthor.Invoke(Author);
            if (result != null)
                Author = result;
        }

        #endregion

        public class Builder
        {
            readonly CellBasicBook _cellBasicBook;

            #region Public

            public Builder IsEdit()
            {
                _cellBasicBook.IsEdit = true;
                return this;
            }

            public CellBasicBook Build()
            {
                return _cellBasicBook;
            }

            #endregion

            #region Constructor

            public Builder(string title, Author author, ImageLinks images, Action updateValidation, Func<string, Task> searchFunc, Func<Author, Task<Author>> funcSelectAuthor)
            {
                _cellBasicBook = new CellBasicBook(title, author, images, updateValidation, searchFunc, funcSelectAuthor);
            }

            #endregion
        }
    }
}