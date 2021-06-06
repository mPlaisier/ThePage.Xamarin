using CBP.Extensions;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellGoogleBook
    {
        readonly GoogleBook _book;

        #region Properties

        public string Title => _book?.VolumeInfo.Title;

        public string Author => GetAuthors();

        public string ImageUri => GetImage();

        #endregion

        #region Constructor

        public CellGoogleBook(GoogleBook book)
        {
            _book = book;
        }

        #endregion

        #region Private

        string GetAuthors()
        {
            if (_book != null && _book.VolumeInfo.Authors.IsNotNullAndHasItems())
            {
                var authors = "";
                _book.VolumeInfo.Authors.ForEach((a) => authors += $", {a}");
                authors.Substring(1);

                return authors;
            }
            return "";
        }

        string GetImage()
        {
            if (_book != null && _book.VolumeInfo.ImageLinks != null)
            {
                var images = _book.VolumeInfo.ImageLinks;
                if (images.Thumbnail != null)
                    return images.Thumbnail;

                if (images.Small != null)
                    return images.Small;

                if (images.SmallThumbnail != null)
                    return images.SmallThumbnail;

                if (images.Medium != null)
                    return images.Medium;

                if (images.Large != null)
                    return images.Large;

                if (images.ExtraLarge != null)
                    return images.ExtraLarge;
            }

            return null;
        }

        #endregion
    }
}
