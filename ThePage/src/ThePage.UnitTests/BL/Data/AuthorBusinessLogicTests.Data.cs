using System;
namespace ThePage.UnitTests.BusinessLogic
{
    public partial class AuthorBusinessLogicTests
    {
        public const string AuthorDataComplete =
            @"[{
                ""_id"":""5e80acc955926700174ef92c"",
                ""name"":""J.R.R. Tolkien"",
                ""__v"":0},
            {
                ""_id"":""5e80ace555926700174ef92d"",
                ""name"":""Brian Staveley"",
                ""__v"":0},
            {
                ""_id"":""5e80acfc55926700174ef92e"",
                ""name"":""Stephen Lloyd Jones"",
                ""__v"":0},
            {
                ""_id"":""5e80ad1355926700174ef92f"",
                ""name"":""Sara Raasch"",
                ""__v"":0},
            {
                ""_id"":""5e80ad2755926700174ef930"",
                ""name"":""George R.R. Martin"",
                ""__v"":0},
            {
                ""_id"":""5e878c7e1d0ad60017ca1f2a"",
                ""name"":""Mark Manson"",
                ""__v"":0
            }]";

        const string AuthorDataEmpty = @"[]";
    }
}
