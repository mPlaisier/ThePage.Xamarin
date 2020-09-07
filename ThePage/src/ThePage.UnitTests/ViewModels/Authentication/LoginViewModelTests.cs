using System;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Authentication
{
    public class LoginViewModelTests : BaseViewModelTests
    {
        LoginViewModel _vm;

        #region Constructor

        public LoginViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<LoginViewModel>();
        }

        #endregion

        [Fact]
        public void BtnShouldBeDisabledAtStart()
        {
            //Assert
            Assert.False(_vm.BtnLoginEnabled);
        }

        [Theory]
        [InlineData("Valid username", "valid password", true)]
        [InlineData("Valid username", "", false)]
        [InlineData("", "valid password", false)]
        public void BtnShouldBeEnabledInCorrectInput(string username, string password, bool isEnabled)
        {
            //Arrange
            _vm.Username = username;
            _vm.Password = password;

            //Execute

            //Check
            Assert.Equal(isEnabled, _vm.BtnLoginEnabled);
        }

    }
}
