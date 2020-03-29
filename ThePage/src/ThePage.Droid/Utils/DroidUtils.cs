using Android.App;
using Android.Content;
using Android.Views.InputMethods;

namespace ThePage.Droid
{
    public static class DroidUtils
    {
        public static void HideKeyboard(Activity context)
        {
            var imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);

            if (context.Window.CurrentFocus == null)
                return;
            {
                imm.HideSoftInputFromWindow(context.Window.CurrentFocus.WindowToken, 0);
            }
        }
    }
}
