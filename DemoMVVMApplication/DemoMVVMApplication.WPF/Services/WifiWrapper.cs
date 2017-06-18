using DemoMVVMApplication.Core.Services;
using SimpleWifi;
using SimpleWifi.Win32;
using SimpleWifi.Win32.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoMVVMApplication.WPF.Services
{
    public class WifiWrapper : IWifiWrapper
    {
        private WlanClient _client;
        private WifiStatus _connectionStatus;
        private bool _isConnectionStatusSet = false;
        public bool NoWifiAvailable = false;

        private WifiState currentState;
        public WifiState CurrentState => currentState;


        public event WifiNotification WifiStateChanged;

        public void Initialize()
        {
            _client = new WlanClient();
           
            NoWifiAvailable = _client.NoWifiAvailable;
            if (_client.NoWifiAvailable)
                return;

            foreach (var inte in _client.Interfaces) {
                inte.WlanNotification += Inte_WlanNotification;
                inte.Scan();
            }
            
        }

        private void Inte_WlanNotification(WlanNotificationData notifyData)
        {
            var listofPoints = GetAccessPoints();
            currentState = new WifiState();
            foreach (var network in listofPoints)
            {
                var networkInterface = _client.Interfaces.Where(i => i.CurrentConnection.profileName == network.profileName).FirstOrDefault();
                if (networkInterface != null)
                {
                    currentState.SSID = Encoding.ASCII.GetString(network.dot11Ssid.SSID, 0, (int)network.dot11Ssid.SSIDLength);
                    currentState.IsInsecure = !network.securityEnabled;
                    for (int i = 0; i < _client.Interfaces.Length; i++)
                    {
                        var interf = _client.Interfaces[i];
                        if (interf.InterfaceGuid == notifyData.interfaceGuid)
                        {
                            currentState.InterfaceName = interf.InterfaceName;
                            currentState.InterfaceIndex = i;
                        }
                    }
                }
            }

            WifiStateChanged.Invoke(this.CurrentState);

        }

        private List<WlanAvailableNetwork> GetAccessPoints()
        {
            List<WlanAvailableNetwork> accessPoints = new List<WlanAvailableNetwork>();
            if (_client.NoWifiAvailable)
                return accessPoints;

            foreach (WlanInterface wlanIface in _client.Interfaces)
            {
                WlanAvailableNetwork[] rawNetworks = wlanIface.GetAvailableNetworkList(0);
                List<WlanAvailableNetwork> networks = new List<WlanAvailableNetwork>();

                // Remove network entries without profile name if one exist with a profile name.
                foreach (WlanAvailableNetwork network in rawNetworks)
                {
                    bool hasProfileName = !string.IsNullOrEmpty(network.profileName);
                    bool anotherInstanceWithProfileExists = rawNetworks.Where(n => n.Equals(network) && !string.IsNullOrEmpty(n.profileName)).Any();

                    if (!anotherInstanceWithProfileExists || hasProfileName)
                        accessPoints.Add(network);
                }
            }

            return accessPoints;
        }
        public void Shutdown()
        {

            if (_client.NoWifiAvailable)
                return;

            foreach (WlanInterface wlanIface in _client.Interfaces)
            {
                wlanIface.Disconnect();
            }

        }
    }
}
