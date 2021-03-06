using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace ThePage.Droid
{
    public class TextViewDrawableLeftBinding : MvxAndroidTargetBinding
    {
        #region Properties

        protected TextView View => (TextView)Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(string);

        #endregion

        #region Constructor

        public TextViewDrawableLeftBinding(TextView view) : base(view)
        {
        }

        #endregion

        #region Protected

        protected override void SetValueImpl(object target, object value)
        {
            if (value == null)
                return;

            var resources = AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier((string)value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
            View.SetCompoundDrawablesWithIntrinsicBounds(id, 0, 0, 0);
        }

        #endregion
    }
}
