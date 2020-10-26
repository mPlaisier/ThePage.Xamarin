using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Book
{
    public class BookSelectViewModelTests : BaseViewModelTests
    {
        readonly BookSelectViewModel _vm;

        #region Constructor

        public BookSelectViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<BookSelectViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(List<ApiBook> parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        #endregion

        [Fact]
        public void CountSelectedItemsCountWithBookParameter()
        {
            //Arrange
            MockThePageService
               .Setup(x => x.GetAllBooks())
               .Returns(() => Task.FromResult(BookDataFactory.GetListBook4ElementsComplete()));

            var listSelectedBook = BookDataFactory.GetListBook1ElementsComplete().Docs;
            LoadViewModel(listSelectedBook);

            //Assert
            _vm.SelectedItems.Should().NotBeNullOrEmpty();
            _vm.Items.Should().HaveCount(4);
            _vm.SelectedItems.Should().HaveCount(1);
        }

        [Fact]
        public void SelectedItemsShouldNotBeNulltWithNullParameter()
        {
            //Arrange
            MockThePageService
               .Setup(x => x.GetAllBooks())
               .Returns(() => Task.FromResult(BookDataFactory.GetListBook4ElementsComplete()));
            LoadViewModel(null);

            //Assert
            _vm.SelectedItems.Should().NotBeNull();
        }

        [Fact]
        public void NoCrashWhenNoDataAvailable()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllBooks())
               .Returns(() => Task.FromResult<ApiBookResponse>(null));
            LoadViewModel(null);

            //Assert
            _vm.Items.Should().BeEmpty();
        }

        [Fact]
        public void AddCellToSelectedListWhenClicked()
        {
            //Prepare
            MockThePageService
               .Setup(x => x.GetAllBooks())
               .Returns(() => Task.FromResult(BookDataFactory.GetListBook4ElementsComplete()));
            LoadViewModel(null);

            //Execute
            var book = BookDataFactory.GetCellBookSelect();
            _vm.CommandSelectItem.Execute(book);

            //Assert
            _vm.SelectedItems.Should().Contain(book.Item);
            book.IsSelected.Should().BeTrue();
        }

        [Fact]
        public void RemoveCellToSelectedListWhenClicked()
        {
            //Prepare
            MockThePageService
               .Setup(x => x.GetAllBooks())
               .Returns(() => Task.FromResult(BookDataFactory.GetListBook4ElementsComplete()));
            LoadViewModel(BookDataFactory.GetListBook1ElementsComplete().Docs);

            //Execute
            var book = BookDataFactory.GetCellBookSelect(true);
            _vm.CommandSelectItem.Execute(book);

            //Assert
            _vm.SelectedItems.Should().NotContain(book.Item);
            book.IsSelected.Should().BeFalse();
        }
    }
}
