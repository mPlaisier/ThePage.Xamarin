using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiError
    {
        #region Properties

        [JsonProperty("message")]
        public string Message { get; internal set; }

        [JsonProperty("code")]
        public EApiErrorCode Code { get; internal set; }

        #endregion

        #region Constructor

        public ApiError()
        {

        }

        #endregion

    }

    //TODO perhaps move to a ENUM class/file
    public enum EApiErrorCode
    {
        Unkown = 0,
        AuthorNotFound = 11,
        BookNotFound = 21
    }

}
