using System.Collections.Generic;
using ThePage.Core.Cells;
using Xunit;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookButtonTests
    {
        [Theory, MemberData(nameof(CellBookButtonScenarios))]
        public void CreateValidCellBookButton(CellBookButton cell, bool isValid)
        {
            //Check
            Assert.Equal(isValid, cell.IsValid);
        }

        public static IEnumerable<object[]> CellBookButtonScenarios =>
            new[]
               {
                new object[] { DataFactory.GetCellBookButtonIsRequiredAndValidNullAction(), true },
                new object[] { DataFactory.GetCellBookButtonIsNotRequiredAndValidNullAction(), true },
                new object[] { DataFactory.GetCellBookButtonIsRequiredAndNotValidNullAction(), false },
                new object[] { DataFactory.GetCellBookButtonIsNotRequiredAndNotValidNullAction(), true }
        };

        public static class DataFactory
        {
            public static CellBookButton GetCellBookButtonIsRequiredAndValidNullAction()
            {
                var title = string.Empty;
                var isRequired = true;

                var cell = new CellBookButton(title, null, isRequired)
                {
                    IsValid = true
                };
                return cell;
            }

            public static CellBookButton GetCellBookButtonIsNotRequiredAndValidNullAction()
            {
                var title = string.Empty;
                var isRequired = false;

                var cell = new CellBookButton(title, null, isRequired)
                {
                    IsValid = true
                };
                return cell;
            }

            public static CellBookButton GetCellBookButtonIsRequiredAndNotValidNullAction()
            {
                var title = string.Empty;
                var isRequired = true;

                return new CellBookButton(title, null, isRequired);
            }

            public static CellBookButton GetCellBookButtonIsNotRequiredAndNotValidNullAction()
            {
                var title = string.Empty;
                var isRequired = false;

                return new CellBookButton(title, null, isRequired);
            }
        }
    }
}