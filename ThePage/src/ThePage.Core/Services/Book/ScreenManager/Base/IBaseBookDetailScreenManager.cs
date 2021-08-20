using System.ComponentModel;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using ThePage.Core.Cells;

namespace ThePage.Core
{
    public interface IBaseBookDetailScreenManager : INotifyPropertyChanged
    {
        #region Properties

        bool IsLoading { get; }

        MvxObservableCollection<ICellBook> Items { get; }

        #endregion

        #region Methods

        void CreateCellBooks(BookDetail bookDetail);

        Task SaveBook();

        Task FetchData();

        #endregion
    }
}
