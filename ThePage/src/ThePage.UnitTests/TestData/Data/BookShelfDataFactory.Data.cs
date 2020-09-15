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
    }
}
