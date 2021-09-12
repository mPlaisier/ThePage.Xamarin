using CBP.Extensions;

namespace ThePage.Core
{
    public static class ImageBusinessLogic
    {
        #region Public

        public static ImageLinks MapImages(Api.ImageLinks images)
        {
            if (images.IsNull())
                return null;

            return new ImageLinks()
            {
                SmallThumbnail = images.SmallThumbnail,
                Thumbnail = images.Thumbnail,
                Small = images.Small,
                Medium = images.Medium,
                Large = images.Large,
                ExtraLarge = images.ExtraLarge
            };
        }

        public static Api.ImageLinks MapImages(ImageLinks images)
        {
            if (images.IsNull())
                return null;

            return new Api.ImageLinks()
            {
                SmallThumbnail = images.SmallThumbnail,
                Thumbnail = images.Thumbnail,
                Small = images.Small,
                Medium = images.Medium,
                Large = images.Large,
                ExtraLarge = images.ExtraLarge
            };
        }

        #endregion
    }
}
