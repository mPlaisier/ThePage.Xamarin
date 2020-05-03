using System;
using Moq;
using MvvmCross.Base;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using MvvmCross.Views;
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

            //IMvxMainThreadAsyncDispatcher:
            //Required creating MvxObservableCollection in the VM Initialize
            var mockMainThreadAsyncDispatcher = new Mock<IMvxMainThreadAsyncDispatcher>();
            Ioc.RegisterSingleton(mockMainThreadAsyncDispatcher.Object);

            MockNavigation = new Mock<IMvxNavigationService>();
            Ioc.RegisterSingleton(MockNavigation.Object);

            MockThePageService = new Mock<IThePageService>();
            Ioc.RegisterSingleton(MockThePageService.Object);

            var mockUserInteraction = new Mock<IUserInteraction>();
            Ioc.RegisterSingleton(mockUserInteraction.Object);

            var mockDevice = new Mock<IDevice>();
            Ioc.RegisterSingleton(mockDevice.Object);

            var mockOpenLibrary = new Mock<IOpenLibraryService>();
            Ioc.RegisterSingleton(mockOpenLibrary.Object);
        }

        #endregion
    }
}