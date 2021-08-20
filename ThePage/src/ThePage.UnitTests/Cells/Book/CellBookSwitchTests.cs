using System.Collections.Generic;
using ThePage.Core.Cells;
using Xunit;
using static ThePage.Core.Enums;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookSwitchTests
    {
        [Theory, MemberData(nameof(CellBookSwitchScenarios))]
        public void CreateValidCellBookSwitch(CellBookSwitch cell, bool isValid)
        {
            //Check
            Assert.Equal(isValid, cell.IsValid);
        }

        public static IEnumerable<object[]> CellBookSwitchScenarios =>
            new[]
               {
                new object[] { DataFactory.GetTrueValueCellBookSwitch(), true },
                new object[] { DataFactory.GetFalseValueCellBookSwitch(), true },
                new object[] { DataFactory.GetEmptyValueCellBookSwitch(), true }
        };

        [Fact]
        public void ChangeCellBookSwitcFromFalseToTrue()
        {
            //Setup
            var cell = DataFactory.GetFalseValueCellBookSwitch();

            //Execute
            cell.IsSelected = true;

            //Check
            Assert.True(cell.IsSelected);
        }

        [Fact]
        public void ChangeCellBookSwitcFromTrueToFalse()
        {
            //Setup
            var cell = DataFactory.GetTrueValueCellBookSwitch();

            //Execute
            cell.IsSelected = false;

            //Check
            Assert.False(cell.IsSelected);
        }

        public static class DataFactory
        {
            public static CellBookSwitch GetTrueValueCellBookSwitch()
            {
                var title = string.Empty;
                var value = true;
                var type = EBookInputType.Read;
                var isEdit = false;

                return new CellBookSwitch(title, value, type, EmptyAction, isEdit);
            }

            public static CellBookSwitch GetFalseValueCellBookSwitch()
            {
                var title = string.Empty;
                var value = false;
                var type = EBookInputType.Read;
                var isEdit = false;

                return new CellBookSwitch(title, value, type, EmptyAction, isEdit);
            }

            public static CellBookSwitch GetEmptyValueCellBookSwitch()
            {
                var title = string.Empty;
                var type = EBookInputType.Read;
                var isEdit = false;

                return new CellBookSwitch(title, type, EmptyAction, isEdit);
            }

            static void EmptyAction() { }
        }
    }
}
