using ManagedNativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WiFiAutoConnector
{
    /*
     * A simple dummy device with some simple commands to control its state
     */
    public interface IDeviceManager
    {
        DeviceStatus Status { get; }
        bool IsConnected { get; }
        NetworkIdentifier ConnectedNetworkIdentifier { get; }
        AvailableNetworkPack ConnectedNetwork { get; }

        IEnumerable<AvailableNetworkPack> GetAvailableNetworks();
        Task<bool> Connect(string ssid);
        void StartAutoConnect(string ssid);
        void StopAutoConnect();

        void Initialise();
        void Start();
        void Stop();
        void Terminate();
    }
}
