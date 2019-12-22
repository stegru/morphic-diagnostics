using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mdiag
{
    public class FakeUsb
    {
        private const int WM_DEVICECHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x2;
        private const int DBTF_NET = 0x2;
        private const int HWND_BROADCAST = 0xffff;

        [StructLayout(LayoutKind.Sequential)]
        private struct DEV_BROADCAST_VOLUME
        {
            public int size;
            public int deviceType;
            public int reserved;
            public int unitmask;
            public int flags;
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, ref DEV_BROADCAST_VOLUME lParam);

        /// <summary>
        /// Notifies the system that a usb drive has been added or removed.
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <param name="addDevice"></param>
        public static void SendNotification(string driveLetter, bool addDevice)
        {
            string drive = driveLetter.Substring(0, 1).ToUpper();

            DEV_BROADCAST_VOLUME volume = new DEV_BROADCAST_VOLUME();
            volume.size = Marshal.SizeOf(volume.GetType());
            volume.deviceType = DBT_DEVTYP_VOLUME;
            volume.flags = 0;

            // Each bit represents a drive letter.
            int bit = Encoding.ASCII.GetBytes(drive)[0] - 0x41;
            volume.unitmask = 1 << bit;

            SendMessage(0xffff, WM_DEVICECHANGE, addDevice ? DBT_DEVICEARRIVAL : DBT_DEVICEREMOVECOMPLETE, ref volume);
        }
    }
}
