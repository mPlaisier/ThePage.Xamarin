using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ThePage.Core;
using ThePage.Core.Cells;
using Xunit;
using static ThePage.Core.Enums;

namespace ThePage.UnitTests.ViewModels.Book
{
    public class AddBookViewModelTests : BaseServicesTests
    {
        readonly AddBookViewModel _vm;

        #region Constructor

        public AddBookViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AddBookViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(AddBookParameter parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        void PrepareAuthorData()
        {
            MockAuthorService
               .Setup(x => x.GetAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
        }

        #endregion





    }
}