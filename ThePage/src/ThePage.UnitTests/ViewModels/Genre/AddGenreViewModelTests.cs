using System.Threading.Tasks;
using Moq;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels
{
    public class AddGenreViewModelTests : BaseServicesTests
    {
        readonly AddGenreViewModel _vm;

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
    }
}