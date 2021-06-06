
using System;
using System.Net;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Net;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace ThePage.Droid
{
    public class ImageViewUrlBinding : MvxAndroidTargetBinding
    {
        #region Properties

        protected ImageView View => (ImageView)Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(string);

        #endregion

        #region Constructor

        public ImageViewUrlBinding(ImageView view) : base(view)
        {
        }

        #endregion

        #region Protected

        protected override void SetValueImpl(object target, object value)
        {
            if (value == null)
                return;

            var imageBitmap = GetImageBitmapFromUrl((string)value);
            View.SetImageBitmap(imageBitmap);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;

        }



        #endregion
    }
}
