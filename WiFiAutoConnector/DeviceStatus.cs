using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiFiAutoConnector
{
    public enum DeviceStatus
    {
        Uninitialised,
        Initialising,
        Initialised,
        Started,
        Stopped,
        Connecting,
        Connected,
        Disconnected,
        Error,
    }
}
