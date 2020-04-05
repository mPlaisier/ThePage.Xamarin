using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests
{
    public partial class AuthorBusinessLogicTests
    {
        [Fact]
        public void CheckMappingCompleteListAuthorsToAuthorCells()
        {
            //Create List<Author>
            var authors = JsonConvert.DeserializeObject<List<Author>>(AuthorDataComplete);

            //Execute
            var authorCells = AuthorBusinessLogic.AuthorsToAuthorCells(authors);

            //Check
            Assert.NotNull(authorCells);
            Assert.NotEmpty(authorCells);
            Assert.Equal(authorCells.Count, authorCells.Count);
        }

        [Fact]
        public void CheckMappingEmptyListAuthorsToAuthorCells()
        {
            //Create List<Author>
            var authors = JsonConvert.DeserializeObject<List<Author>>(AuthorDataEmpty);

            //Execute
            var authorCells = AuthorBusinessLogic.AuthorsToAuthorCells(authors);

            //Check
            Assert.NotNull(authorCells);
            Assert.Empty(authorCells);
        }

        [Fact]
        public void CheckMappingNullListAuthorsToAuthorsCells()
        {
            //Create List<Author>
            List<Author> authors = null;

            //Execute
            var authorCells = AuthorBusinessLogic.AuthorsToAuthorCells(authors);

            //Check
            Assert.Null(authorCells);
        }
    }
}
