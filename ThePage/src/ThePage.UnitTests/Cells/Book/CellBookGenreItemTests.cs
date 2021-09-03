using System.Collections.Generic;
using ThePage.Core.Cells;
using Xunit;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookGenreItemTests
    {
        [Theory, MemberData(nameof(CellBookGenreItemScenarios))]
        public void CreateValidCellBookGenreItem(CellBookGenreItem cell, bool isValid)
        {
            //Check
            Assert.Equal(isValid, cell.IsValid);
        }

        [Fact]
        public void NoCrashForNullAction()
        {
            //Setup
            var cell = DataFactory.GetGenreValueCellBookGenreItemNullAction();

            //Execute
            cell.DeleteCommand.Execute();
        }

        public static IEnumerable<object[]> CellBookGenreItemScenarios =>
            new[]
               {
                new object[] { DataFactory.GetGenreValueCellBookGenreItemNullAction(), true },
                new object[] { DataFactory.GetNullValueCellBookGenreItem(), true }
        };

        public static class DataFactory
        {
            public static CellBookGenreItem GetGenreValueCellBookGenreItemNullAction()
            {
                var genre = GenreDataFactory.GetSingleGenre();
                var isEdit = false;

                return new CellBookGenreItem(genre, null, isEdit);
            }

            public static CellBookGenreItem GetNullValueCellBookGenreItem()
            {
                Core.Genre genre = null;
                var isEdit = false;

                return new CellBookGenreItem(genre, null, isEdit);
            }
        }
    }
}