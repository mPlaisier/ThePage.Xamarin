using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class BookShelfBusinessLogic
    {
        public static (ApiBookShelfRequest request, IEnumerable<ApiBook> books) CreateApiBookShelfRequestFromInput(IEnumerable<ICell> items, string id = null, ApiBookShelfDetailResponse originalResponse = null)
        {
            //Name
            var name = items.OfType<CellBookShelfTextView>().First(p => p.InputType == EBookShelfInputType.Name).TxtInput.Trim();
            if (name == null || name.Equals(originalResponse?.Name))
                name = null;

            //Books
            var books = items.OfType<CellBookShelfBookItem>().Select(i => i.Book).ToList();
            if (books == null || (books.Count == originalResponse?.Books.Count && !books.Except(originalResponse?.Books).Any()))
                books = null;

            return (name == null && books == null
                ? null
                : new ApiBookShelfRequest(id, name, books.GetIdStrings(true)), books);
        }
    }
}
