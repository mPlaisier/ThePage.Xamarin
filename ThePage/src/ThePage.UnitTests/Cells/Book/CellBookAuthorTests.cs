using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using MvvmCross.Navigation;
using ThePage.Core;
using ThePage.Core.Cells;
using Xunit;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookAuthorTests
    {
        [Theory, MemberData(nameof(CellBookAuthorScenarios))]
        public void CreateCellBookAuthorAndCheckValidProperty(CellBookAuthor cell, bool isValid)
        {
            //Check
            cell.IsValid.Should().Be(isValid);
        }

        [Fact]
        public void CheckCellBookAuthorIsValidAfterUpdateAuthor()
        {
            //Setup
            var cell = DataFactory.GetCellBookAuthorIsNotValid();

            //Execute
            cell.Item = AuthorDataFactory.GetSingleAuthor();

            //Execute
            cell.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CheckCellBookAuthorIsNotValidAfterUpdateAuthor()
        {
            //Setup
            var cell = DataFactory.GetCellBookAuthorIsNotValid();

            //Execute
            cell.Item = null;

            //Execute
            cell.IsValid.Should().BeFalse();
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
                var cell = new CellBookAuthor(author, It.IsAny<MvxNavigationService>(), It.IsAny<IDevice>(), () => { });

                return cell;
            }

            public static CellBookAuthor GetCellBookAuthorIsNotValid()
            {
                Core.Author author = null;
                var cell = new CellBookAuthor(author, It.IsAny<MvxNavigationService>(), It.IsAny<IDevice>(), () => { });

                return cell;
            }
        }
    }
}