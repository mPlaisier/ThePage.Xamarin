using System;
using System.Threading.Tasks;
using Moq;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels
{
    public class GenreDetailViewModelTests : BaseServicesTests
    {
        readonly GenreDetailViewModel _vm;

        #region Constructor

        public GenreDetailViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<GenreDetailViewModel>();
        }

        #endregion

        #region Setup

        void LoadGenreDetailViewModel(GenreDetailParameter parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        #endregion

        [Theory]
        [InlineData("Valid name", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void GenreValidationOK(string name, bool isValid)
        {
            //Arrange
            LoadGenreDetailViewModel(new GenreDetailParameter(GenreDataFactory.GetSingleGenre()));

            //Execute
            _vm.TxtName = name;

            //Check
            Assert.Equal(isValid, _vm.IsValid);
        }

        [Fact]
        public void GenreIsValidPropertyChangedTest()
        {
            //Arrange
            LoadGenreDetailViewModel(new GenreDetailParameter(GenreDataFactory.GetSingleGenre()));

            //Execute
            Assert.PropertyChanged(_vm, nameof(_vm.IsValid),
                () => _vm.TxtName = "input");
        }

        [Fact]
        public void GenreViewEditIsDisabledAStart()
        {
            //Arrange
            LoadGenreDetailViewModel(new GenreDetailParameter(GenreDataFactory.GetSingleGenre()));

            //Assert
            Assert.False(_vm.IsEditing);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadingAndEditingToFalseAfterEdit(bool result)
        {
            //TODO only place using ThePageService
            // => Fix to use GenreService!
            //Arrange
            MockThePageService
                .Setup(x => x.UpdateGenre(It.IsAny<string>(), It.IsAny<Api.ApiGenreRequest>()))
                .Returns(() => result ? Task.FromResult(new Api.ApiGenre()) : Task.FromResult<Api.ApiGenre>(null));
            LoadGenreDetailViewModel(new GenreDetailParameter(GenreDataFactory.GetSingleGenre()));

            //Executev
            _vm.UpdateGenreCommand.Execute();

            //Assert
            Assert.False(_vm.IsLoading);
            Assert.False(_vm.IsEditing);
        }
    }
}