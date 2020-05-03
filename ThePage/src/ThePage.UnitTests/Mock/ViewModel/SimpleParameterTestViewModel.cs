using System;
using MvvmCross.ViewModels;

namespace ThePage.UnitTests
{
    public class SimpleParameterTestViewModel : MvxViewModel<string>
    {
        public string Parameter { get; set; }

        public virtual void Init()
        {
        }

        public override void Prepare(string parameter)
        {
            Parameter = parameter;
        }
    }
}