namespace ThePage.Core
{
    public class Genre
    {
        #region Properties

        public string Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            return obj is Genre item && Id.Equals(item.Id) && Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
