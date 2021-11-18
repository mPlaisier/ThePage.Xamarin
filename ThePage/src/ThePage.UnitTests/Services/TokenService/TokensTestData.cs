using System;
using Newtonsoft.Json;
using ThePage.Api;

namespace ThePage.UnitTests
{
    public static class TokensTestData
    {

        public static ApiTokens GetValidTokens()
        {
            return JsonConvert.DeserializeObject<ApiTokens>(ValidTokens);
        }

        public static ApiTokens GetInValidTokens()
        {
            return JsonConvert.DeserializeObject<ApiTokens>(InValidTokens);
        }

        public static string ValidTokens = @"
            {
                ""access"": {
                    ""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1ZWQ5NDNkOTNiYjU0MDRhZmI2NTgzNzQiLCJpYXQiOjE2MzMyMTExNjksImV4cCI6MTYzMzIxMjk2OX0.8Z70uogLGGvPizjg9oeIUbmr-jjsoAaRimJXLC8yWlE"",
                    ""expires"": ""2100-10-02T00:00:00.000Z""
                },
                ""refresh"": {
                    ""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1ZWQ5NDNkOTNiYjU0MDRhZmI2NTgzNzQiLCJpYXQiOjE2MzMyMTExNjksImV4cCI6MTYzNTgwMzE2OX0.NfvSY1hbYs5m3QtubKH3al06nMXKrsNGt9ghujj90Lo"",
                    ""expires"": ""2100-11-01T00:00:00.000Z""
                }
            }
        ";

        public static string InValidTokens = @"
            {
                ""access"": {
                    ""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1ZWQ5NDNkOTNiYjU0MDRhZmI2NTgzNzQiLCJpYXQiOjE2MzMyMTExNjksImV4cCI6MTYzMzIxMjk2OX0.8Z70uogLGGvPizjg9oeIUbmr-jjsoAaRimJXLC8yWlE"",
                    ""expires"": ""2000-10-02T00:00:00.000Z""
                },
                ""refresh"": {
                    ""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1ZWQ5NDNkOTNiYjU0MDRhZmI2NTgzNzQiLCJpYXQiOjE2MzMyMTExNjksImV4cCI6MTYzNTgwMzE2OX0.NfvSY1hbYs5m3QtubKH3al06nMXKrsNGt9ghujj90Lo"",
                    ""expires"": ""2000-11-01T00:00:00.000Z""
                }
            }
        ";
    }
}
