using MonkeyCache.LiteDB;

namespace ThePage.Core
{
    public partial class ThePageService : IThePageService
    {
        readonly IUserInteraction _userInteraction;
        readonly IAuthService _authService;
        readonly IExceptionService _exceptionService;

        #region Constructor

        public ThePageService(IUserInteraction userInteraction, IAuthService authService, IExceptionService exceptionService)
        {
            _userInteraction = userInteraction;
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