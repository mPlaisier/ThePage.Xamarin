using CBP.Extensions;
using ThePage.Api;

namespace ThePage.Core.Cells
{
    public class CellGoogleBook
    {
        #region Properties

        public GoogleBook Book { get; }

        public string Title => Book?.VolumeInfo.Title;

        public string Author => GetAuthors();

        public string ImageUri => Book.GetImageUrl();

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
                authors = authors.Remove(0, 2);

                return authors;
            }
            return "";
        }

        #endregion
    }
}