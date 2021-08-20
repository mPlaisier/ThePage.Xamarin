using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.Cells;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookDetailParameter
    {
        #region Properties

        public Book Book { get; }

        #endregion

        #region Constructor

        public BookDetailParameter(Book book)
        {
            Book = book;
        }

        #endregion
    }

    public class BookDetailViewModel : BaseViewModel<BookDetailParameter, bool>
    {
        readonly IBookDetailScreenManagerService _screenManager;
        readonly IMvxNavigationService _navigationService;

        #region Properties

        public MvxObservableCollection<ICellBook> Items { get; private set; } = new MvxObservableCollection<ICellBook>();

        public override string LblTitle => _screenManager.Title;

        readonly MvxInteraction _updateToolbarInteraction = new MvxInteraction();
        public IMvxInteraction UpdateToolbarInteraction => _updateToolbarInteraction;

        public bool IsEditing { get; private set; }

        #endregion

        #region Commands

        IMvxCommand _editbookCommand;
        public IMvxCommand EditBookCommand => _editbookCommand ??= new MvxCommand(_screenManager.ToggleEditValue);

        #endregion

        #region Constructor

        public BookDetailViewModel(IBookDetailScreenManagerService screenManager, IMvxNavigationService navigationService)
        {
            _screenManager = screenManager;
            _navigationService = navigationService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(BookDetailParameter parameter)
        {
            _screenManager.Init(parameter, Close);
        }

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookDetailViewModel)}");

            await base.Initialize();

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
            else if (e.PropertyName.Equals(nameof(_screenManager.IsEditing)))
            {
                IsEditing = _screenManager.IsEditing;
            }
            else if (e.PropertyName.Equals(nameof(_screenManager.Title)))
                _updateToolbarInteraction.Raise();
        }

        void Close() => _navigationService.Close(this, true);

        #endregion
    }
}