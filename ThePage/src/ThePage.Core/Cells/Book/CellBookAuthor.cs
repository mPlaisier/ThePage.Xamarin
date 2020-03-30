using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBookAuthor : CellBookInput
    {
        readonly IDevice _device;

        #region Properties

        public string LblAuthor => "Author:";

        List<Author> _authors;
        public List<Author> Authors
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }

        Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set => SetProperty(ref _selectedAuthor, value);
        }

        public override bool IsValid => SelectedAuthor != null;

        public override EBookInputType InputType => EBookInputType.Author;

        #endregion

        #region Commands

        IMvxCommand<Author> _itemSelectedCommand;
        public IMvxCommand<Author> ItemSelectedCommand => _itemSelectedCommand ??= new MvxCommand<Author>((authorCell) =>
        {
            _device.HideKeyboard();
            SelectedAuthor = authorCell;
        });



        #endregion

        #region Constructor

        public CellBookAuthor(IDevice device, List<Author> authors, Action updateValidation)
        {
            _device = device;
            Authors = authors;
            UpdateValidation = updateValidation;
        }

        #endregion


    }
}
