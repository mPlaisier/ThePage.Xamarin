namespace ThePage.UnitTests
{
    public static partial class GenreDataFactory
    {
        const string SingleGenre =
            @"{
                ""id"":""5f2841021bf6180017179b8d"",
                ""name"":""Fiction""
            }";

        const string ListGenre4ElementsComplete =
           @"{
                ""docs"": [
                    {
                        ""name"": ""Fiction"",
                        ""id"": ""5f2841021bf6180017179b8d""
                    },
                    {
                        ""name"": ""Non-Fiction"",
                        ""id"": ""5f2872947e7cfd00174298ec""
                    },
                    {
                        ""name"": ""Epic-Fantasy"",
                        ""id"": ""5f25b4b4537e9f0017b5948a""
                    },
                    {
                        ""name"": ""Fantasy"",
                        ""id"": ""5f268ed046219d001762142e""
                    }
                ],
                ""totalDocs"": 6,
                ""limit"": 25,
                ""totalPages"": 1,
                ""page"": 1,
                ""pagingCounter"": 1,
                ""hasPrevPage"": false,
                ""hasNextPage"": false,
                ""prevPage"": null,
                ""nextPage"": null
            }";

        const string ListGenreDataEmpty =
            @"{
                ""docs"": [],
                ""totalDocs"": 0,
                ""limit"": 25,
                ""totalPages"": 1,
                ""page"": 1,
                ""pagingCounter"": 1,
                ""hasPrevPage"": false,
                ""hasNextPage"": false,
                ""prevPage"": null,
                ""nextPage"": null
            }";
    }
}