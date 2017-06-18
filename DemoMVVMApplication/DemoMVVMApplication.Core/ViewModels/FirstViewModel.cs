using DemoMVVMApplication.Core.Services;
using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace DemoMVVMApplication.Core.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {
        private IWifiWrapper wifiWrapper;
        public FirstViewModel(IWifiWrapper wifiWrapper)
        {
            this.wifiWrapper = wifiWrapper;
            wifiWrapper.WifiStateChanged += WifiWrapper_WifiStateChanged;
        }

        private string ssid = "initializing...";
        public string Ssid
        {
            get { return ssid; }
            set { SetProperty(ref ssid, value); }
        }
        private string interfaceName;
        public string InterfaceName
        {
            get { return interfaceName; }
            set { SetProperty(ref interfaceName, value); }
        }
        private int interfaceId;
        public int InterfaceId
        {
            get { return interfaceId; }
            set { SetProperty(ref interfaceId, value); }
        }
        private bool isInsecure;
        public bool IsInsecure
        {
            get { return isInsecure; }
            set { SetProperty(ref isInsecure, value); }
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            wifiWrapper.Initialize();
            base.InitFromBundle(parameters);
        }

        private void WifiWrapper_WifiStateChanged(WifiState state)
        {
            InterfaceName = wifiWrapper.CurrentState.InterfaceName;
            Ssid = wifiWrapper.CurrentState.SSID;
            InterfaceId = wifiWrapper.CurrentState.InterfaceIndex;
            IsInsecure = wifiWrapper.CurrentState.IsInsecure;

        }
    }
}
