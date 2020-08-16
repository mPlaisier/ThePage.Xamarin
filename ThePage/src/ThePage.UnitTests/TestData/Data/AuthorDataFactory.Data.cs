namespace ThePage.UnitTests
{
    public static partial class AuthorDataFactory
    {
        const string SingleAuthor =
            @"{
                ""id"":""5f2872867e7cfd00174298eb"",
                ""name"":""Author2""
            }";

        const string ListAuthor4ElementsComplete =
           @"{
                ""docs"": [
                {
                  ""name"": ""Author1"",
                  ""id"": ""5f2841281bf6180017179b90""
                },
                {
                  ""name"": ""Author2"",
                  ""id"": ""5f2872867e7cfd00174298eb""
                },
                {
                  ""name"": ""Author3"",
                  ""olkey"": ""OL1234"",
                  ""id"": ""5eda16f40e0f1e6d7765e824""
                },
                {
                  ""name"": ""Author4"",
                  ""id"": ""5f25dfb383c1eb00173fa0ed""
                }
              ],
              ""totalDocs"": 4,
              ""limit"": 25,
              ""totalPages"": 1,
              ""page"": 1,
              ""pagingCounter"": 1,
              ""hasPrevPage"": false,
              ""hasNextPage"": false,
              ""prevPage"": null,
              ""nextPage"": null
            }";

        const string ListAuthorDataEmpty =
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