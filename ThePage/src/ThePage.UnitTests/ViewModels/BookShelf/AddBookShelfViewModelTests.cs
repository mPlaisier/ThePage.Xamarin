using System.Linq;
using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.BookShelf
{
    public class AddBookShelfViewModelTests : BaseServicesTests
    {
        readonly AddBookShelfViewModel _vm;

        #region Constructor

        public AddBookShelfViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AddBookShelfViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel()
        {
            _vm.Initialize();
        }

        #endregion

        [Fact]
        public void CheckItemCountAtStart()
        {
            //Setup
            LoadViewModel();

            //Assert
            _vm.Items.Should().HaveCount(4);
        }

        [Fact]
        public void BtnDisabledAtStart()
        {
            //Setup
            LoadViewModel();

            //Assert
            _vm.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ViewModelIsNotLoadingAtStartup()
        {
            //Setup
            LoadViewModel();

            //Assert
            _vm.IsLoading.Should().BeFalse();
        }

        [Fact]
        public void CheckIfTypesAreCorrectOfCellItemsAtStartup()
        {
            //Setup
            LoadViewModel();

            //Assert
            _vm.Items[0].Should().BeOfType<BaseCellTitle>();
            _vm.Items[1].Should().BeOfType<CellBookShelfTextView>();
            _vm.Items[2].Should().BeOfType<BaseCellTitle>();
            _vm.Items[3].Should().BeOfType<BaseCellClickableText>();
        }

        [Theory]
        [InlineData("a", true)]
        [InlineData("valid", true)]
        [InlineData("abcdefghijklmopqrstuvwxyz123456789", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void BtnIsEnabledWithValidInput(string name, bool isValid)
        {
            //Setup
            LoadViewModel();

            //Execute
            var cellName = _vm.Items.OfType<CellBookShelfTextView>().Where(x => x.InputType == EBookShelfInputType.Name).First();
            cellName.TxtInput = name;

            //Assert
            _vm.IsValid.Should().Be(isValid);

        }
    }
}