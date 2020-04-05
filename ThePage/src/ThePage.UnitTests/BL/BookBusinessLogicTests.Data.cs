using System;
namespace ThePage.UnitTests
{
    public partial class BookBusinessLogicTests
    {
        const string BookDataComplete =
            @"[
            {
                ""genres"":[
                    ""5e7fc0bbbc363c52e7d115ce""
                ],
                ""_id"":""5e8787a51d0ad60017ca1f22"",
                ""title"":""De reisgenoten"",
                ""author"":""5e80acc955926700174ef92c"",
                ""__v"":1
            },
            {
                ""genres"":[
                    ""5e7fc0bbbc363c52e7d115ce""
                ],
                ""_id"":""5e8787af1d0ad60017ca1f23"",
                ""title"":""De Twee Torens"",
                ""author"":""5e80acc955926700174ef92c"",
                ""__v"":1
            },
            {
                ""genres"":[
                    ""5e80755dd032d70017508517"",
                    ""5e7fc0bbbc363c52e7d115ce""
                ],
                ""_id"":""5e8787cd1d0ad60017ca1f24"",
                ""title"":""De Terugkeer van de Koning"",
                ""author"":""5e80acc955926700174ef92c"",
                ""__v"":1
            },
            {
                ""genres"":[
                    ""5e7fc0bbbc363c52e7d115ce"",
                    ""5e80755dd032d70017508517""
                ],
                ""_id"":""5e878aca1d0ad60017ca1f25"",
                ""title"":""Snow Like Ashes"",
                ""author"":""5e80ad1355926700174ef92f"",
                ""__v"":1
            },
            {
                ""genres"":[
                    ""5e7fc0bbbc363c52e7d115ce""
                ],
                ""_id"":""5e878b871d0ad60017ca1f27"",
                ""title"":""Het boek Jakab"",
                ""author"":""5e80acfc55926700174ef92e"",
                ""__v"":2
            },
            {
                ""genres"":[
                    ""5e80755dd032d70017508517"",
                    ""5e7fc0bbbc363c52e7d115ce""
                ],
                ""_id"":""5e878bb61d0ad60017ca1f28"",
                ""title"":""Het Bloed van de Keizer"",
                ""author"":""5e80ace555926700174ef92d"",
                ""__v"":1
            },
            {
                ""genres"":[
                    ""5e80755dd032d70017508517"",
                    ""5e7fc0bbbc363c52e7d115ce""
                ],
                ""_id"":""5e878c4d1d0ad60017ca1f29"",
                ""title"":""Het Vuur van de Keizer"",
                ""author"":""5e80ace555926700174ef92d"",
                ""__v"":2
            },
            {
                ""genres"":[
                    ""5e7fc0c9bc363c52e7d115cf""
                ],
                ""_id"":""5e878ca61d0ad60017ca1f2b"",
                ""title"":""The Subtle Art of not Giving a F*ck"",
                ""author"":""5e878c7e1d0ad60017ca1f2a"",
                ""__v"":0
            }]";

        const string BookDataEmpty = @"[]";
    }
}