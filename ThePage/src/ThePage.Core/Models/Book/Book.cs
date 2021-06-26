namespace ThePage.Core
{
    public class Book
    {
        #region Properties

        public string Id { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            return obj is Book item && Id.Equals(item.Id) && Title.Equals(item.Title);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
