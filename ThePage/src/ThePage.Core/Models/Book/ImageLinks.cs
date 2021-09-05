namespace ThePage.Core
{
    public class ImageLinks
    {
        #region Properties

        public string SmallThumbnail { get; set; }

        public string Thumbnail { get; set; }

        public string Small { get; set; }

        public string Medium { get; set; }

        public string Large { get; set; }

        public string ExtraLarge { get; set; }

        #endregion

        #region Public

        public string GetImageUrl()
            => Thumbnail ?? Small ?? SmallThumbnail ?? Medium ?? Large ?? ExtraLarge ?? null;

        #endregion
    }
}
