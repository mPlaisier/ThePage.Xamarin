namespace ThePage.Core
{
    public class Author
    {
        #region Properties

        public string Id { get; set; }

        public string Name { get; set; }

        public string Olkey { get; set; }

        #endregion

        #region Constructor

        public Author()
        {
        }

        public Author(string name)
        {
            Name = name;
        }

        #endregion
    }
}