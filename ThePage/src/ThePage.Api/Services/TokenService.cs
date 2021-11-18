namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class TokenService : ITokenService
    {
        readonly ILocalDatabaseService _localDatabase;
        readonly ITimeKeeper _timeKeeper;

        #region Constructor

        public TokenService(ILocalDatabaseService localDatabase, ITimeKeeper timeKeeper)
        {
            _localDatabase = localDatabase;
            _timeKeeper = timeKeeper;
        }

        #endregion

        #region Public

        public TokenResult GetAccessToken()
        {
            var tokens = _localDatabase.GetData<ApiTokens>(EApiDataType.Tokens);

            if (tokens != null)
            {
                return tokens.Access.Expires > _timeKeeper.Now
                    ? new TokenResult(tokens, false)
                    : new TokenResult(tokens, true);
            }
            return default;
        }

        public void SetSessionToken(ApiTokens tokens)
        {
            _localDatabase.StoreData(tokens, EApiDataType.Tokens);
        }

        public bool ShouldRefreshToken()
        {
            var tokens = _localDatabase.GetData<ApiTokens>(EApiDataType.Tokens);

            return tokens == null || tokens.Access.Expires < _timeKeeper.Now;
        }

        public string GetRefreshToken()
        {
            var tokens = _localDatabase.GetData<ApiTokens>(EApiDataType.Tokens);

            string refreshToken = null;
            if (tokens != null && tokens.Refresh.Expires > _timeKeeper.Now)
            {
                refreshToken = tokens.Refresh.Token;
            }
            return refreshToken;
        }

        #endregion
    }

    public class TokenResult
    {
        #region Properties

        public ApiTokens Tokens { get; }

        public bool ShouldRefresh { get; }

        #endregion

        #region Constructor

        public TokenResult(ApiTokens tokens, bool shouldRefresh)
        {
            Tokens = tokens;
            ShouldRefresh = shouldRefresh;
        }

        #endregion
    }
}
