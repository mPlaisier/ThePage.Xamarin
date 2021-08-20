namespace ThePage.UnitTests
{
    public static partial class ApiBookDataFactory
    {
        const string SingleBookWithGenres =
            @"{
                ""genres"": [
                    ""5eaeaf3940d0030017be787d"",
                    ""5eaeaf3940d0030017be7879""
                ],
                ""_id"": ""5eaeaf3c40d0030017be788c"",
                ""title"": ""Book 1"",
                ""author"": ""5eaeaf3c40d0030017be788b"",
                ""isbn"": ""123456789"",
                ""owned"": false,
                ""read"": true,
                ""pages"": 279
            }";

        const string SingleApiBook =
            @"{
                ""title"": ""Title1"",
                  ""author"": {
                    ""name"": ""Author1"",
                    ""id"": ""5ebe620b77a70d9bb9fb5c20""
                  },
                  ""id"": ""5eda2fc8c3939e752b81d76b""
            }";

        const string ApiBookDetailResponseWithGenres =
        @"{
            ""genres"": [
            {
                ""name"": ""Genre1"",
                ""id"": ""5f2b035b162feb001791363a""
            },
            {
                ""name"": ""Genre2"",
                ""id"": ""5f2b0362162feb001791363b""
            }],
            ""title"": ""New"",
            ""author"": {
                ""name"": ""Mattias2"",
                ""olkey"": ""123456"",
                ""id"": ""5eda16f40e0f1e6d7765e824""
            },
            ""owned"": false,
            ""read"": false,
            ""pages"": 55,
            ""id"": ""5f2b04c8162feb001791363e""
        }";

        const string ApiBookDetailResponseNoGenres =
        @"{
            ""genres"": [],
            ""title"": ""New"",
            ""author"": {
                ""name"": ""Mattias2"",
                ""olkey"": ""123456"",
                ""id"": ""5eda16f40e0f1e6d7765e824""
            },
            ""owned"": false,
            ""read"": false,
            ""pages"": 55,
            ""id"": ""5f2b04c8162feb001791363e""
        }";

        const string ListBook4ElementsComplete =
           @"{
                ""docs"": [
                {
                  ""title"": ""Title1"",
                  ""author"": {
                    ""name"": ""Author1"",
                    ""id"": ""5ebe620b77a70d9bb9fb5c20""
                  },
                  ""id"": ""5eda2fc8c3939e752b81d76b""
                },
                {
                  ""title"": ""Title2"",
                  ""author"": {
                    ""name"": ""Author1"",
                    ""id"": ""5ebe620b77a70d9bb9fb5c20""
                  },
                  ""id"": ""5eda2b3a695c3574ce9c02bc""
                },
                {
                  ""title"": ""Title3"",
                  ""author"": {
                    ""name"": ""Author1"",
                    ""id"": ""5ebe620b77a70d9bb9fb5c20""
                  },
                  ""id"": ""5eda29b141a1ab74802ba736""
                },
                {
                  ""title"": ""Title4"",
                  ""author"": {
                    ""name"": ""Author2"",
                    ""id"": ""5eda16f40e0f1e6d7765e824""
                  },
                  ""id"": ""5f2876e57e7cfd00174298f0""
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

        const string ListBook1ElementsComplete =
            @"{
                ""docs"": [
                {
                  ""title"": ""Title1"",
                  ""author"": {
                    ""name"": ""Author1"",
                    ""id"": ""5ebe620b77a70d9bb9fb5c20""
                  },
                  ""id"": ""5eda2fc8c3939e752b81d76b""
                }
              ],
              ""totalDocs"": 1,
              ""limit"": 25,
              ""totalPages"": 1,
              ""page"": 1,
              ""pagingCounter"": 1,
              ""hasPrevPage"": false,
              ""hasNextPage"": false,
              ""prevPage"": null,
              ""nextPage"": null
            }";

        const string ListBookDataEmpty =
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
