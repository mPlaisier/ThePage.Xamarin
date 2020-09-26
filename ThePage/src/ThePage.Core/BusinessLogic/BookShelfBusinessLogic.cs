using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class BookShelfBusinessLogic
    {
        public static ApiBookShelfRequest CreateApiBookShelfRequestFromInput(IEnumerable<ICell> items, string id = null)
        {
            //Name
            var name = items.OfType<CellBookShelfTextView>().Where(p => p.InputType == EBookShelfInputType.Name).First().TxtInput.Trim();

            //Books
            var books = items.OfType<CellBookShelfBookItem>().Select(i => i.Book).ToList();

            //Create Request
            return new ApiBookShelfRequest(id, name, books.GetIdStrings(true));
        }
    }
}
