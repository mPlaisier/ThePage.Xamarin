using System;
namespace ThePage.UnitTests
{
    public static partial class BookDataFactory
    {
        const string SingleBook =
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

        const string ListBook4ElementsComplete =
           @"[
           {
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
           },
           {
                ""genres"": [],
                ""_id"": ""5eaeaf3c40d0030017be788d"",
                ""title"": ""Book 2"",
                ""author"": ""5eaeaf3b40d0030017be7885"",
                ""isbn"": ""123456789"",
                ""owned"": false,
                ""read"": true,
                ""pages"": 252
           },
           {
                ""genres"": [
                    ""5eaeaf3a40d0030017be7881"",
                    ""5eaeaf3940d0030017be787d"",
                    ""5eaeaf3940d0030017be787b""
                ],
                ""_id"": ""5eaeaf3c40d0030017be788e"",
                ""title"": ""Book 3"",
                ""author"": ""5eaeaf3b40d0030017be7888"",
                ""isbn"": ""123456789"",
                ""owned"": false,
                ""read"": false,
                ""pages"": 313
           },
           {
                ""genres"": [],
                ""_id"": ""5eaeaf3c40d0030017be788f"",
                ""title"": ""Book 4"",
                ""author"": ""5eaeaf3b40d0030017be7889"",
                ""isbn"": ""123456789"",
                ""owned"": true,
                ""read"": true,
                ""pages"": 138
           }]";

        const string ListBookDataEmpty = @"[]";

        const string SingleOLObject =
            @"{
                  ""publishers"":[
                     {
                        ""name"":""UITGEVERIJ""
                     }
                  ],
                  ""links"":[
                     {
                        ""url"":""http://www.tolkienestate.com/en/writing/tales-of-middle-earth/the-lord-of-the-rings.html"",
                        ""title"":""Tolkien Estate | The Lord of the Rings""
                     },
                     {
                        ""url"":""https://en.wikipedia.org/wiki/The_Fellowship_of_the_Ring"",
                        ""title"":""The Fellowship of the Ring - Wikipedia""
                     },
                     {
                        ""url"":""https://archive.nytimes.com/www.nytimes.com/books/01/02/11/specials/tolkien-fellowship.html"",
                        ""title"":""The Hero is a Hobbit (New York Times Books)""
                     }
                  ],
                  ""title"":""De Reisgenoten"",
                  ""url"":""http://openlibrary.org/books/OL27047721M/De_Reisgenoten"",
                  ""identifiers"":{
                     ""isbn_13"":[
                        ""9789022531938""
                     ],
                     ""openlibrary"":[
                        ""OL27047721M""
                     ],
                     ""isbn_10"":[
                        ""9022531937""
                     ]
                  },
                  ""cover"":{
                     ""small"":""https://covers.openlibrary.org/b/id/8666743-S.jpg"",
                     ""large"":""https://covers.openlibrary.org/b/id/8666743-L.jpg"",
                     ""medium"":""https://covers.openlibrary.org/b/id/8666743-M.jpg""
                  },
                  ""subject_places"":[
                     {
                        ""url"":""https://openlibrary.org/subjects/place:middle_earth"",
                        ""name"":""Middle Earth""
                     },
                     {
                        ""url"":""https://openlibrary.org/subjects/place:mordor"",
                        ""name"":""Mordor""
                     }
                  ],
                  ""subjects"":[
                     {
                        ""url"":""https://openlibrary.org/subjects/elves"",
                        ""name"":""Elves""
                     },
                     {
                        ""url"":""https://openlibrary.org/subjects/dwarves"",
                        ""name"":""Dwarves""
                     }
                  ],
                  ""subject_people"":[
                     {
                        ""url"":""https://openlibrary.org/subjects/person:bilbo_baggins"",
                        ""name"":""Bilbo Baggins""
                     },
                     {
                        ""url"":""https://openlibrary.org/subjects/person:gandalf_the_wizard"",
                        ""name"":""Gandalf the Wizard""
                     },
                     {
                        ""url"":""https://openlibrary.org/subjects/person:frodo_baggins"",
                        ""name"":""Frodo Baggins""
                     }
                  ],
                  ""key"":""/books/OL27047721M"",
                  ""authors"":[
                     {
                        ""url"":""http://openlibrary.org/authors/OL1234/J.R.R._Tolkien"",
                        ""name"":""J.R.R. Tolkien""
                     }
                  ],
                  ""publish_date"":""Jul 01, 2002"",
                  ""excerpts"":[
                     {
                        ""comment"":""first sentence"",
                        ""text"":""This book is largely concerned with Hobbits, and from its pages a reader may discover much of their character and a little of their history.""
                     }
                  ],
                  ""subject_times"":[
                     {
                        ""url"":""https://openlibrary.org/subjects/time:before_the_age_of_men"",
                        ""name"":""Before the Age of Men""
                     }
                  ]
            }";
    }
}
