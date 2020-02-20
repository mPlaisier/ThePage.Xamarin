using System;
namespace ThePage.Core
{
    public class AuthorCell : BaseCell
    {
        #region Properties

        public string Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region Constructor

        public AuthorCell()
        {

        }

        public AuthorCell(string id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion
    }
}
