using System;

namespace JeffDziad.InventoryAssist.Models
{
    public class RawInputDevice
    {
        public IntPtr Handle { get; set; }
        public RawInputDeviceType DeviceType { get; set; }
        public string DevicePath { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;
        public bool IsConnected { get; set; } = true;
        public override string ToString()
        {
            return FriendlyName;
        }
    }
}

