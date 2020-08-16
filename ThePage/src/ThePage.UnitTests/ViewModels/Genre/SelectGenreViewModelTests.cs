using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Genre
{
    public class SelectGenreViewModelTests : BaseViewModelTests
    {
        SelectGenreViewModel _vm;

        #region Constructor

        public SelectGenreViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<SelectGenreViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(SelectedGenreParameters parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        SelectedGenreParameters GetSingleSelectedItemParameter()
        {
            var list = new List<ApiGenre>()
            {
                GenreDataFactory.GetSingleGenre()
            };
            return new SelectedGenreParameters(list);
        }

        #endregion

        [Fact]
        public void CountSelectedItemsCount()
        {
            //Arrange
            MockThePageService
               .Setup(x => x.GetAllGenres())
               .Returns(() => Task.FromResult(GenreDataFactory.GetListGenre4ElementsComplete()));
            LoadViewModel(GetSingleSelectedItemParameter());

            Assert.Single(_vm.SelectedItems);
            Assert.Equal(4, _vm.Items.Count);
        }


        [Fact]
        public void CommandSelectItemGenreRemovedTest()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllGenres())
                .Returns(() => Task.FromResult(GenreDataFactory.GetListGenre4ElementsComplete()));
            LoadViewModel(GetSingleSelectedItemParameter());

            var selectedGenre = _vm.Items.Where(x => x.IsSelected == true).FirstOrDefault();
            _vm.CommandSelectItem.Execute(selectedGenre);

            var selectedGenres = _vm.Items.Where(x => x.IsSelected == true).ToList();
            Assert.Empty(selectedGenres);
            Assert.Empty(_vm.SelectedItems);
        }


        [Fact]
        public void CommandSelectItemGenreAddTest()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllGenres())
                .Returns(() => Task.FromResult(GenreDataFactory.GetListGenre4ElementsComplete()));
            LoadViewModel(GetSingleSelectedItemParameter());

            var selectedGenre = _vm.Items.Where(x => x.IsSelected == false).FirstOrDefault();
            _vm.CommandSelectItem.Execute(selectedGenre);

            var selectedGenres = _vm.Items.Where(x => x.IsSelected == true).ToList();
            Assert.Equal(2, selectedGenres.Count);
            Assert.Equal(2, _vm.SelectedItems.Count);
        }


        [Fact]
        public void NoCrashWhenNoDataAvailable()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllGenres())
                .Returns(() => Task.FromResult<ApiGenreResponse>(null));
            LoadViewModel(GetSingleSelectedItemParameter());

            Assert.Empty(_vm.Items);
        }
    }
}
