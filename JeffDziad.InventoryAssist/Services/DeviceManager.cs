using JeffDziad.InventoryAssist.Native;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace JeffDziad.InventoryAssist.Services
{
    public class DeviceManager
    {
        public void test()
        {
            var devices = RawInput.GetDeviceList();

            foreach(var device in devices)
            {
                Debug.WriteLine(RawInput.GetDeviceName(device.hDevice));
            }
        }
    }

}
