using ManagedNativeWifi;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Timer = System.Windows.Forms.Timer;

namespace WiFiAutoConnector
{
    public class DeviceManager : IDeviceManager
    {
        public DeviceManager()
        {
            Status = DeviceStatus.Uninitialised;
        }

        public delegate void StatusChangeEvent();
        public event StatusChangeEvent OnStatusChange;

        private Timer _scanNetworksTimer = null;
        private string _autoconnectTo;
        private bool _autoconnect = false;

        private async void _scanNetworksTimer_Tick(object? sender, EventArgs e)
        {
            await NativeWifi.ScanNetworksAsync(timeout: TimeSpan.FromSeconds(10));

            if (_autoconnect && _autoconnectTo != null) 
            {
                if (!IsConnected || _autoconnectTo != ConnectedNetworkIdentifier.ToString())
                {
                    await Connect(_autoconnectTo);
                }
            }

            InvokeEvent();
        }

        private void InvokeEvent()
        {
            OnStatusChange?.Invoke();
        }

        #region IDeviceManager

        public DeviceStatus Status { get; private set; }
        public bool IsConnected { get => NativeWifi.EnumerateConnectedNetworkSsids().Count() > 0; }
        public NetworkIdentifier ConnectedNetworkIdentifier { get => NativeWifi.EnumerateConnectedNetworkSsids().FirstOrDefault(); }
        public AvailableNetworkPack ConnectedNetwork { get => NativeWifi.EnumerateAvailableNetworks().Where((x) => x.Ssid == ConnectedNetworkIdentifier).FirstOrDefault(); }

        public async Task<bool> Connect(string ssid)
        {
            NativeWifi.EnumerateAvailableNetworks();

            AvailableNetworkPack network = GetAvailableNetworks().Where(x => x.Ssid.ToString() == ssid).FirstOrDefault();
            bool success = false;

            if (network != null)
            {
                success = await NativeWifi.ConnectNetworkAsync(
                interfaceId: network.Interface.Id,
                profileName: network.ProfileName,
                bssType: network.BssType,
                timeout: TimeSpan.FromSeconds(10));
            }

            InvokeEvent();
            return success;
        }

        public IEnumerable<AvailableNetworkPack> GetAvailableNetworks()
        {
            return NativeWifi.EnumerateAvailableNetworks()
                .Where(x => !string.IsNullOrWhiteSpace(x.ProfileName))
                .OrderByDescending(x => x.SignalQuality);
        }

        public async void StartAutoConnect(string ssid)
        {
            if (_autoconnect != true)
            {
                _autoconnect = true;
                _autoconnectTo = ssid;
                await Connect(ssid);
            }
        }

        public void StopAutoConnect()
        {
            _autoconnect = false;
            _autoconnectTo = null;
        }

        public void Initialise()
        {
            if (Status == DeviceStatus.Uninitialised)
            {
                Status = DeviceStatus.Initialising;

                _scanNetworksTimer = new Timer();
                _scanNetworksTimer.Interval = 60 * 1000;
                _scanNetworksTimer.Tick += _scanNetworksTimer_Tick;

                Status = DeviceStatus.Initialised;
            }
        }

        public void Start()
        {
            _scanNetworksTimer_Tick(null, null);
            _scanNetworksTimer.Start();
        }

        public void Stop()
        {
            _scanNetworksTimer.Stop();
        }

        public void Terminate()
        {
            Stop();
            Status = DeviceStatus.Uninitialised;
        }

        #endregion
    }
}
