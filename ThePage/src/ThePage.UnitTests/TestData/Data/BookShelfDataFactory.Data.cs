using System;
namespace ThePage.UnitTests
{
    public static partial class BookShelfDataFactory
    {
        const string ListBookShelfEmpty =
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

        const string ListBookShelf2Elements =
            @"{
                ""docs"": [
                    {
                      ""books"": [],
                      ""name"": ""Empty shelf"",
                      ""id"": ""5eda855b7990b506f8719f23""
                    },
                    {
                      ""books"": [
                        ""5eda2ef1f9b08975083a1d1f"",
                        ""5eda2fc8c3939e752b81d76b""
                      ],
                      ""name"": ""New shelfs"",
                      ""id"": ""5eda84e87990b506f8719f22""
                    }
                ],
                ""totalDocs"": 2,
                ""limit"": 25,
                ""totalPages"": 1,
                ""page"": 1,
                ""pagingCounter"": 1,
                ""hasPrevPage"": false,
                ""hasNextPage"": false,
                ""prevPage"": null,
                ""nextPage"": null
            }";

        const string BookShelfDetailResponseWithBooks =
            @"{
                ""books"": [
                {
                    ""title"": ""title 1"",
                    ""author"": {
                    ""name"": ""name 1"",
                    ""id"": ""5f369567ab0b90001703d5df""
                },
                    ""id"": ""5f7065b62e98b70017ac3ed1""
                },
                {
                    ""title"": ""titel 2"",
                    ""author"": {
                        ""name"": ""author 1"",
                        ""id"": ""5eda16f40e0f1e6d7765e824""
                    },
                    ""id"": ""5f56950327b2c30017cbd2a5""
                },
                {
                    ""title"": ""title 3"",
                    ""author"": {
                        ""name"": ""author 2"",
                        ""id"": ""5ebe620b77a70d9bb9fb5c20""
                    },
                    ""id"": ""5eda29b141a1ab74802ba736""
                }
                ],
                ""name"": ""Bookshelf name"",
                ""id"": ""5f7066162e98b70017ac3ed3""
            }";

        const string BookShelfDetailResponseWithoutBooks =
            @"{
                ""books"": [],
                ""name"": ""Bookshelf name"",
                ""id"": ""5f7066162e98b70017ac3ed3""
            }";

        const string SingleBookfShelfWithBooks =
            @"{
                ""books"": [
                    ""5eda29b141a1ab74802ba736"",
                    ""5f56950327b2c30017cbd2a5"",
                    ""5f7065b62e98b70017ac3ed1""
                ],
                ""name"": ""Bookshelf name"",
                ""id"": ""5f7066162e98b70017ac3ed3""
            }";

        const string SingleBookfShelfWithoutBooks =
             @"{
                ""books"": [],
                ""name"": ""Bookshelf name"",
                ""id"": ""5f7066162e98b70017ac3ed3""
            }";
    }
}