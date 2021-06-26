using System.Collections.Generic;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookAuthorTests
    {
        [Theory, MemberData(nameof(CellBookAuthorScenarios))]
        public void CreateCellBookAuthorAndCheckValidProperty(CellBookAuthor cell, bool isValid)
        {
            //Check
            Assert.Equal(isValid, cell.IsValid);
        }

        [Fact]
        public void CheckCellBookAuthorIsValidAfterUpdateAuthor()
        {
            //Setup
            var cell = DataFactory.GetCellBookAuthorIsNotValid();

            //Execute
            cell.Item = AuthorDataFactory.GetSingleAuthor();

            //Execute
            Assert.True(cell.IsValid);
        }

        [Fact]
        public void CheckCellBookAuthorIsNotValidAfterUpdateAuthor()
        {
            //Setup
            var cell = DataFactory.GetCellBookAuthorIsNotValid();

            //Execute
            cell.Item = null;

            //Execute
            Assert.False(cell.IsValid);
        }

        public static IEnumerable<object[]> CellBookAuthorScenarios =>
            new[]
               {
                new object[] { DataFactory.GetCellBookAuthorIsValid(), true },
                new object[] { DataFactory.GetCellBookAuthorIsNotValid(), false }
            };

        public static class DataFactory
        {
            public static CellBookAuthor GetCellBookAuthorIsValid()
            {
                var author = AuthorDataFactory.GetSingleAuthor();
                var cell = new CellBookAuthor(author, null, null, null);

                return cell;
            }

            public static CellBookAuthor GetCellBookAuthorIsNotValid()
            {
                Core.Author author = null;
                var cell = new CellBookAuthor(author, null, null, null);

                return cell;
            }
        }
    }
}