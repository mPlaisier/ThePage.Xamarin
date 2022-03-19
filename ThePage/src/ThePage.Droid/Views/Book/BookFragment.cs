using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Base;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using ThePage.Core;
using ThePage.Core.ViewModels.Main;
using ZXing.Mobile;
using static ThePage.Core.BookViewModel;

namespace ThePage.Droid
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel),
        Resource.Id.content_frame,
        AddToBackStack = true,
        EnterAnimation = Resource.Animation.pull_in_right,
        ExitAnimation = Resource.Animation.push_out_left,
        PopEnterAnimation = Resource.Animation.pull_in_left,
        PopExitAnimation = Resource.Animation.push_out_right
    )]
    public class BookFragment : BaseListFragment<BookViewModel>
    {
        GetIsbnCode _getIsbnCode;
        string _isbnCode;

        #region Properties

        protected override int FragmentLayoutId => Resource.Layout.fragment_book;

        IMvxInteraction<GetIsbnCode> _isbnInteraction;
        public IMvxInteraction<GetIsbnCode> ISBNInteraction
        {
            get => _isbnInteraction;
            set
            {
                if (_isbnInteraction != null)
                    _isbnInteraction.Requested -= OnISBNInteractionRequested;

                _isbnInteraction = value;
                if (_isbnInteraction != null)
                    _isbnInteraction.Requested += OnISBNInteractionRequested;
            }
        }

        #endregion

        #region LifeCycle

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.lstItems);

            int spanCount = 3;
            recyclerView.SetLayoutManager(new GridLayoutManager(Context, spanCount));

            int spacing = 25;
            bool includeEdge = true;
            recyclerView.AddItemDecoration(new GridSpacingItemDecoration(spanCount, spacing, includeEdge));

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            handleBarcodeScannerResult();
        }

        public override void OnViewModelSet()
        {
            base.OnViewModelSet();

            ISBNInteraction = ViewModel.ISBNInteraction;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            ISBNInteraction = null;
        }

        #endregion

        #region Private

        async void OnISBNInteractionRequested(object sender, MvxValueEventArgs<GetIsbnCode> eventArgs)
        {
            _getIsbnCode = eventArgs.Value;

            MobileBarcodeScanner.Initialize(Activity.Application);

            var MScanner = new MobileBarcodeScanner();
            var Result = await MScanner.Scan();

            _isbnCode = Result == null ? string.Empty : Result.Text;
        }

        void handleBarcodeScannerResult()
        {
            if (_getIsbnCode != null && _isbnCode != null)
            {
                _getIsbnCode.ISBNCallback(_isbnCode);

                _getIsbnCode = null;
                _isbnCode = null;
            }
        }

        #endregion
    }
}