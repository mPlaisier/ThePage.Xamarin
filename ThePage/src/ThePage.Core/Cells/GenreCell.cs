namespace ThePage.Core
{
    public class CellGenre
    {
        #region Properties

        public string Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region Constructor

        public CellGenre()
        {

        }

        public CellGenre(string id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion
    }
}
