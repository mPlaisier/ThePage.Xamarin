using ThePage.Api;

namespace ThePage.Core
{
    [ThePageLazySingletonService]
    public partial class ThePageService : IThePageService
    {
        readonly IExceptionService _exceptionService;

        readonly IGenreWebService _genreWebService;
        readonly IAuthorWebService _authorWebService;
        readonly IBookWebService _bookWebService;
        readonly IBookShelfWebService _bookShelfWebService;

        #region Constructor

        public ThePageService(IExceptionService exceptionService,
                              IGenreWebService genreWebService,
                              IAuthorWebService authorWebService,
                              IBookWebService bookWebService,
                              IBookShelfWebService bookShelfWebService)
        {
            _exceptionService = exceptionService;

            _genreWebService = genreWebService;
            _authorWebService = authorWebService;
            _bookWebService = bookWebService;
            _bookShelfWebService = bookShelfWebService;
        }

        #endregion
    }
}