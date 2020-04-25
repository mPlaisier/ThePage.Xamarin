using System;
namespace ThePage.UnitTests
{
    public partial class GenreBusinessLogicTests
    {
        public const string GenreDataComplete =
            @"[{
                ""_id"":""5e7fc0bbbc363c52e7d115ce"",
                ""name"":""Fiction"",
                ""__v"":0},
            {
                ""_id"":""5e7fc0c9bc363c52e7d115cf"",
                ""name"":""Non-Fiction"",
                ""__v"":0},
            {
                ""_id"":""5e80755dd032d70017508517"",
                ""name"":""Epic-Fantasy"",
                ""__v"":0},

            {
                ""_id"":""5e878ae11d0ad60017ca1f26"",
                ""name"":""Fantasy"",
                ""__v"":0}
            ]";

        const string GenreDataEmpty = @"[]";
    }
}