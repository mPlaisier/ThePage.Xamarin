using System.Collections.Generic;
using ThePage.Core.Cells;
using Xunit;
using static ThePage.Core.Enums;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookTextViewTests
    {
        [Theory, MemberData(nameof(CellBookTextViewScenarios))]
        public void CreateCellBookTextViewAndCheckIfValid(CellBookTextView cell, bool isValid)
        {
            //Assert
            Assert.Equal(isValid, cell.IsValid);
        }

        [Fact]
        public void CellBookTextViewValidAfterInput()
        {
            //Arrange
            var cell = DataFactory.GetNoValueCellBookTextViewNoValueIsRequiredAndIsNotEdit();

            //Execute
            cell.TxtInput = DataFactory.ValidString;

            //Assert
            Assert.True(cell.IsValid);
        }

        public static IEnumerable<object[]> CellBookTextViewScenarios =>
               new[]
               {
                new object[] {DataFactory.GetValidInputCellBookTextViewIsRequiredAndIsNotEdit(), true },
                new object[] { DataFactory.GetEmptyInputValueCellBookTextViewIsRequiredAndIsNotEdit(), false },
                new object[] { DataFactory.GetNullInputValueCellBookTextViewIsRequiredAndIsNotEdit(), false },
                new object[] { DataFactory.GetNoValueCellBookTextViewNoValueIsRequiredAndIsNotEdit(), false },
                new object[] { DataFactory.GetNoValueCellBookTextViewNoValueIsNotRequiredAndIsNotEdit() , true},
                new object[] { DataFactory.GetValidInputCellBookTextViewIsNotRequiredAndIsNotEdit(), true },
                new object[] { DataFactory.GetCellBookTextViewNoValueIsNotRequiredAndIsNotEdit(), true },
                new object[] { DataFactory.GetNullInputValueCellBookTextViewIsNotRequiredAndIsNotEdit(), true },
        };

        public static class DataFactory
        {
            public static string ValidString => "Valid String";

            public static CellBookTextView GetValidInputCellBookTextViewIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = ValidString;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookTextView GetEmptyInputValueCellBookTextViewIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookTextView GetNullInputValueCellBookTextViewIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookTextView GetNoValueCellBookTextViewNoValueIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookTextView(title, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookTextView GetNoValueCellBookTextViewNoValueIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookTextView(title, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookTextView GetValidInputCellBookTextViewIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = ValidString;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookTextView GetCellBookTextViewNoValueIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookTextView(title, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookTextView GetNullInputValueCellBookTextViewIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            static void EmptyAction() { }
        }
    }
}