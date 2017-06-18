using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using DemoMVVMApplication.Core.ViewModels;

namespace DemoMVVMApplication.iOS.Views
{
    [MvxFromStoryboard]
    public partial class FirstView : MvxViewController
    {
        public FirstView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<FirstView, FirstViewModel>();
            set.Bind(Label).To(vm => vm.Ssid);
            set.Bind(TextField).To(vm => vm.Ssid);
            set.Apply();
        }
    }
}
