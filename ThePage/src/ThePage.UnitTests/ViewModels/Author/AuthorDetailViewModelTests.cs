using System;
using System.Threading.Tasks;
using Moq;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Author
{
    public class AuthorDetailViewModelTests : BaseViewModelTests
    {
        AuthorDetailViewModel _vm;

        #region Constructor

        public AuthorDetailViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AuthorDetailViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(AuthorDetailParameter parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        #endregion

        [Theory]
        [InlineData("Valid name", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void AuthorValidationOK(string name, bool isValid)
        {
            //Arrange
            LoadViewModel(new AuthorDetailParameter(AuthorDataFactory.GetSingleCellAuthor()));

            //Execute
            _vm.TxtName = name;

            //Check
            Assert.Equal(isValid, _vm.IsValid);
        }

        [Fact]
        public void AuthorIsValidPropertyChangedTest()
        {
            //Arrange
            LoadViewModel(new AuthorDetailParameter(AuthorDataFactory.GetSingleCellAuthor()));

            //Execute
            Assert.PropertyChanged(_vm, nameof(_vm.IsValid),
                () => _vm.TxtName = "input");
        }

        [Fact]
        public void AuthorViewEditIsDisabledAStart()
        {
            //Arrange
            LoadViewModel(new AuthorDetailParameter(AuthorDataFactory.GetSingleCellAuthor()));

            //Assert
            Assert.False(_vm.IsEditing);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadingAndEditingToFalseAfterEdit(bool result)
        {
            //Arrange
            MockThePageService
                .Setup(x => x.UpdateAuthor(It.IsAny<Api.Author>()))
                .Returns(() => result ? Task.FromResult(new Api.Author()) : Task.FromResult<Api.Author>(null));
            LoadViewModel(new AuthorDetailParameter(AuthorDataFactory.GetSingleCellAuthor()));

            //Execute
            _vm.UpdateAuthorCommand.Execute();

            //Assert
            Assert.False(_vm.IsLoading);
            Assert.False(_vm.IsEditing);
        }
    }
}
