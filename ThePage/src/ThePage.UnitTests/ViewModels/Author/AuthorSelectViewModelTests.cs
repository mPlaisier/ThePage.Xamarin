using System.Threading.Tasks;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Author
{
    public class AuthorSelectViewModelTests : BaseViewModelTests
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
            MockThePageService
               .Setup(x => x.GetAllAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
            LoadViewModel(GetSingleSelectedItemParameter());

            Assert.NotNull(_vm.SelectedItem);
            Assert.Equal(4, _vm.Items.Count);
        }

        [Fact]
        public void CountSelectedItemsCountWithNullParameter()
        {
            //Arrange
            MockThePageService
               .Setup(x => x.GetAllAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
            LoadViewModel(new AuthorSelectParameter(null));

            Assert.Null(_vm.SelectedItem);
            Assert.Equal(4, _vm.Items.Count);
        }

        [Fact]
        public void NoCrashWhenNoDataAvailable()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllAuthors())
                .Returns(() => Task.FromResult<ApiAuthorResponse>(null));
            LoadViewModel(GetSingleSelectedItemParameter());

            Assert.Empty(_vm.Items);
        }
    }
}
