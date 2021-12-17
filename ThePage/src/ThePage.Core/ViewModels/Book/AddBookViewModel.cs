using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.Cells;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddBookParameter
    {
        #region Properties

        public string ISBN { get; }

        public OLObject Book { get; }

        #endregion

        #region Constructor

        public AddBookParameter(string isbn, OLObject book)
        {
            ISBN = isbn;
            Book = book;
        }

        #endregion
    }

    public class AddBookViewModel : BaseViewModel<AddBookParameter, string>
    {
        readonly IMvxNavigationService _navigation;
        readonly IAddBookScreenManagerService _screenManager;

        AddBookParameter _parameter;

        #region Properties

        public override string LblTitle => "Add book";

        public MvxObservableCollection<ICellBook> Items { get; set; } = new MvxObservableCollection<ICellBook>();

        #endregion

        #region Constructor

        public AddBookViewModel(IMvxNavigationService navigation,
                                IAddBookScreenManagerService screenManager)
        {
            _navigation = navigation;
            _screenManager = screenManager;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(AddBookParameter parameter)
        {
            _parameter = parameter;
        }

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddBookViewModel)}");
            await base.Initialize();

            _screenManager.Init(_parameter, Close);
            _screenManager.PropertyChanged += _screenManager_PropertyChanged;

            await _screenManager.FetchData();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _screenManager.PropertyChanged -= _screenManager_PropertyChanged;
        }

        #endregion

        #region Private

        void Close(string result)
        {
            _navigation.Close(this, result);
        }

        void _screenManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(_screenManager.Items)))
            {
                Items = _screenManager.Items;
            }
            else if (e.PropertyName.Equals(nameof(_screenManager.IsLoading)))
            {
                IsLoading = _screenManager.IsLoading;
            }
        }

        #endregion
    }
}