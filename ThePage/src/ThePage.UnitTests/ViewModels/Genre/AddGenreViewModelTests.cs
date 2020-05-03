using System.Threading.Tasks;
using Moq;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Genre
{
    public class AddGenreViewModelTests : BaseViewModelTests
    {
        AddGenreViewModel _vm;

        #region Constructor

        public AddGenreViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AddGenreViewModel>();
        }

        #endregion

        [Theory]
        [InlineData("Valid name", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void GenreValidationOK(string name, bool isValid)
        {
            //Setup
            _vm.TxtName = name;

            //Check
            Assert.Equal(isValid, _vm.IsValid);
        }

        [Fact]
        public void GenreIsValidPropertyChangedTest()
        {
            //Execute
            Assert.PropertyChanged(_vm, nameof(_vm.IsValid),
                () => _vm.TxtName = "input");
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void LoadingAddGenreSuccessful(bool result, bool isLoading)
        {
            MockThePageService
                .Setup(x => x.AddGenre(It.IsAny<Api.Genre>()))
                .Returns(() => Task.FromResult(result));

            _vm.TxtName = "";
            _vm.AddGenreCommand.Execute();

            //Execute
            Assert.Equal(isLoading, _vm.IsLoading);
        }


    }
}