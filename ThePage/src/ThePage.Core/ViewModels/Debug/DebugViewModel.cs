using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class DebugViewModel : BaseViewModel
    {
        readonly IUserInteraction _userInteraction;
        readonly IThePageService _thePageService;

        #region Properties

        public override string Title => "Debug Menu";

        public List<CellDebug> Items { get; internal set; }

        MvxInteraction<GetIsbnCode> _isbnInteraction = new MvxInteraction<GetIsbnCode>();
        public IMvxInteraction<GetIsbnCode> ISBNInteraction => _isbnInteraction;

        #endregion

        #region Commands

        private MvxCommand<CellDebug> _itemClickCommand;
        public MvxCommand<CellDebug> ItemClickCommand => _itemClickCommand = _itemClickCommand ?? new MvxCommand<CellDebug>(OnItemClick);

        #endregion

        #region Constructor

        public DebugViewModel(IUserInteraction userInteraction, IThePageService thePageService)
        {
            _userInteraction = userInteraction;
            _thePageService = thePageService;
        }

        #endregion 

        #region LifeCycle

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(DebugViewModel)}");

            CreateMenuItems();

            return base.Initialize();
        }

        #endregion

        #region Private

        void CreateMenuItems()
        {
            //Alert
            var debugTypeAlert = EDebugType.Alert;
            Items = new List<CellDebug>
            {
                new CellDebugHeader("Alerts",debugTypeAlert),
                new CellDebugItem("Confirm OK", debugTypeAlert,EDebugItemType.ConfirmOk),
                new CellDebugItem("Confirm answer", debugTypeAlert, EDebugItemType.ConfirmAnswer),
                //new CellDebugItem("Confirm async", debugTypeAlert, EDebugItemType.ConfirmAsync),
                new CellDebugItem("Alert",debugTypeAlert, EDebugItemType.Alert),
                //new CellDebugItem("Alert async", debugTypeAlert, EDebugItemType.AlerAsync),
                new CellDebugItem("Input ok", debugTypeAlert, EDebugItemType.InputOk),
                //new CellDebugItem("Input Answer", debugTypeAlert, EDebugItemType.InputAnwser),
                //new CellDebugItem("Input Async", debugTypeAlert, EDebugItemType.InputAsync),
                new CellDebugItem("Confirm three buttons", debugTypeAlert, EDebugItemType.ConfirmThreeButtons),
               // new CellDebugItem("Confirm three buttons async", debugTypeAlert, EDebugItemType.ConfirmThreeButtonsAsync),

                new CellDebugHeader("Toast",EDebugType.Toast),
               new CellDebugItem("Toast message", EDebugType.Toast,EDebugItemType.Toast),

               new CellDebugHeader("Data", EDebugType.Data),
               new CellDebugItem("Book not found error", EDebugType.Data,EDebugItemType.BookNotFound),

                new CellDebugHeader("BarCode",EDebugType.BarcodeScanner),
                new CellDebugItem("Open Scanner", EDebugType.BarcodeScanner,EDebugItemType.BarcodeScanner),
            };
        }

        void OnItemClick(CellDebug obj)
        {
            //TODO add logic to open en close section
            if (obj is CellDebugHeader)
                return;

            if (obj is CellDebugItem debug)
            {
                switch (debug.ItemType)
                {
                    case EDebugItemType.ConfirmOk:
                        _userInteraction.Confirm("Message", OkAction, "Custom title");
                        break;
                    case EDebugItemType.ConfirmAnswer:
                        _userInteraction.Confirm("Custom message", OkActionAnswer, "Custom title");
                        break;
                    case EDebugItemType.Alert:
                        _userInteraction.Alert("Custom message alert", AlertAction, "Custom Title");
                        break;
                    case EDebugItemType.InputOk:
                        _userInteraction.Input("Message", InputOK, "placeholder", "Custom title", "OK", "Cancel", "initial Text");
                        break;
                    case EDebugItemType.ConfirmThreeButtons:
                        _userInteraction.ConfirmThreeButtons("Custom message", ThreeButtonAnwer, "Custom Title");
                        break;

                    case EDebugItemType.Toast:
                        _userInteraction.ToastMessage("Custom message");
                        break;
                    case EDebugItemType.BookNotFound:
                        BookNotFoundCall().Forget();
                        break;
                    case EDebugItemType.BarcodeScanner:
                        StartBarcodeScanner();
                        break;
                    default:
                        break;
                }
            }

        }

        #endregion

        #region Alerts

        void OkAction()
        {
            System.Diagnostics.Debug.WriteLine("OK Action: Pressed OK", "DEBUG ALERTS");
        }

        void OkActionAnswer(bool answer)
        {
            System.Diagnostics.Debug.WriteLine($"OkActionAnswer Pressed {answer}", "DEBUG ALERTS");
        }

        void AlertAction()
        {
            System.Diagnostics.Debug.WriteLine("AlertAction Pressed OK", "DEBUG ALERTS");
        }

        private void InputOK(string obj)
        {
            System.Diagnostics.Debug.WriteLine($"InputOK Pressed {obj}", "DEBUG ALERTS");
        }

        private void ThreeButtonAnwer(ConfirmThreeButtonsResponse obj)
        {
            System.Diagnostics.Debug.WriteLine($"ThreeButtonAnwer Pressed {obj}", "DEBUG ALERTS");
        }

        #endregion

        #region Data

        async Task BookNotFoundCall()
        {
            await _thePageService.GetBook("5e4eca767f80d86dd7865ee8");
        }

        #endregion

        #region BarcodeScanner

        void StartBarcodeScanner()
        {
            var request = new GetIsbnCode
            {
                ISBNCallback = (isbn) =>
                {
                    if (isbn == null)
                        _userInteraction.Confirm("ISBN:" + isbn, OkAction, "Error");
                    else
                        _userInteraction.Confirm("ISBN:" + isbn, OkAction, "Success");
                }
            };

            _isbnInteraction.Raise(request);
        }

        #endregion

        public class GetIsbnCode
        {
            public Action<string> ISBNCallback { get; set; }
        }
    }
}