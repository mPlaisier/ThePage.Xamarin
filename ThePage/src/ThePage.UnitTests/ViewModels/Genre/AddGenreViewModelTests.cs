using System.Threading.Tasks;
using Moq;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Genre
{
    public class AddGenreViewModelTests : BaseViewModelTests
    {
        #region Constructor

        public AddGenreViewModelTests()
        {
            Setup();
        }

        #endregion

        #region Setup

        AddGenreViewModel LoadViewModel()
        {
            var vm = Ioc.IoCConstruct<AddGenreViewModel>();

            return vm;
        }

        #endregion

        [Theory]
        [InlineData("Valid name", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void GenreValidationOK(string name, bool isValid)
        {
            //Setup
            var vm = LoadViewModel();
            vm.TxtName = name;

            //Check
            Assert.Equal(isValid, vm.IsValid);
        }

        [Fact]
        public void GenreIsValidPropertyChangedTest()
        {
            // Arrange
            var vm = LoadViewModel();

            //Execute
            Assert.PropertyChanged(vm, nameof(vm.IsValid),
                () => vm.TxtName = "input");
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void LoadingAddGenreSuccessfulTest(bool result, bool isLoading)
        {
            MockThePageService
                .Setup(x => x.AddGenre(It.IsAny<Api.Genre>()))
                .Returns(() => Task.FromResult(result));

            // Arrange
            var vm = LoadViewModel();

            vm.TxtName = "";
            vm.AddGenreCommand.Execute();

            //Execute
            Assert.Equal(isLoading, vm.IsLoading);
        }


    }
}
