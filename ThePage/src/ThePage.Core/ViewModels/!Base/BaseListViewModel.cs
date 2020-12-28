using System.Threading.Tasks;

namespace ThePage.Core.ViewModels
{
    public abstract class BaseListViewModel : BaseViewModel
    {
        protected int _currentPage;
        protected bool _hasNextPage;
        protected bool _isLoadingNextPage;

        protected string _search;
        protected bool _isSearching;

        #region Methods

        public abstract Task LoadNextPage();

        public abstract Task Search(string search);

        public abstract void StopSearch();

        #endregion
    }
}