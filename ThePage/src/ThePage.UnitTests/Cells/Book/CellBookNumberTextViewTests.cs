using System.Collections.Generic;
using FluentAssertions;
using ThePage.Core.Cells;
using Xunit;
using static ThePage.Core.Enums;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookNumberTextViewTests
    {
        [Theory, MemberData(nameof(CellBookNumberTextViewScenarios))]
        public void CreateCellBookNumberTextViewAndCheckIfValid(CellBookNumberTextView cell, bool isValid)
        {
            //Assert
            cell.IsValid.Should().Be(isValid);
        }

        [Fact]
        public void CellBookNumberTextViewValidAfterInput()
        {
            //Arrange
            var cell = DataFactory.GetNoValueCellBookNumberTextViewNoValueIsRequiredAndIsNotEdit();

            //Execute
            cell.TxtInput = DataFactory.ValidNumberString;

            //Assert
            cell.IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> CellBookNumberTextViewScenarios =>
            new[]
            {
                new object[] {DataFactory.GetValidInputCellBookNumberTextViewIsRequiredAndIsNotEdit(), true },
                new object[] { DataFactory.GetEmptyInputValueCellBookNumberTextViewIsRequiredAndIsNotEdit(), false },
                new object[] { DataFactory.GetNullInputValueCellBookNumberTextViewIsRequiredAndIsNotEdit(), false },
                new object[] { DataFactory.GetNoValueCellBookNumberTextViewNoValueIsRequiredAndIsNotEdit(), false },
                new object[] { DataFactory.GetNoValueCellBookNumberTextViewNoValueIsNotRequiredAndIsNotEdit() , true},
                new object[] { DataFactory.GetValidInputCellBookNumberTextViewIsNotRequiredAndIsNotEdit(), true },
                new object[] { DataFactory.GetCellBookNumberTextViewNoValueIsNotRequiredAndIsNotEdit(), true },
                new object[] { DataFactory.GetNullInputValueCellBookNumberTextViewIsNotRequiredAndIsNotEdit(), true },
                new object[] { DataFactory.GetStringInputCellBookNumberTextViewIsRequiredAndIsNotEdit(), false }
            };

        public static class DataFactory
        {
            public static string ValidNumberString => "1234";
            public static string InvalidString => "abc";

            public static CellBookNumberTextView GetValidInputCellBookNumberTextViewIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = ValidNumberString;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookNumberTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetEmptyInputValueCellBookNumberTextViewIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookNumberTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetNullInputValueCellBookNumberTextViewIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookNumberTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetNoValueCellBookNumberTextViewNoValueIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookNumberTextView(title, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetNoValueCellBookNumberTextViewNoValueIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookNumberTextView(title, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetValidInputCellBookNumberTextViewIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = ValidNumberString;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookNumberTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetCellBookNumberTextViewNoValueIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookNumberTextView(title, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetNullInputValueCellBookNumberTextViewIsNotRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = string.Empty;
                var type = EBookInputType.Title;
                var isRequired = false;
                var isEdit = false;

                return new CellBookNumberTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            public static CellBookNumberTextView GetStringInputCellBookNumberTextViewIsRequiredAndIsNotEdit()
            {
                var title = string.Empty;
                var value = InvalidString;
                var type = EBookInputType.Title;
                var isRequired = true;
                var isEdit = false;

                return new CellBookNumberTextView(title, value, type, EmptyAction, isRequired, isEdit);
            }

            static void EmptyAction() { }
        }
    }
}
