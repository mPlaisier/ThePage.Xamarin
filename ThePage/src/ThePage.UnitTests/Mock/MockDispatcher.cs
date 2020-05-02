using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace ThePage.UnitTests
{
    public class MockDispatcher : MvxMainThreadDispatcher, IMvxViewDispatcher
    {
        #region Properties

        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

        public override bool IsOnMainThread => true;

        #endregion

        #region IMvxViewDispatcher

        Task<bool> IMvxViewDispatcher.ShowViewModel(MvxViewModelRequest request)
        {
            Requests.Add(request);
            return Task.FromResult(true);
        }

        Task<bool> IMvxViewDispatcher.ChangePresentation(MvxPresentationHint hint)
        {
            Hints.Add(hint);
            return Task.FromResult(true);
        }

        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            action();
            return Task.FromResult(true);
        }

        public Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            action();
            return Task.FromResult(true);
        }

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            action();
            return true;
        }

        #endregion
    }
}
