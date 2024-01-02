using System.Diagnostics;
using System.Reflection;

namespace WiFiAutoConnector
{
    public class ViewManager
    {
        public ViewManager(IDeviceManager deviceManager)
        {
            System.Diagnostics.Debug.Assert(deviceManager != null);

            _deviceManager = deviceManager;

            _components = new System.ComponentModel.Container();
            _notifyIcon = new System.Windows.Forms.NotifyIcon(_components)
            {
                ContextMenuStrip = new ContextMenuStrip() { Renderer = new ToolStripProfessionalRenderer(), },
                Icon = Properties.Resources.WiFi,
                Text = "WiFi Auto Connector",
                Visible = true,
            };

            _notifyIcon.ContextMenuStrip.Opening += _ContextMenuStrip_Opening; ;
            _notifyIcon.MouseUp += _notifyIcon_MouseUp;

            _connectedNetworkMenuItem = new ToolStripMenuItem("No Network");
            _wifiNetworksMenuItem = new ToolStripMenuItem("WiFi Networks");
            _autoconnectButton = new ToolStripButton("Autoconnect", null, _autoconnectButton_Click)
            {
                CheckOnClick = true,
                Checked = Settings.Instance.IsAutoconnect,
            };

            _exitMenuItem = new ToolStripMenuItem("&Exit", null, _exitItem_Click)
            {
                ToolTipText = "Exits System Tray App",
            };

            OnStatusChange();
        }

        private System.ComponentModel.IContainer _components;

        private NotifyIcon _notifyIcon;
        IDeviceManager _deviceManager;


        private ToolStripMenuItem _connectedNetworkMenuItem;
        private ToolStripMenuItem _wifiNetworksMenuItem;
        private ToolStripButton _autoconnectButton;
        private ToolStripMenuItem _exitMenuItem;

        private void DisplayStatusMessage(string text)
        {
            _notifyIcon.BalloonTipText = text;
            _notifyIcon.ShowBalloonTip(3000);
        }

        public void OnStatusChange()
        {
            if (!string.IsNullOrEmpty(Settings.Instance.Target))
            {
                _connectedNetworkMenuItem.Text = "Target: " + Settings.Instance.Target;
            }
            else
            {
                _connectedNetworkMenuItem.Text = "Not Connected";
            }

            if (Settings.Instance.IsAutoconnect)
            {
                _deviceManager.StartAutoConnect(Settings.Instance.Target);
            }
            else
            {
                _deviceManager.StopAutoConnect();

            }

            _wifiNetworksMenuItem.DropDownItems.Clear();
            foreach (var item in _deviceManager.GetAvailableNetworks())
            {
                ToolStripMenuItem networkItem = new ToolStripMenuItem(item.Ssid.ToString(), null, (o, e) => _connect(item.Ssid.ToString()));
                
                _wifiNetworksMenuItem.DropDownItems.Add(networkItem);
            }

            _autoconnectButton.Image = _autoconnectButton.Checked ? Properties.Resources.Check.ToBitmap() : Properties.Resources.CheckEmpty.ToBitmap();   
        }

        private void _connect(string ssid)
        {
            Settings.Instance.Target = ssid;
            _deviceManager.Connect(ssid);
            DisplayStatusMessage($"Connecting to {ssid}");
        }

        private void _showWebSite_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo() { FileName = "https://github.com/JosWigchert/WiFiAutoConnector", UseShellExecute = true, });
        }

        private void _exitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void _autoconnectButton_Click(object sender, EventArgs e)
        {
            if (_deviceManager.IsConnected)
            {
                Settings.Instance.IsAutoconnect = _autoconnectButton.Checked;
                OnStatusChange();
            }
        }

        private void _notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(_notifyIcon, null);
            }
        }

        private void _ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

            if (_notifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                _notifyIcon.ContextMenuStrip.Items.Add(_connectedNetworkMenuItem);
                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                
                _notifyIcon.ContextMenuStrip.Items.Add(_wifiNetworksMenuItem);
                _notifyIcon.ContextMenuStrip.Items.Add(_autoconnectButton);
                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("GitHub", null, _showWebSite_Click) { ToolTipText = "Navigates to the GitHub page" });
                _notifyIcon.ContextMenuStrip.Items.Add(_exitMenuItem);
            }
        }
    }
}
