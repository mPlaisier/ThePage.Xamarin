using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Author
{
    public class AuthorViewModelTests : BaseViewModelTests
    {
        AuthorViewModel _vm;

        #region Constructor

        public AuthorViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AuthorViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel()
        {
            _vm.Initialize();
        }

        #endregion

        [Fact]
        public void ShowAuthorsWhenDataAvailable()
        {
            //Arrange
            MockAuthorService
                .Setup(x => x.GetAuthors())
                .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));

            //Setup
            LoadViewModel();

            //Check
            Assert.NotNull(_vm.Authors);
            Assert.NotEmpty(_vm.Authors);
            Assert.Equal(4, _vm.Authors.Count);
        }

        [Fact]
        public void ShowEmptyAuthorsNoDataAvailable()
        {
            //Arrange
            MockAuthorService
                .Setup(x => x.GetAuthors())
                .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthorEmpty()));

            //Setup
            LoadViewModel();

            //Check
            Assert.NotNull(_vm.Authors);
            Assert.Empty(_vm.Authors);
        }

        [Fact]
        public void ShowNullAuthorsNoDataAvailable()
        {
            //Arrange
            MockAuthorService
                .Setup(x => x.GetAuthors())
                .Returns(() => Task.FromResult<IEnumerable<Core.Author>>(null));

            //Setup
            LoadViewModel();

            //Check
            Assert.Null(_vm.Authors);
        }
    }
}