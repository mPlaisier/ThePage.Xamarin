using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Author
{
    public class AddAuthorViewModelTests : BaseViewModelTests
    {
        AddAuthorViewModel _vm;

        #region Constructor

        public AddAuthorViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AddAuthorViewModel>();
        }

        #endregion

        [Theory]
        [InlineData("Valid name", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void AuthorValidationOK(string name, bool isValid)
        {
            //Setup
            _vm.TxtName = name;

            //Check
            Assert.Equal(isValid, _vm.IsValid);
        }

        [Fact]
        public void AuthorIsValidPropertyChangedTest()
        {
            //Execute
            Assert.PropertyChanged(_vm, nameof(_vm.IsValid),
                () => _vm.TxtName = "input");
        }
    }
}