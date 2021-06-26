using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class AuthorDataFactory
    {


        public static Author GetSingleAuthor()
        {
            return GetFakeAuthor().Generate();
        }

        public static IEnumerable<Author> GetListAuthor4ElementsComplete()
        {
            return GetFakeAuthor().Generate(4);
        }

        public static IEnumerable<Author> GetListAuthorEmpty()
        {
            return Enumerable.Empty<Author>();
        }

        #region private

        static Faker<Author> GetFakeAuthor()
        {
            return new Faker<Author>()
                .RuleFor(a => a.Id, f => Guid.NewGuid().ToString())
                .RuleFor(a => a.Name, f => f.Person.FullName);
        }

        #endregion
    }
}
