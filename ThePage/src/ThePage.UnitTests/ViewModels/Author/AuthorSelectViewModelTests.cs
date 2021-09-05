using System.Collections.Generic;
using System.Threading.Tasks;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Author
{
    public class AuthorSelectViewModelTests : BaseServicesTests
    {
        AuthorSelectViewModel _vm;

        #region Constructor

        public AuthorSelectViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AuthorSelectViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(AuthorSelectParameter parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        AuthorSelectParameter GetSingleSelectedItemParameter()
        {
            return new AuthorSelectParameter(AuthorDataFactory.GetSingleAuthor());
        }

        #endregion

        [Fact]
        public void CountSelectedItemsCountWithAuthorParameter()
        {
            //Arrange
            MockAuthorService
               .Setup(x => x.GetAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
            LoadViewModel(GetSingleSelectedItemParameter());

            Assert.NotNull(_vm.SelectedItem);
            Assert.Equal(4, _vm.Items.Count);
        }

        [Fact]
        public void CountSelectedItemsCountWithNullParameter()
        {
            //Arrange
            MockAuthorService
               .Setup(x => x.GetAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
            LoadViewModel(new AuthorSelectParameter(null));

            Assert.Null(_vm.SelectedItem);
            Assert.Equal(4, _vm.Items.Count);
        }

        [Fact]
        public void NoCrashWhenNoDataAvailable()
        {
            //Arrange
            MockAuthorService
                .Setup(x => x.GetAuthors())
                .Returns(() => Task.FromResult<IEnumerable<Core.Author>>(null));
            LoadViewModel(GetSingleSelectedItemParameter());

            Assert.Empty(_vm.Items);
        }
    }
}
