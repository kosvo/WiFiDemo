
using System;
namespace DemoMVVMApplication.Core.Services
{
        public delegate void WifiNotification(WifiState state);

        public interface IWifiWrapper
        {
            event WifiNotification WifiStateChanged;

            WifiState CurrentState{ get; }

            void Initialize();

            void Shutdown();
        }

        public class WifiState
        {

            public bool IsInsecure;

            public int InterfaceIndex;

            public string InterfaceName;

            public string SSID;

        }

   
}
