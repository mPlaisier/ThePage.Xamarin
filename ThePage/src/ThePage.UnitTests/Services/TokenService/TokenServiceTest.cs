using System;
using FluentAssertions;
using ThePage.Api;
using Xunit;

namespace ThePage.UnitTests.Services.Tokens
{
    public class TokenServiceTest : BaseServicesTests
    {
        readonly TokenService _tokenService;

        #region Constructor

        public TokenServiceTest()
        {
            Setup();

            _tokenService = Ioc.IoCConstruct<TokenService>();
        }

        #endregion

        #region GetAccessToken

        [Fact]
        public void AccessTokenShouldBeNullIfLocalDatabaseIsNull()
        {
            //Arrange
            SetupLocalDatabase(null);

            //Execute
            var tokens = _tokenService.GetAccessToken();

            //Assert
            tokens.Should().BeNull();
        }

        [Fact]
        public void AccessTokenResultShouldNotBeRefreshIfValid()
        {
            //Arrange
            SetupLocalDatabase(TokensTestData.GetValidTokens());
            SetupTimeKeeper();

            //Execute
            var tokens = _tokenService.GetAccessToken();

            tokens.Should().NotBeNull();
            tokens.ShouldRefresh.Should().BeFalse();
        }

        [Fact]
        public void AccessTokenResultShouldBeRefreshIfInValid()
        {
            //Arrange
            SetupLocalDatabase(TokensTestData.GetInValidTokens());
            SetupTimeKeeper();

            //Execute
            var tokens = _tokenService.GetAccessToken();

            //Assert
            tokens.Should().NotBeNull();
            tokens.ShouldRefresh.Should().BeTrue();
        }

        #endregion

        #region ShouldRefreshToken

        [Fact]
        public void ShouldRefreshTokenShouldBeTrueIfNull()
        {
            //Arrange
            SetupLocalDatabase(null);

            //Execute
            var result = _tokenService.ShouldRefreshToken();

            //Assert
            result.Should().BeTrue("tokens are null");
        }

        [Fact]
        public void ShouldRefreshTokenShouldBeTrueIfExpired()
        {
            //Arrange
            SetupLocalDatabase(TokensTestData.GetInValidTokens());
            SetupTimeKeeper();

            //Execute
            var result = _tokenService.ShouldRefreshToken();

            //Assert
            result.Should().BeTrue("tokens are expired");
        }

        [Fact]
        public void ShouldRefreshTokenShouldBeFalseIfValid()
        {
            //Arrange
            SetupLocalDatabase(TokensTestData.GetValidTokens());
            SetupTimeKeeper();

            //Execute
            var result = _tokenService.ShouldRefreshToken();

            //Assert
            result.Should().BeFalse("tokens are valid");
        }

        #endregion

        #region GetRefreshToken

        [Fact]
        public void GetRefreshTokenShouldBeNullIfLocalDatabaseIsNull()
        {
            //Arrange
            SetupLocalDatabase(null);

            //Execute
            var result = _tokenService.GetRefreshToken();

            //Assert
            result.Should().BeNull("tokens are null");
        }

        [Fact]
        public void GetRefreshTokenShouldReturnNullIfExpired()
        {
            //Arrange
            SetupLocalDatabase(TokensTestData.GetInValidTokens());
            SetupTimeKeeper();

            //Execute
            var result = _tokenService.GetRefreshToken();

            //Assert
            result.Should().BeNull("tokens are expired");
        }

        [Fact]
        public void GetRefreshTokenShouldReturnValueIfValid()
        {
            //Arrange
            SetupLocalDatabase(TokensTestData.GetValidTokens());
            SetupTimeKeeper();

            //Execute
            var result = _tokenService.GetRefreshToken();

            //Assert
            result.Should().NotBeNull("tokens are valid");
            result.Should().NotBeEmpty();
        }

        #endregion

        #region Private

        void SetupLocalDatabase(ApiTokens result)
        {
            MockLocalDatabaseService
               .Setup(x => x.GetData<ApiTokens>(EApiDataType.Tokens, null, null))
               .Returns(() => result);
        }

        void SetupTimeKeeper()
        {
            MockTimeKeeper
                .Setup(x => x.Now)
                .Returns(() => DateTime.UtcNow);
        }

        #endregion

    }
}
