using JeffDziad.InventoryAssist.Models;
using System;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace JeffDziad.InventoryAssist.Native
{
    public class RawInput
    {
        public const int WM_INPUT = 0x00FF;
        public const uint RID_INPUT = 0x10000003;
        public const uint RIDI_DEVICENAME = 0x20000007;
        public const uint RIM_TYPEMOUSE = 0;
        public const uint RIM_TYPEKEYBOARD = 1;
        public const uint RIM_TYPEHID = 2;

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICELIST
        {
            public IntPtr hDevice;
            public uint dwType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICE
        {
            public ushort usUsagePage;
            public ushort usUsage;
            public uint dwFlags;
            public IntPtr hwndTarget;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTHEADER
        {
            public uint dwType;
            public uint dwSize;
            public IntPtr hDevice;
            public IntPtr wParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWKEYBOARD
        {
            public ushort MakeCode;
            public ushort Flags;
            public ushort Reserved;
            public ushort VKey;
            public uint Message;
            public uint ExtraInformation;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct RAWINPUT
        {
            [FieldOffset(0)]
            public RAWINPUTHEADER header;

            [FieldOffset(16)]
            public RAWKEYBOARD keyboard;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterRawInputDevices(
            RAWINPUTDEVICE[] devices, 
            uint numDevices, 
            uint size);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetRawInputData(
            IntPtr hRawInput, 
            uint command, 
            IntPtr data, 
            ref uint size, 
            uint headerSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetRawInputDeviceList(
            IntPtr deviceList, 
            ref uint deviceCount, 
            uint structSize);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint GetRawInputDeviceInfo(
            IntPtr device, 
            uint command, 
            IntPtr data, 
            ref uint size);

        public static List<RawInputDevice> GetDeviceList()
        {
            uint deviceCount = 0;
            GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)Marshal.SizeOf<RAWINPUTDEVICELIST>());

            List<RawInputDevice> devices = new();

            int size = Marshal.SizeOf<RAWINPUTDEVICELIST>();
            IntPtr buffer = Marshal.AllocHGlobal(size * (int)deviceCount);

            try
            {
                GetRawInputDeviceList(
                    buffer, 
                    ref deviceCount, 
                    (uint)Marshal.SizeOf<RAWINPUTDEVICELIST>());

                for(int i = 0; i < deviceCount; i++)
                {
                    IntPtr current = IntPtr.Add(buffer, i * size);
                    var device = Marshal.PtrToStructure<RAWINPUTDEVICELIST>(current);
                    devices.Add(new RawInputDevice
                    {
                        Handle = device.hDevice,
                        DeviceType = (RawInputDeviceType)device.dwType,
                    });
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return devices;
        }

        public static string GetDeviceName(IntPtr deviceHandle)
        {
            uint size = 0;

            GetRawInputDeviceInfo(deviceHandle, RIDI_DEVICENAME, IntPtr.Zero, ref size);

            if (size == 0) return string.Empty;

            IntPtr buffer = Marshal.AllocHGlobal((int)size);

            try
            {
                uint result = GetRawInputDeviceInfo(deviceHandle, RIDI_DEVICENAME, buffer, ref size);

                if (result == uint.MaxValue) return string.Empty;

                return Marshal.PtrToStringUni(buffer) ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}

