using System;
using System.Threading.Tasks;
using Moq;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Genre
{
    public class GenreDetailViewModelTests : BaseViewModelTests
    {
        #region Constructor

        public GenreDetailViewModelTests()
        {
            Setup();
        }

        #endregion

        #region Setup

        GenreDetailViewModel LoadViewModel()
        {
            var vm = Ioc.IoCConstruct<GenreDetailViewModel>();

            return vm;
        }

        #endregion

        [Theory]
        [InlineData("Valid name", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void GenreValidationOK(string name, bool isValid)
        {
            //Arrange
            var vm = LoadViewModel();

            //Execute
            vm.TxtName = name;

            //Check
            Assert.Equal(isValid, vm.IsValid);
        }

        [Fact]
        public void GenreIsValidPropertyChangedTest()
        {
            //Arrange
            var vm = LoadViewModel();

            //Execute
            Assert.PropertyChanged(vm, nameof(vm.IsValid),
                () => vm.TxtName = "input");
        }

        [Fact]
        public void GenreViewEditIsDisabledAtInit()
        {
            //Arrange
            var vm = LoadViewModel();

            //Assert
            Assert.False(vm.IsEditing);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadingAndEditingToFalseAfterEdit(bool result)
        {
            //Arrange
            MockThePageService
                .Setup(x => x.UpdateGenre(It.IsAny<Api.Genre>()))
                .Returns(() => result ? Task.FromResult(new Api.Genre()) : Task.FromResult<Api.Genre>(null));
            var vm = LoadViewModel();

            //Execute
            vm.TxtName = "";
            vm.GenreCell = new CellGenre(new Api.Genre());
            vm.UpdateGenreCommand.Execute();

            //Assert
            Assert.False(vm.IsLoading);
            Assert.False(vm.IsEditing);
        }
    }
}
