using System;
namespace ThePage.UnitTests
{
    public static partial class AuthorDataFactory
    {
        const string SingleAuthor =
            @"{
                ""_id"":""5e7fc0bbbc363c52e7d115ce"",
                ""name"":""William Shakespeare""
            }";

        const string ListAuthor4ElementsComplete =
           @"[{
                ""_id"":""5e7fc0bbbc363c52e7d115ce"",
                ""name"":""J.R.R. Tolkin""},
            {
                ""_id"":""5e7fc0c9bc363c52e7d115cf"",
                ""name"":""Ozzy""},
            {
                ""_id"":""5e80755dd032d70017508517"",
                ""name"":""William Shakespeare""},
            {
                ""_id"":""5e878ae11d0ad60017ca1f26"",
                ""name"":""Mattias Plaisier""}
            ]";

        const string ListAuthorDataEmpty = @"[]";
    }
}
