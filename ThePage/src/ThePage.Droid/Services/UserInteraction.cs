using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Platforms.Android;
using ThePage.Core;

namespace ThePage.Droid
{
    public class UserInteraction : IUserInteraction
    {
        protected Activity CurrentActivity => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            Confirm(message, confirmed =>
            {
                if (confirmed)
                    okClicked();
            },
            title, okButton, cancelButton);
        }

        public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            //Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction();
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null)
                    return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetPositiveButton(okButton, delegate
                        {
                            if (answer != null)
                                answer(true);
                        })
                        .SetNegativeButton(cancelButton, delegate
                        {
                            if (answer != null)
                                answer(false);
                        }).SetCancelable(false)
                        .Show();
            }, null);
        }

        public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
        {
            var tcs = new TaskCompletionSource<bool>();
            Confirm(message, tcs.SetResult, title, okButton, cancelButton);
            return tcs.Task;
        }

        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No",
            string neutral = "Maybe")
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null)
                    return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetPositiveButton(positive, delegate
                        {
                            if (answer != null)
                                answer(ConfirmThreeButtonsResponse.Positive);
                        })
                        .SetNegativeButton(negative, delegate
                        {
                            if (answer != null)
                                answer(ConfirmThreeButtonsResponse.Negative);
                        })
                        .SetNeutralButton(neutral, delegate
                        {
                            if (answer != null)
                                answer(ConfirmThreeButtonsResponse.Neutral);
                        })
                        .Show();
            }, null);
        }

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No",
            string neutral = "Maybe")
        {
            var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
            ConfirmThreeButtons(message, tcs.SetResult, title, positive, negative, neutral);
            return tcs.Task;
        }

        public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null)
                    return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetPositiveButton(okButton, delegate
                        {
                            if (done != null)
                                done();
                        })
                        .Show();
            }, null);
        }

        public Task AlertAsync(string message, string title = "", string okButton = "OK")
        {
            var tcs = new TaskCompletionSource<object>();
            Alert(message, () => tcs.SetResult(null), title, okButton);
            return tcs.Task;
        }

        public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            Input(message, (ok, text) =>
            {
                if (ok)
                    okClicked(text);
            },
                placeholder, title, okButton, cancelButton, initialText);
        }

        public void Input(string message, Action<bool, string> answer, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null)
                    return;
                var input = new EditText(CurrentActivity) { Hint = hint, Text = initialText };
                input.SetPadding(5, 0, 5, 0);

                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetView(input)
                        .SetPositiveButton(okButton, delegate
                        {
                            answer?.Invoke(true, input.Text);
                        })
                        .SetNegativeButton(cancelButton, delegate
                        {
                            answer?.Invoke(false, input.Text);
                        })
                        .Show();
            }, null);
        }

        public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            var tcs = new TaskCompletionSource<InputResponse>();
            Input(message, (ok, text) => tcs.SetResult(new InputResponse { Ok = ok, Text = text }), placeholder, title, okButton, cancelButton, initialText);
            return tcs.Task;
        }

        public void ToastMessage(string message)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null)
                    return;

                //Toast.MakeText(CurrentActivity, message, ToastLength.Long).Show();
                ToastMessage(message, EToastType.Error);

            }, null);
        }

        public void ToastMessage(string message, EToastType type)
        {
            LayoutInflater inflater = CurrentActivity.LayoutInflater;
            var view = inflater.Inflate(Resource.Layout.custom_toast, null);

            var layout = view.FindViewById<LinearLayout>(Resource.Id.toast);
            var img = view.FindViewById<ImageView>(Resource.Id.imgCustomToast);
            var txt = view.FindViewById<TextView>(Resource.Id.txtCustomToast);

            //Set Layout values
            layout.SetBackgroundColor(GetToastBackgroundColor(type));

            //Set ImageView values
            img.SetImageResource(GetToastTypeImage(type));
            img.SetColorFilter(Color.Argb(255, 255, 255, 255)); // White Tint
            img.Drawable.SetColorFilter(GetToastTextColor(type), PorterDuff.Mode.SrcIn);

            //Set TextView values
            txt.Text = message;
            txt.SetTextColor(GetToastTextColor(type));

            //Show Toast
            var toast = new Toast(CurrentActivity)
            {
                Duration = ToastLength.Short,
                View = view
            };
            toast.Show();

            static int GetToastTypeImage(EToastType type)
            {
                switch (type)
                {
                    case EToastType.Error:
                        return Resource.Drawable.ic_error;
                    case EToastType.Success:
                        return Resource.Drawable.ic_check;
                    case EToastType.Other:
                        return 0;
                    default:
                        return Resource.Drawable.ic_error;
                }
            }

            Color GetToastTextColor(EToastType type)
            {
                int resourceColor;
                switch (type)
                {
                    case EToastType.Error:
                        resourceColor = Resource.Color.primaryLightColorError;
                        break;
                    case EToastType.Success:
                        resourceColor = Resource.Color.primaryLightColorSuccess;
                        break;
                    case EToastType.Other:
                    default:
                        resourceColor = Resource.Color.black;
                        break;
                }
                return new Color(ContextCompat.GetColor(CurrentActivity, resourceColor));
            }

            Color GetToastBackgroundColor(EToastType type)
            {
                int resourceColor;
                switch (type)
                {
                    case EToastType.Error:
                        resourceColor = Resource.Color.primaryDarkColorError;
                        break;
                    case EToastType.Success:
                        resourceColor = Resource.Color.primaryDarkColorSuccess;
                        break;
                    case EToastType.Other:
                    default:
                        resourceColor = Resource.Color.white;
                        break;
                }
                return new Color(ContextCompat.GetColor(CurrentActivity, resourceColor));
            }
        }
    }
}