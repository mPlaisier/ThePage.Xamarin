using MonkeyCache.LiteDB;

namespace ThePage.Core
{
    [ThePageLazySingletonService]
    public partial class ThePageService : IThePageService
    {
        readonly IAuthService _authService;
        readonly IExceptionService _exceptionService;

        #region Constructor

        public ThePageService(IAuthService authService, IExceptionService exceptionService)
        {
            _authService = authService;
            _exceptionService = exceptionService;

            Barrel.ApplicationId = "thepageapplication";
            Barrel.EncryptionKey = "encryptionKey";

            //LiteDB Upgrade (4 -> 5) | NuGet 1.3 -> 1.5
            Barrel.Upgrade = true;
        }

        #endregion
    }
}