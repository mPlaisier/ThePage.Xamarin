using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBasicBook : CellBookInput
    {
        readonly ImageLinks _images;
        readonly Func<string, Task> _searchFunc;

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

        public string ImageUri => _images.GetImageUrl();

        public override bool IsValid => Author != null && !string.IsNullOrWhiteSpace(TxtTitle);

        public override EBookInputType InputType => EBookInputType.BasicInfo;

        #endregion

        #region Commands

        IMvxAsyncCommand _commandSearchClick;
        public IMvxAsyncCommand CommandSearchClick => _commandSearchClick ??= new MvxAsyncCommand(HandleSearch);

        IMvxAsyncCommand _commandSelectAuthor;
        public IMvxAsyncCommand CommandSelectAuthor => _commandSelectAuthor ??= new MvxAsyncCommand(HandleSelectAuthor);

        #endregion

        #region Constructor

        public CellBasicBook()
        {
        }

        public CellBasicBook(string title, Author author, ImageLinks images, Action updateValidation, Func<string, Task> searchFunc)
        {
            _txtInput = title;
            _author = author;
            _images = images;
            UpdateValidation = updateValidation;
            _searchFunc = searchFunc;
        }

        #endregion

        #region Private

        protected virtual async Task HandleSearch()
        {
            if (!string.IsNullOrWhiteSpace(TxtTitle))
                await _searchFunc(TxtTitle);
        }

        //TODO
        async Task HandleSelectAuthor()
        {
            //_device.HideKeyboard();

            //var result = await _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, ApiAuthor>(new AuthorSelectParameter(Author));
            //if (result != null)
            //    Author = result;
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

            public Builder(string title, Author author, ImageLinks images, Action updateValidation, Func<string, Task> searchFunc)
            {
                _cellBasicBook = new CellBasicBook(title, author, images, updateValidation, searchFunc);
            }

            #endregion
        }
    }
}