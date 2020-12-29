using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Graphics;
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
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null)
                    return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetPositiveButton(okButton, delegate
                        {
                            answer?.Invoke(true);
                        })
                        .SetNegativeButton(cancelButton, delegate
                        {
                            answer?.Invoke(false);
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
                            answer?.Invoke(ConfirmThreeButtonsResponse.Positive);
                        })
                        .SetNegativeButton(negative, delegate
                        {
                            answer?.Invoke(ConfirmThreeButtonsResponse.Negative);
                        })
                        .SetNeutralButton(neutral, delegate
                        {
                            answer?.Invoke(ConfirmThreeButtonsResponse.Neutral);
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
                            done?.Invoke();
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

                Toast.MakeText(CurrentActivity, message, ToastLength.Long).Show();
            }, null);
        }

        public void ToastMessage(string message, EToastType type)
        {
            Application.SynchronizationContext.Post(ignored =>
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
                img.SetColorFilter(BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(GetToastTextColor(type), BlendModeCompat.SrcIn));

                //Set TextView values
                txt.Text = message;
                txt.SetTextColor(GetToastTextColor(type));

                //Show Toast
                var toast = new Toast(CurrentActivity)
                {
                    Duration = ToastLength.Short,
                    View = view,
                };
                toast.SetGravity(GravityFlags.Bottom | GravityFlags.FillHorizontal, 0, 0);

                toast.Show();
            }, null);

            static int GetToastTypeImage(EToastType type)
            {
                return type switch
                {
                    EToastType.Error => Resource.Drawable.ic_error,
                    EToastType.Success => Resource.Drawable.ic_check,
                    EToastType.Info => Resource.Drawable.ic_info_outline,
                    EToastType.Other => 0,
                    _ => Resource.Drawable.ic_error,
                };
            }

            Color GetToastTextColor(EToastType type)
            {
                var resourceColor = type switch
                {
                    EToastType.Error => Resource.Color.primaryLightColorError,
                    EToastType.Success => Resource.Color.primaryLightColorSuccess,
                    EToastType.Info => Resource.Color.black,
                    _ => Resource.Color.black,
                };
                return new Color(ContextCompat.GetColor(CurrentActivity, resourceColor));
            }

            Color GetToastBackgroundColor(EToastType type)
            {
                var resourceColor = type switch
                {
                    EToastType.Error => Resource.Color.primaryDarkColorError,
                    EToastType.Success => Resource.Color.primaryDarkColorSuccess,
                    EToastType.Info => Resource.Color.primaryLightColor,
                    _ => Resource.Color.white,
                };
                return new Color(ContextCompat.GetColor(CurrentActivity, resourceColor));
            }
        }
    }
}