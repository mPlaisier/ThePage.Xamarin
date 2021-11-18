using Moq;
using MvvmCross.Base;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using ThePage.Api;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public class BaseServicesTests : MvxIoCSupportingTest
    {
        #region API Properties

        protected Mock<ILocalDatabaseService> MockLocalDatabaseService { get; private set; }
        protected Mock<ITimeKeeper> MockTimeKeeper { get; private set; }

        #endregion

        #region Properties

        protected Mock<IMvxNavigationService> MockNavigation { get; private set; }
        protected Mock<IThePageService> MockThePageService { get; private set; }
        protected Mock<IAuthService> MockAuthService { get; private set; }
        protected Mock<IUserInteraction> MockUserInteraction { get; private set; }

        protected Mock<IBookService> MockBookService { get; private set; }
        protected Mock<IAuthorService> MockAuthorService { get; private set; }
        protected Mock<IGenreService> MockGenreService { get; private set; }
        protected Mock<IBookShelfService> MockBookShelfService { get; private set; }

        #endregion

        #region Setup

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            //IMvxMainThreadAsyncDispatcher:
            //Required creating MvxObservableCollection in the VM Initialize
            var mockMainThreadAsyncDispatcher = new Mock<IMvxMainThreadAsyncDispatcher>();
            Ioc.RegisterSingleton(mockMainThreadAsyncDispatcher.Object);

            //API
            MockLocalDatabaseService = new Mock<ILocalDatabaseService>();
            Ioc.RegisterSingleton(MockLocalDatabaseService.Object);

            MockTimeKeeper = new Mock<ITimeKeeper>();
            Ioc.RegisterSingleton(MockTimeKeeper.Object);

            //CORE
            MockNavigation = new Mock<IMvxNavigationService>();
            Ioc.RegisterSingleton(MockNavigation.Object);

            MockThePageService = new Mock<IThePageService>();
            Ioc.RegisterSingleton(MockThePageService.Object);

            MockBookService = new Mock<IBookService>();
            Ioc.RegisterSingleton(MockBookService.Object);

            MockAuthorService = new Mock<IAuthorService>();
            Ioc.RegisterSingleton(MockAuthorService.Object);

            MockGenreService = new Mock<IGenreService>();
            Ioc.RegisterSingleton(MockGenreService.Object);

            MockBookShelfService = new Mock<IBookShelfService>();
            Ioc.RegisterSingleton(MockBookShelfService.Object);

            MockUserInteraction = new Mock<IUserInteraction>();
            Ioc.RegisterSingleton(MockUserInteraction.Object);

            var mockDevice = new Mock<IDevice>();
            Ioc.RegisterSingleton(mockDevice.Object);

            MockAuthService = new Mock<IAuthService>();
            Ioc.RegisterSingleton(MockAuthService.Object);

            var mockOpenLibrary = new Mock<IOpenLibraryService>();
            Ioc.RegisterSingleton(mockOpenLibrary.Object);

            var mockGoogleService = new Mock<IGoogleBooksService>();
            Ioc.RegisterSingleton(mockGoogleService.Object);
        }

        #endregion
    }
}