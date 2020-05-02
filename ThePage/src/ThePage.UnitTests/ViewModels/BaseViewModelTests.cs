using System;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public class BaseViewModelTests : MvxIoCSupportingTest
    {
        #region Properties

        protected Mock<IMvxNavigationService> MockNavigation { get; private set; }
        protected Mock<IThePageService> MockThePageService { get; private set; }

        #endregion

        #region Setup

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            MockNavigation = new Mock<IMvxNavigationService>();
            Ioc.RegisterSingleton(MockNavigation.Object);

            MockThePageService = new Mock<IThePageService>();
            Ioc.RegisterSingleton(MockThePageService.Object);

            var mockUserInteraction = new Mock<IUserInteraction>();
            Ioc.RegisterSingleton(mockUserInteraction.Object);

            var mockDevice = new Mock<IDevice>();
            Ioc.RegisterSingleton(mockDevice.Object);
        }

        #endregion
    }
}