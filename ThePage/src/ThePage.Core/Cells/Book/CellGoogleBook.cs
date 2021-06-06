using CBP.Extensions;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellGoogleBook
    {


        #region Properties

        public GoogleBook Book { get; }

        public string Title => Book?.VolumeInfo.Title;

        public string Author => GetAuthors();

        public string ImageUri => GetImage();

        #endregion

        #region Constructor

        public CellGoogleBook(GoogleBook book)
        {
            Book = book;
        }

        #endregion

        #region Private

        string GetAuthors()
        {
            if (Book != null && Book.VolumeInfo.Authors.IsNotNullAndHasItems())
            {
                var authors = "";
                Book.VolumeInfo.Authors.ForEach((a) => authors += $", {a}");
                authors.Substring(1);

                return authors;
            }
            return "";
        }

        string GetImage()
        {
            if (Book != null && Book.VolumeInfo.ImageLinks != null)
            {
                var images = Book.VolumeInfo.ImageLinks;
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
